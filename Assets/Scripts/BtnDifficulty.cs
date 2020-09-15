using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnDifficulty : MonoBehaviour
{
    private Text text;
    private int currentIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        SettingsData data = SaveSystem.LoadSettings();
        currentIndex = data.difficulty;
        UpdateDifficulty(currentIndex);
    }

    private void UpdateDifficulty(int index) {
        switch (index) {
            case 1:
                text.text = "EASY";
                break;
            case 2:
                text.text = "NORMAL";
                break;
            case 3:
                text.text = "HARD";
                break;
        }
        GameManager.Instance.difficulty = index;
    }

    public void ChangeDifficulty() {
        if (currentIndex < 3)
            currentIndex++;
        else currentIndex = 1;

        UpdateDifficulty(currentIndex);

        SettingsData data = SaveSystem.LoadSettings();
        data.difficulty = currentIndex;
        SaveSystem.SaveSettings(data);

    }
}
