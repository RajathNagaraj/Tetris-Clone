using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
   private Board m_board;
   private Spawner m_spawner;
    // Start is called before the first frame update
    void Start()
    {
        m_board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();

        m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
