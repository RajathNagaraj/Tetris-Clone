using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;
    Transform[,] m_grid;

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

    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x <= m_width && y>=0);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            if(!IsWithinBoard((int) pos.x, (int) pos.y))
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
}
