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

    // Update is called once per frame
    void Update()
    {
        if(m_board == null || m_spawner == null)
        {
            return;
        }

        if(Time.time > m_timeToDrop)
        {
            if(m_activeShape != null)
            {
                m_activeShape.MoveDown();
                if(!m_board.IsValidPosition(m_activeShape))
                {
                     m_activeShape.MoveUp();
                     m_activeShape = m_spawner.SpawnShape(); 
                }
                

              
            }

            m_timeToDrop = Time.time + m_dropInterval;
        }
    }
}
