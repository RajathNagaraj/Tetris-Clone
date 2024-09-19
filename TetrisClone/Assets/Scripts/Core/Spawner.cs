using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;

    private Shape GetRandomShape()
    {
        int shapeIndex = Random.Range(0, m_allShapes.Length);
        if(m_allShapes[shapeIndex] != null)
        {
            return m_allShapes[shapeIndex];
        }
        else
        {
            Debug.Log("Warning!!! Shape not Found!!");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);
        return shape;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
