using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public delegate void DisableSounds();
    
    public static event DisableSounds DisableSoundsEvent;

    public bool musicEnabled = true;
    public bool soundsEnabled = true;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundsSource;

    [SerializeField] private AudioClip buttonClickSound;

    [SerializeField] private AudioClip linePopSoundClip;
    [SerializeField] private AudioClip TNTSoundClip;

    public delegate void ButtonSoundDelegate();
    public static event ButtonSoundDelegate OnButtonClicked;
    public static event ButtonSoundDelegate OnLinePopped;
    
    public static event ButtonSoundDelegate OnTNTPlaced;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        OnButtonClicked += PlayButtonSound;
        OnLinePopped += PlayLinePoppedSound;
        OnTNTPlaced += () =>
        {
            if (!soundsEnabled) return;

            soundsSource.clip = TNTSoundClip;
            soundsSource.PlayOneShot(TNTSoundClip);
        };
    }

    private void Start()
    {
        
    }

    private void PlayButtonSound()
    {
        if (!soundsEnabled) return;
        
        soundsSource.clip = buttonClickSound;
        soundsSource.PlayOneShot(buttonClickSound);

    }

    private void PlayLinePoppedSound()
    {
        if (!soundsEnabled) return;
        
        soundsSource.clip = linePopSoundClip;
        soundsSource.PlayOneShot(linePopSoundClip);
    }
    
    public static void ButtonPressed()
    {
        OnButtonClicked?.Invoke();
    }
    
    public static void LinePopped()
    {
        OnLinePopped?.Invoke();
    }
    
    public static void TNTPlaced()
    {
        OnTNTPlaced?.Invoke();
    }
}
