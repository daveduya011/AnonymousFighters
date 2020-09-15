using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider soundFXSlider;
    public Slider bgMusicSlider;
    public ToggleGroup graphicsToggleGroup;


    [HideInInspector]
    public Toggle[] toggles;

    public float sfxVol;
    public float bgMusicVol;
    public int graphics;
    private float initBgMusicVol;
    private float initSfxVol;
    public void Start() {
        toggles = graphicsToggleGroup.GetComponentsInChildren<Toggle>();
        initBgMusicVol = BGSoundSystem.Instance.GetCurrentVolume();
        initSfxVol = FXSoundSystem.Instance.GetCurrentVolume();
        LoadPreviousSettings();
    }

    private void LoadPreviousSettings() {
        SettingsData data = SaveSystem.LoadSettings();

        sfxVol = data.sfxVol;
        bgMusicVol = data.bgMusicVol;
        graphics = data.graphics;

        soundFXSlider.value = sfxVol;
        bgMusicSlider.value = bgMusicVol;

        graphicsToggleGroup.SetAllTogglesOff();
        toggles[graphics].isOn = true;
    }

    public void SaveSettings() {
        int tempGraphics = -1;
        for (int i = 0; i < toggles.Length; i++) {
            if (toggles[i].isOn) {
                tempGraphics = i;
                break;
            }
        }
        initBgMusicVol = BGSoundSystem.Instance.GetCurrentVolume();
        initSfxVol = FXSoundSystem.Instance.GetCurrentVolume();

        graphics = tempGraphics;
        sfxVol = soundFXSlider.value;
        bgMusicVol = bgMusicSlider.value;

        GameManager.Instance.ChangeGraphics(graphics);
        SaveSystem.SaveSettings(this);
        gameObject.SetActive(false);
    }

    public void ShowSettings() {
        gameObject.SetActive(true);
    }

    public void HideSettings() {
        LoadPreviousSettings();
        BGSoundSystem.Instance.SetSoundVolume(initBgMusicVol);
        FXSoundSystem.Instance.SetSoundVolume(initSfxVol);
        gameObject.SetActive(false);
    }

    public void LoadControllerSettings() {
        BGSoundSystem.Instance.SetSoundVolume(initBgMusicVol);
        FXSoundSystem.Instance.SetSoundVolume(initSfxVol);
        SceneManager.LoadScene("Control Configuration");
    }

    public void OnBGVolumeChange() {
        BGSoundSystem.Instance.SetSoundVolume(bgMusicSlider.value);
    }
    public void OnSFXVolChange() {
        FXSoundSystem.Instance.SetSoundVolume(soundFXSlider.value);
    }
}
