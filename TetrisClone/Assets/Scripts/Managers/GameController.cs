using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
   private Board m_board;
   private Spawner m_spawner;
   private Shape m_activeShape;
   private float m_timeToDrop;
   private float m_dropInterval = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        if(m_spawner != null)
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
            if(m_activeShape == null)
            {
                 m_activeShape = m_spawner.SpawnShape();
            }
        }
        
    }

    void PlayerInput()
    {
        if(Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            if(!m_board.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
            
        }
        else if(Input.GetButtonDown("MoveLeft"))
        {
             m_activeShape.MoveLeft();
            if(!m_board.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }
        else if(Input.GetButtonDown("Rotate"))
        {
            m_activeShape.RotateRight();
            if(!m_board.IsValidPosition(m_activeShape))
                m_activeShape.RotateLeft();

        }
        else if(Input.GetButtonDown("MoveDown"))
        {
            m_dropInterval /= 4;               
        }
        else if(Input.GetButtonUp("MoveDown"))
        {
            m_dropInterval *= 4;
        }

        if(Time.time > m_timeToDrop)
        {
            if(m_activeShape != null)
            {
                m_activeShape.MoveDown();
                if(!m_board.IsValidPosition(m_activeShape))
                {
                     m_activeShape.MoveUp();
                     m_board.StoreShapeInGrid(m_activeShape);
                     m_activeShape = m_spawner.SpawnShape(); 
                }
                

              
            }

            m_timeToDrop = Time.time + m_dropInterval;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_board == null || m_spawner == null)
        {
            return;
        }

        PlayerInput();

        
    }
}
