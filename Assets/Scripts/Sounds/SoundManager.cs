using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public delegate void DisableSounds();
    
    public static event DisableSounds DisableSoundsEvent;

    public bool musicEnabled = true;
    public bool soundsEnabled = true;

    private AudioSource _soundsSource = new AudioSource();
    private AudioSource _musicSource = new AudioSource();


    private void Start()
    {
//        _soundsSource.PlayOneShot();
        
    }

    private void ChangeSetting(bool status, AudioSource audioSource)
    {
//        audioSource.gameObject.SetActive(false);
        if (status)
        {
            // turn off
        }
        else
        {
            // turn on
        }
    }
    
}
