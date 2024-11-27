using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    public float m_targetAlpha = 0f;
    public float m_startAlpha = 1f;
    public float m_delay = 1f;
    public float m_timeToFade = 1f;
    private float m_inc;
    private float m_currentAlpha;
    private MaskableGraphic m_graphic;
    private Color m_originalColor;


    // Start is called before the first frame update
    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        m_originalColor = m_graphic.color;
        Color tempColor = new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);
        m_inc = ((m_targetAlpha - m_startAlpha)/m_timeToFade) * Time.deltaTime;
        StartCoroutine("FadeRoutine");        
    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(m_delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
