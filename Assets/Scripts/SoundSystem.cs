using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundSystem : MonoBehaviour
{
    protected AudioSource audioSource;

    protected static SoundSystem _instance;
    public static SoundSystem Instance { get { return _instance; } }

    // Start is called before the first frame update
    public virtual void Awake()
    {
        if (_instance == null) {
            _instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
            OnStart();
        }
        else {
            audioSource = _instance.gameObject.GetComponent<AudioSource>();
        }
    }

    public virtual void OnStart() { }

    public void PlaySound(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip, 1f);
    }
    public void PlaySound(AudioClip audioClip, float volume) {
        audioSource.PlayOneShot(audioClip, volume);
    }


    public void SetSoundVolume(float value) {
        audioSource.volume = value;
    }
    
    public float GetCurrentVolume() {
        return audioSource.volume;
    }

}
