using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;
    public ParticleSystem particle;
    private ParticleSystem generatedParticle;
    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;
    Transform[,] m_grid;
    public int m_rowsCompleted = 0;
    public Action OnRowCompleted = delegate{}; 
    public Action<int> OnAllRowsCleared = delegate{};

    private void Awake()
    {
        m_grid = new Transform[m_width,m_height];
    }


    // Start is called before the first frame update
    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            if(child.position.y > m_height - m_header)
            {
                return true;
            }
        }

        return false;
    }

    bool IsOccupied(int x, int y, Shape shape)
    {
        return(m_grid[x,y] != null && m_grid[x,y].parent != shape.transform);
    }

    public void StoreShapeInGrid(Shape shape)
    {
        if(shape == null)
        {
            return;
        }

        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int) pos.x, (int) pos.y] = child;
        }
    }

    bool IsWithinBoard(int x, int y)
    {
        return x >= 0 && x < m_width && y>=0;
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            if(!IsWithinBoard((int) pos.x, (int) pos.y))
                return false;
            if(IsOccupied((int) pos.x, (int) pos.y, shape))
                return false;

        }

        return true;
    }

    void DrawEmptyCells()
    {
        if(m_emptySprite != null)
        {
            for(int y = 0; y < m_height - m_header; y++)
            {
                for(int x = 0; x < m_width; x++)
                {
                    Transform clone;
                    clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity);
                    clone.transform.parent = transform;
                    clone.name = "Board Space(x = " + x.ToString() + "," + "y = " + y.ToString() + ")";
                }
            }
        }
        else
        {
            Debug.Log("Please assign the sprite field in Board!!");
        }
       
        
    }

    private bool IsComplete(int y)
    {
        for(int x = 0; x < m_width; x++)
        {
            if(m_grid[x,y] == null)
                return false;
        }
        return true;
    }

    private void ClearRow(int y)
    {
        for(int x = 0; x < m_width; ++x)
        {
            if(m_grid[x,y] != null)
            {
                EmitParticles(m_grid[x,y].position);
                Destroy(m_grid[x,y].gameObject);
            }
                
            
            m_grid[x,y] = null;
        }
        OnRowCompleted?.Invoke();
       
       
    }

    private void EmitParticles(Vector3 position)
    {
        generatedParticle = Instantiate(particle, position, Quaternion.identity);
        generatedParticle.Play();       
    }

   

    private void ShiftOneRowDown(int y)
    {
        for(int x = 0; x < m_width; ++x)
        {
            if(m_grid[x,y] != null)
            {
                m_grid[x,y-1] = m_grid[x,y];
                m_grid[x,y] = null;
                m_grid[x,y-1].position += new Vector3(0, -1, 0); 
            }
        }
    }

    private void ShiftRowsDown(int startY)
    {
        for(int i = startY; i < m_height; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows()
    {
        m_rowsCompleted = 0;
        for(int y = 0; y < m_height; ++y)
        {
            if(IsComplete(y))
            {
                ClearRow(y);
                ++m_rowsCompleted;                
                ShiftRowsDown(y+1);
                y--;
            }
        }
        OnAllRowsCleared?.Invoke(m_rowsCompleted);
    }


}
