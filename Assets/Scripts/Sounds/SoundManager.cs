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

    public delegate void ButtonSoundDelegate();

    public static event ButtonSoundDelegate OnButtonClicked;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        OnButtonClicked += PlayButtonSound;
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

    public static void ButtonPressed()
    {
        OnButtonClicked?.Invoke();
    }
    

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           
        }
    }
}
