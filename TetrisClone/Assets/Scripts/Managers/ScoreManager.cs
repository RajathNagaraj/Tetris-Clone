using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int m_lines;
    private int m_score = 0;
    private int m_level = 1;
    public int m_linesPerLevel = 5;
    private const int m_MINLINES = 1;
    private const int m_MAXLINES = 4;

    public Action<int> OnLevelUp = delegate { };

    public Action OnLevelUpNotifyUI = delegate { };

    public Action<int, int, int> OnUpdateUI = delegate { };

    public void ScoreLines(int numberOfLines)
    {
        numberOfLines = Mathf.Clamp(numberOfLines, m_MINLINES, m_MAXLINES);

        switch (numberOfLines)
        {
            case 1:
                m_score += 40 * m_level;
                break;
            case 2:
                m_score += 100 * m_level;
                break;
            case 3:
                m_score += 300 * m_level;
                break;
            case 4:
                m_score += 1200 * m_level;
                break;
        }
        m_lines -= numberOfLines;

        if (m_lines <= 0)
            LevelUp();

        OnUpdateUI?.Invoke(m_lines, m_level, m_score);
    }

    public void Reset()
    {
        m_level = 1;
        m_lines = m_linesPerLevel * m_level;
        OnUpdateUI?.Invoke(m_lines, m_level, m_score);
    }


    private void LevelUp()
    {
        m_level++;
        m_lines = m_linesPerLevel * m_level;
        OnLevelUp?.Invoke(m_level);
        OnLevelUpNotifyUI?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
