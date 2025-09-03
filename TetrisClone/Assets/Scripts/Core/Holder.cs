using System;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public Transform m_holderXForm;
    public Shape m_heldShape = null;
    private float m_scaleFactor = 0.5f;
    private bool canHold = true;

    public void Catch(Shape shape)
    {
        if (m_heldShape != null)
        {
            Debug.LogWarning("Release a shape before trying to catch one!");
        }
        if (shape == null)
        {
            Debug.LogWarning("Invalid Shape!!");
        }
        if (m_holderXForm != null)
        {
            m_heldShape = shape;
            m_heldShape = Instantiate(m_heldShape, m_holderXForm.position + shape.m_queueOffset, Quaternion.identity);
            m_heldShape.transform.localScale = new Vector3(m_scaleFactor, m_scaleFactor, m_scaleFactor);
        }
        else
        {
            Debug.LogWarning("Please assign holder transform!!");
        }
    }

    public Shape Release()
    {
        Shape shape = null;
        shape = m_heldShape;
        shape = Instantiate(shape, transform.position, Quaternion.identity);
        shape.transform.localScale = Vector3.one;
        Destroy(m_heldShape.gameObject);
        m_heldShape = null;
        return shape;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CanHold
    {
        get { return canHold; }
        set { canHold = value; }
    }
}
