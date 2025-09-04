using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool m_musicEnabled = true;
    public bool m_fxEnabled = true;

    [Range(0f, 1f)]
    public float m_musicVolume;
    [Range(0f, 1f)]
    public float m_fxVolume;

    public AudioClip m_rowClearSound;
    public AudioClip m_holdSound;
    public AudioClip m_moveSound;
    public AudioClip m_errorSound;
    public AudioClip m_dropSound;
    public AudioClip m_gameOverSound;
    public AudioSource m_musicSource;
    public AudioClip[] m_musicClips;
    public AudioClip[] m_vocalClips;
    public AudioClip m_gameOverVocalClip;
    public AudioClip m_levelUpVocalClip;
    public IconToggle m_musicIconToggle;
    public IconToggle m_fxIconToggle;

    AudioClip m_randomMusicClip;

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!m_musicEnabled || musicClip == null || m_musicSource == null)
        {
            return;
        }

        m_musicSource.Stop();

        m_musicSource.clip = musicClip;

        m_musicSource.volume = m_musicVolume;

        m_musicSource.loop = true;

        m_musicSource.Play();

    }
    // Start is called before the first frame update
    void Start()
    {
        m_randomMusicClip = GetRandomClip(m_musicClips);
        PlayBackgroundMusic(m_randomMusicClip);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMusic()
    {
        m_musicEnabled = !m_musicEnabled;

        if (m_musicIconToggle != null)
        {
            m_musicIconToggle.ToggleIcon(m_musicEnabled);
        }

        UpdateMusic();
    }

    void UpdateMusic()
    {
        if (m_musicSource.isPlaying != m_musicEnabled)
        {
            if (m_musicEnabled)
            {
                m_randomMusicClip = GetRandomClip(m_musicClips);
                PlayBackgroundMusic(m_randomMusicClip);
            }
            else
            {
                m_musicSource.Stop();
            }
        }
    }

    public void ToggleFX()
    {
        m_fxEnabled = !m_fxEnabled;

        if (m_fxIconToggle != null)
        {
            m_fxIconToggle.ToggleIcon(m_fxEnabled);
        }
    }
}
