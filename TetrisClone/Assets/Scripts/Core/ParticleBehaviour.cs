using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DestroyGeneratedParticle(1f)); 
    }

     private IEnumerator DestroyGeneratedParticle(float time)
    {
        yield return new WaitForSeconds(time);        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
