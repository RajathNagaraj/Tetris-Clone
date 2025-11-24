using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DestroyGeneratedParticle(2f));
    }

    private IEnumerator DestroyGeneratedParticle(float time)
    {
        var parent = transform.parent;
        yield return new WaitForSeconds(time);
        if (parent != null)
            Destroy(parent.gameObject);
        else
            Destroy(gameObject);
    }


}
