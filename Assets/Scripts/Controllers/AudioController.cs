using UnityEngine.Audio;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    public static AudioClip correctMatch, incorrectMatch, backgroundSound;
    static AudioSource audioSource;

    void Awake()
    {
        audioSource = GameController.Instance.GetComponent<AudioSource>();
        correctMatch = Resources.Load<AudioClip>("correct");
        incorrectMatch = Resources.Load<AudioClip>("incorrect");
        backgroundSound = Resources.Load<AudioClip>("background");
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "correct":
                audioSource.PlayOneShot(correctMatch);
                break;
            case "incorrect":
                audioSource.PlayOneShot(incorrectMatch);
                break;
        }

    }

    public static void StopSound()
    {
        audioSource.Stop();
    }

    public static void StartBackgroundSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(backgroundSound);
    }
    public static bool SetSound()
    {
        bool isMuted = !audioSource.mute;
        audioSource.mute = isMuted;
        return isMuted;
    }
    public static bool SoundEnabled()
    {
        return audioSource.mute;
    }
}
