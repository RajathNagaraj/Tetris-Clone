using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text m_linesText;
    [SerializeField]
    private Text m_levelText;
    [SerializeField]
    private Text m_scoreText;

    [SerializeField]
    private GameObject LevelUpText;


    public void UpdateUI(int lines, int level, int score)
    {
        if(m_linesText != null)
        {
            m_linesText.text = lines.ToString();
        }

        if(m_levelText != null)
        {
            m_levelText.text = level.ToString();
        }

        if(m_scoreText != null)
        {
            m_scoreText.text = padZero(score,5);
        }
    }

    public void LevelUp()
    {
        LevelUpText.SetActive(true);
        StartCoroutine(DisableLevelUpText(3f));
    }

    private IEnumerator DisableLevelUpText(float timeToDisable)
    {
        yield return new WaitForSeconds(timeToDisable);
        LevelUpText.SetActive(false);     
    }

    private string padZero(int score, int padDigits)
    {
        string scoreString = score.ToString();

        while(scoreString.Length < padDigits)
        {
            scoreString = "0" + scoreString;
        }

        return scoreString;
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
