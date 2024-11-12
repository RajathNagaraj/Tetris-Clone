using System;
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
    private SoundManager m_soundManager;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        m_gameOverPanel.SetActive(false);
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        m_soundManager = FindObjectOfType<SoundManager>();
        mainCamera = Camera.main;
        if(m_spawner != null)
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
            if(m_activeShape == null)
            {
                 m_activeShape = m_spawner.SpawnShape();
            }
        }

        m_board.OnRowCompleted += ()=>
        {
            PlaySound(m_soundManager.m_rowClearSound);
        };
        
    }

   

    // Update is called once per frame
    void Update()
    {
        if(m_board == null || m_activeShape == null || m_spawner == null || m_soundManager == null || m_gameOver)
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
            ProcessInput(() => m_activeShape.MoveRight(), () => m_activeShape.MoveLeft());            
        }
        else if((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKey) || Input.GetButtonDown("MoveLeft"))
        {
           ProcessInput(() => m_activeShape.MoveLeft(), () => m_activeShape.MoveRight());
        }
        else if(Input.GetButtonDown("Rotate") )
        {            
           ProcessInput(() => m_activeShape.RotateRight(), () => m_activeShape.RotateLeft());
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

    private void ProcessInput(Action action,Action revertedAction)
    {
            m_timeToNextKey = m_keyRepeatRate + Time.time;
            action();
            if(!m_board.IsValidPosition(m_activeShape))
            {
                PlaySound(m_soundManager.m_errorSound);
                revertedAction();
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound);
            }
    }

    private void GameOver()
    {
         m_activeShape.MoveUp();
         m_gameOver = true;
         m_gameOverPanel.SetActive(true);
         PlaySound(m_soundManager.m_gameOverSound);
         PlaySound(m_soundManager.m_gameOverVocalClip);
         Debug.Log("Game Over");
    }

    private void LandShape()
    {
        m_activeShape.MoveUp();
        m_board.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape(); 

        m_board.ClearAllRows();

        if(m_board.m_rowsCompleted > 1)
        {
            AudioClip randomVocalClip = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
            PlaySound(randomVocalClip);
        }

        PlaySound(m_soundManager.m_dropSound);
    }

    void PlaySound(AudioClip sound)
    {
        if(sound != null && m_soundManager.m_fxEnabled)
        {
            AudioSource.PlayClipAtPoint(sound,mainCamera.transform.position,m_soundManager.m_fxVolume);
        }
    }
    
}
