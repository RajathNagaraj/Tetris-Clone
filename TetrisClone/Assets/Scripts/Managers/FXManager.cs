using System;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public Transform glowingSquare;
    public Transform rowGlow;
    private ParticlePlayer rowGlowPlayer;

    void OnEnable()
    {
        GameEvents.OnRowGlow += Glow;
        GameEvents.OnLandShapeGlow += LandShapeGlow;
    }

    private void LandShapeGlow(Transform shapeTransform)
    {
        foreach (Transform roundedBlock in shapeTransform)
        {
            Vector3 pos = roundedBlock.position;
            var glowingSquareParticle = Instantiate(glowingSquare, pos, Quaternion.identity);
            glowingSquareParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Glow(int y)
    {
        Vector3 rowPos = new Vector3(0 - 0.5f, y - 0.5f, 0);
        var glowEffect = Instantiate(rowGlow, rowPos, Quaternion.identity);
        rowGlowPlayer = glowEffect.GetComponent<ParticlePlayer>();
        rowGlowPlayer.Play();
    }
}
