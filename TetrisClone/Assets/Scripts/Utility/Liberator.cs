using UnityEngine;

public static class Liberator
{
    //We want to dereference the child blocks from the parent so that they dont get deleted when the parent
    //is destroyed
    public static void FreeChildrenInCurrentShape(ref Shape shape)
    {
        var transforms = shape.transform.GetComponentsInChildren<Transform>();
        if (transforms != null)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].parent = null;
            }
        }
    }
}
