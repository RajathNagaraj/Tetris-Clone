using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;
    public Transform[] m_queuedXForms = new Transform[3];
    private Shape[] m_shapeQueue = new Shape[3];
    private float m_queueScale = 0.5f;

    private Shape GetRandomShape()
    {
        int shapeIndex = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[shapeIndex] != null)
        {
            return m_allShapes[shapeIndex];
        }
        else
        {
            Debug.Log("Warning!!! Shape not Found!!");
            return null;
        }
    }

    private Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);
        return shape;
    }

    private void InitQueue()
    {
        for (int i = 0; i < m_shapeQueue.Length; i++)
        {
            m_shapeQueue[i] = null;
        }
        FillQueue();
    }

    private void FillQueue()
    {
        for (int i = 0; i < m_shapeQueue.Length; i++)
        {
            if (m_shapeQueue[i] == null)
            {
                m_shapeQueue[i] = SpawnShape();
                m_shapeQueue[i].transform.localScale = new Vector3(m_queueScale, m_queueScale, m_queueScale);
                m_shapeQueue[i].transform.position = m_queuedXForms[i].position + m_shapeQueue[i].m_queueOffset;
            }

        }
    }

    public Shape GetQueuedShape()
    {
        Shape queuedShape = null;
        if (m_shapeQueue[0] != null)
        {
            queuedShape = m_shapeQueue[0];
        }
        for (int i = 1; i < m_shapeQueue.Length; i++)
        {
            m_shapeQueue[i - 1] = m_shapeQueue[i];
            m_shapeQueue[i - 1].transform.position = m_queuedXForms[i - 1].position + m_shapeQueue[i - 1].m_queueOffset;
        }
        queuedShape.transform.position = transform.position;
        m_shapeQueue[m_shapeQueue.Length - 1] = null;
        FillQueue();
        queuedShape.transform.localScale = Vector3.one;
        return queuedShape;
    }

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        InitQueue();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
