using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{

    public bool m_canRotate = true;

    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

   public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

   public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

   public void RotateLeft()
    {
        if(m_canRotate)
             transform.Rotate(0, 0, 90);
    }

    public void RotateRight()
    {
        if(m_canRotate)
             transform.Rotate(0, 0, -90);
    }

    public void RotateClockwise(bool clockwise)
    {
        if(clockwise)
        {
            RotateRight();
        }
        else
        {
            RotateLeft();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    private void OnDestroy() 
    {
        
        var children = GetComponentsInChildren<Transform>();
        for(int i = 0; i < children.Length; i++)
        {
            children[i].parent = null;
        }

        
       
    }
    */
    
}
