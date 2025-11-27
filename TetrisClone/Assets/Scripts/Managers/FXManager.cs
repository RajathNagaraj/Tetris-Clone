using System;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public Transform glowingSquare;
    public Transform rowGlow;
    public Transform backgroundSquaresFX;
    public Transform spaceDustFX;
    public Transform wormHoleFX;
    private GameObject particleExtrasObject;
    private ParticlePlayer rowGlowPlayer;
    private float duration = 10f;
    private float speed;

    void Awake()
    {
        particleExtrasObject = new GameObject("Particle Extras");
        particleExtrasObject.transform.position = Vector3.zero;
    }

    void OnEnable()
    {
        GameEvents.OnRowGlow += Glow;
        GameEvents.OnLandShapeGlow += LandShapeGlow;
        GameEvents.OnGameOver += StopFX;
    }
    void OnDisable()
    {
        GameEvents.OnRowGlow -= Glow;
        GameEvents.OnLandShapeGlow -= LandShapeGlow;
        GameEvents.OnGameOver -= StopFX;
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
        backgroundSquaresFX = Create(backgroundSquaresFX, new Vector3(3.5f, 12f, 0f));
        spaceDustFX = Create(spaceDustFX, new Vector3(3f, 15f, 0f));
        wormHoleFX = Create(wormHoleFX, new Vector3(4.5f, 25f, 0f));

        speed = 360f / duration;
    }

    private Transform Create(Transform fxTransform, Vector3 position)
    {
        fxTransform = Instantiate(fxTransform, position, Quaternion.identity);
        SetParent(fxTransform);
        return fxTransform;
    }

    private void SetParent(Transform child)
    {
        child.parent = particleExtrasObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundSquaresFX.Rotate(0f, 0f, speed * Time.deltaTime);
    }

    private void StopFX()
    {
        ParticleSystem[] particles = particleExtrasObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            ps.Stop();
        }
    }

    private void Glow(int y)
    {
        Vector3 rowPos = new Vector3(0 - 0.5f, y - 0.5f, 0);
        var glowEffect = Instantiate(rowGlow, rowPos, Quaternion.identity);
        rowGlowPlayer = glowEffect.GetComponent<ParticlePlayer>();
        rowGlowPlayer.Play();
    }
}
