using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private Board m_board;
    private Spawner m_spawner;
    private Shape m_activeShape;
    private Shape m_ghostShape = null;
    public bool m_ghostHitBottom = false;
    public Color m_ghostColor = new Color(1f, 1f, 1f, 0.2f);
    private float m_timeToDrop;
    private float m_dropInterval = 0.25f;
    private float m_dropIntervalModded;
    private float m_timeToNextKey;
    private float m_keyRepeatRate = 0.15f;
    private bool m_gameOver = false;
    public GameObject m_gameOverPanel;
    private SoundManager m_soundManager;
    private ScoreManager m_scoreManager;
    private UIManager m_uiManager;
    private Holder m_holder;
    private Camera mainCamera;
    private InputSetup m_inputSetup;
    private TouchGestures m_touchGestures;
    public IconToggle m_rotIconToggle;
    private bool m_clockwise = true;
    public bool m_isPaused = false;
    public GameObject m_pausePanel;
    public PlayerControls m_playerControls;
    void Awake()
    {
        m_playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Disabling the GameOver and Pause panels, as we dont want them appearing 
        //when the game starts
        m_gameOverPanel.SetActive(false);
        m_pausePanel.SetActive(false);

        //Finding and caching references to all important components of the Game
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        m_spawner = FindFirstObjectByType<Spawner>();
        m_soundManager = FindFirstObjectByType<SoundManager>();
        m_scoreManager = FindFirstObjectByType<ScoreManager>();
        m_uiManager = FindFirstObjectByType<UIManager>();
        m_holder = FindFirstObjectByType<Holder>();
        m_inputSetup = FindFirstObjectByType<InputSetup>();
        m_touchGestures = FindFirstObjectByType<TouchGestures>();

        m_inputSetup.SetupControls(m_playerControls);
        m_touchGestures.SetupTouch(m_playerControls);

        mainCamera = Camera.main;

        //A constant drop interval makes the game boring.
        //So, we modify the drop interval when the Player completes a Line.
        m_dropIntervalModded = m_dropInterval;

        //If the Spawner exists
        if (m_spawner != null)
        {
            //We round off the position of the Spawner if it is in Decimals
            //And store it back in the Spawner position variable
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
            //If there isnt an active Shape present in the scene
            if (m_activeShape == null)
            {
                //Spawn one and store it in a field variable already present
                m_activeShape = m_spawner.GetQueuedShape();
            }
        }



    }

    private void OnEnable()
    {
        //OnRowCompleted is an action present in the Board class that subscribes to
        //a lambda in this class, which in turn plays a sound upon clearing a row.
        GameEvents.OnRowCompleted += () =>
        {
            PlaySound(m_soundManager.m_rowClearSound);
        };

        //This action gets triggered when all rows in the board are checked for clearing.
        //It passes the number of rows actually cleared to a method called UpdateScore.
        GameEvents.OnAllRowsCleared += UpdateScore;

        //Everytime Player levels up the drop interval shortens making
        //shapes fall faster.
        GameEvents.OnLevelUp += (int level) =>
        {
            Invoke("PlayLevelUpVocalClip", 1f);

            //We perform a calculation and store the value in the modded drop interval variable.
            //It is clamped between 0.05 and 1.
            m_dropIntervalModded = Mathf.Clamp(m_dropInterval - ((level - 1) * 0.05f), 0.05f, 1f);
        };

        m_playerControls.Gameplay.Enable();

        SubscribeToInputEvents();

    }

    private void SubscribeToInputEvents()
    {
        GameEvents.OnMoveShapeLeft += MoveLeft;
        GameEvents.OnMoveShapeRight += MoveRight;
        GameEvents.OnRotateShape += RotateShape;
        GameEvents.OnShapeFallFaster += FallFaster;
    }

    private void UnsubscribeFromInputEvents()
    {
        GameEvents.OnMoveShapeLeft -= MoveLeft;
        GameEvents.OnMoveShapeRight -= MoveRight;
        GameEvents.OnRotateShape -= RotateShape;
        GameEvents.OnShapeFallFaster -= FallFaster;
    }

    private void FallFaster(bool m_isDroppingFast)
    {
        if (m_isDroppingFast)
            m_dropIntervalModded /= 4;
        else
            m_dropIntervalModded *= 4;
    }

    private void RotateShape()
    {
        //If the button corresponding to Rotate has been detected within the alloted time frame?
        if (Time.time > m_timeToNextKey)
        {
            ProcessInput(() => m_activeShape.RotateClockwise(m_clockwise), () => m_activeShape.RotateClockwise(!m_clockwise));
        }
    }

    private void MoveRight()
    {
        //If the button corresponding to MoveRight has been detected within the alloted time frame?
        if (Time.time > m_timeToNextKey)
        {
            ProcessInput(() => m_activeShape.MoveRight(), () => m_activeShape.MoveLeft());
        }
    }

    private void MoveLeft()
    {
        //If the button corresponding to MoveLeft has been detected within the alloted time frame?
        if (Time.time > m_timeToNextKey)
        {
            ProcessInput(() => m_activeShape.MoveLeft(), () => m_activeShape.MoveRight());
        }
    }

    private void OnDisable()
    {
        GameEvents.OnAllRowsCleared -= UpdateScore;

        m_playerControls.Gameplay.Disable();

        GameEvents.OnRowCompleted = null;

        GameEvents.OnLevelUp = null;

        UnsubscribeFromInputEvents();
    }

    //The Level Up Vocal Clip is invoked after 1 second as we do not want many vocal clips playing at the same time
    private void PlayLevelUpVocalClip()
    {
        PlaySound(m_soundManager.m_levelUpVocalClip);
    }


    //The Board class notifies us when all rows are cleared. Thats when the score 
    //has to be updated in case any rows are completed.
    private void UpdateScore(int rowsCompleted)
    {
        m_scoreManager.ScoreLines(rowsCompleted);
    }



    // Update is called once per frame
    void Update()
    {
        //Precautionary check to ensure that all critical components and variables are present.
        //Return if any are missing. Dont do anything.
        if (m_board == null || m_activeShape == null || m_spawner == null || m_soundManager == null || m_scoreManager == null || m_gameOver)
        {
            return;
        }
        //Lower the Shape at constant intervals.
        DropShape();
    }

    void LateUpdate()
    {
        //Draw the Ghost shape ONLY AFTER the real shape has been drawn.
        DrawGhost();

    }
    //Restart Logic
    //Freeze time and Reload Scene.
    public void Restart()
    {
        Debug.Log("Restarted Level");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }
    //Drop Shape Logic
    void DropShape()
    {

        if (Time.time > m_timeToDrop)
        {
            if (m_activeShape != null)
            {
                m_activeShape.MoveDown();
                if (!m_board.IsValidPosition(m_activeShape))
                {

                    if (m_board.IsOverLimit(m_activeShape))
                    {
                        GameOver();
                    }
                    else
                    {
                        LandShape();
                    }
                }



            }

            m_timeToDrop = Time.time + m_dropIntervalModded;
        }
    }

    private void ProcessInput(Action action, Action revertedAction)
    {
        m_timeToNextKey = m_keyRepeatRate + Time.time;
        action();
        if (!m_board.IsValidPosition(m_activeShape))
        {
            PlaySound(m_soundManager.m_errorSound);
            revertedAction();
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound);
        }
    }

    private void DrawGhost()
    {
        if (m_ghostShape == null)
        {
            m_ghostShape = Instantiate(m_activeShape, m_activeShape.transform.position, m_activeShape.transform.rotation) as Shape;
            m_ghostShape.gameObject.name = "GhostShape";

            SpriteRenderer[] allRenderers = m_ghostShape.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < allRenderers.Length; ++i)
            {
                allRenderers[i].color = m_ghostColor;
            }
        }
        else
        {
            m_ghostShape.transform.position = m_activeShape.transform.position;
            m_ghostShape.transform.rotation = m_activeShape.transform.rotation;
            m_ghostShape.transform.localScale = m_activeShape.transform.localScale;
        }

        m_ghostHitBottom = false;

        while (!m_ghostHitBottom)
        {
            m_ghostShape.MoveDown();
            if (!m_board.IsValidPosition(m_ghostShape))
            {
                m_ghostShape.MoveUp();
                m_ghostHitBottom = true;
            }
        }
    }

    private void GameOver()
    {
        //m_activeShape.MoveUp();
        m_gameOver = true;
        GameEvents.OnGameOver?.Invoke();
        GameEvents.OnCrossLineOfDeath?.Invoke();
        StartCoroutine(DelayGameOverPanel());
        PlaySound(m_soundManager.m_gameOverSound);
        PlaySound(m_soundManager.m_gameOverVocalClip);
        Debug.Log("Game Over");
    }

    private IEnumerator DelayGameOverPanel()
    {
        yield return new WaitForSeconds(1f);
        m_gameOverPanel.SetActive(true);
    }

    private void LandShape()
    {
        m_activeShape.MoveUp();
        m_holder.CanHold = true;

        //The if check below was for a particular case when the shape would land and part of it 
        //would still be above the board and the game would continue.
        if (m_board.IsOverLimit(m_activeShape))
        {
            GameOver();
        }

        GameEvents.OnLandShapeGlow?.Invoke(m_activeShape.transform);
        m_board.StoreShapeInGrid(m_activeShape);

        DestroyActiveShape();


        m_activeShape = m_spawner.GetQueuedShape();

        m_board.ClearAllRows();

        if (m_board.m_rowsCompleted > 1)
        {
            AudioClip randomVocalClip = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
            PlaySound(randomVocalClip);
        }

        PlaySound(m_soundManager.m_dropSound);
    }

    private void DestroyActiveShape()
    {
        Destroy(m_activeShape.gameObject);
        DestroyGhost();
        m_activeShape = null;
    }
    private void DestroyGhost()
    {
        Destroy(m_ghostShape.gameObject);
        m_ghostShape = null;
    }

    void PlaySound(AudioClip sound)
    {
        if (sound != null && m_soundManager.m_fxEnabled)
        {
            AudioSource.PlayClipAtPoint(sound, mainCamera.transform.position, m_soundManager.m_fxVolume);
        }
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;

        if (m_rotIconToggle != null)
        {
            m_rotIconToggle.ToggleIcon(m_clockwise);
        }
    }

    public void TogglePause()
    {
        if (m_gameOver)
        {
            return;
        }

        m_isPaused = !m_isPaused;

        if (m_pausePanel != null)
        {
            m_pausePanel.SetActive(m_isPaused);

            m_soundManager.m_musicSource.volume = m_isPaused ? (m_soundManager.m_musicVolume * 0.25f) : m_soundManager.m_musicVolume;

            Time.timeScale = m_isPaused ? 0 : 1;

        }
    }

    public void ProcessHoldButton()
    {

        if (m_holder.m_heldShape == null && m_holder.CanHold)
        {
            PlaySound(m_soundManager.m_holdSound);
            m_holder.Catch(m_activeShape);
            DestroyActiveShape();
            m_activeShape = m_spawner.GetQueuedShape();
        }
        else if (m_holder.m_heldShape != null)
        {
            PlaySound(m_soundManager.m_holdSound);
            DestroyActiveShape();
            m_activeShape = m_holder.Release();
            m_holder.CanHold = false;
            m_activeShape.transform.position = m_spawner.transform.position;
            DrawGhost();
        }
        else
        {
            PlaySound(m_soundManager.m_errorSound);
            return;
        }

    }

}
