using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
   private Board m_board;
   private Spawner m_spawner;
   private Shape m_activeShape;
   private float m_timeToDrop;
   private float m_dropInterval = 0.25f;
   private float m_timeToNextKey;
   private float m_keyRepeatRate = 0.15f;
    private bool m_gameOver = false;
    public GameObject m_gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        m_gameOverPanel.SetActive(false);
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
        if(m_board == null || m_spawner == null || m_gameOver)
        {
            return;
        }

        PlayerInput();

        
    }

    public void Restart()
    {
        Debug.Log("Restarted Level");
        SceneManager.LoadScene("Gameplay");
    }

     void PlayerInput()
    {
        if((Input.GetButton("MoveRight") && Time.time > m_timeToNextKey) || Input.GetButtonDown("MoveRight"))
        {
            m_timeToNextKey = m_keyRepeatRate + Time.time;
            m_activeShape.MoveRight();
            if(!m_board.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
            
        }
        else if((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKey) || Input.GetButtonDown("MoveLeft"))
        {
            m_timeToNextKey = m_keyRepeatRate + Time.time;
             m_activeShape.MoveLeft();
            if(!m_board.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }
        else if(Input.GetButtonDown("Rotate") )
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
                   if(m_board.IsOverLimit(m_activeShape))
                   {
                      GameOver();
                   }
                   else
                   {
                      LandShape(); 
                   }
                   
                }
                

              
            }

            m_timeToDrop = Time.time + m_dropInterval;
        }
    }

    private void GameOver()
    {
         m_activeShape.MoveUp();
         m_gameOver = true;
         m_gameOverPanel.SetActive(true);
         Debug.Log("Game Over");
    }

    private void LandShape()
    {
        m_activeShape.MoveUp();
        m_board.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape(); 

        m_board.ClearAllRows();
    }
}
