using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundSystem : SoundSystem
{

    public override void Awake() {

        if (_instance == null) {
            _instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
            OnStart();
        } else {
            audioSource = _instance.gameObject.GetComponent<AudioSource>();
        }

    }
    public override void OnStart() {
        base.OnStart();
        audioSource.Play();
        SettingsData settings = SaveSystem.LoadSettings();
        SetSoundVolume(settings.bgMusicVol);
    }
}
