using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips de sonido")]
    public AudioClip countdownClip;
    public AudioClip correctAnswerClip;
    public AudioClip wrongAnswerClip;
    public AudioClip applause1Clip;
    public AudioClip applause2Clip;
    public AudioClip applause3Clip;
    public AudioClip ohNoClip;
    public AudioClip typingClip;

    private AudioSource typingSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // fuente exclusiva para el sonido de tecleo
            typingSource = gameObject.AddComponent<AudioSource>();
            typingSource.loop = true;
            typingSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Música
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Sonidos
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayLoopingSFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopLoopingSFX()
    {
        sfxSource.Stop();
        sfxSource.loop = false;
    }

    public void StartTypingSFX()
    {
        if (typingClip != null && !typingSource.isPlaying)
        {
            typingSource.clip = typingClip;
            typingSource.Play();
        }
    }

    public void StopTypingSFX()
    {
        if (typingSource.isPlaying)
        {
            typingSource.Stop();
        }
    }


}

