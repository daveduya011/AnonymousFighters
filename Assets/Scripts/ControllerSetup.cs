using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerSetup : MonoBehaviour
{
    public enum JoystickType {DYNAMIC, FIXED}

    public JoystickType joystickType = JoystickType.DYNAMIC;
    public int autoHide = 0;

    public float[,] controllerPos;
    public MovableUIObject[] movableObjects;
    private SettingsData settingsData;
    public ToggleGroup joystickTypeGroup;
    public ToggleGroup autoHideGroup;

    private Toggle[] joystickTypeToggles;
    private Toggle[] autoHideToggles;
    // Start is called before the first frame update
    void Start()
    {
        movableObjects = GetComponentsInChildren<MovableUIObject>();
        joystickTypeToggles = joystickTypeGroup.GetComponentsInChildren<Toggle>();
        autoHideToggles = autoHideGroup.GetComponentsInChildren<Toggle>();
        controllerPos = new float[movableObjects.Length,3];


        LoadPreviousSettings();
    }
    private void LoadPreviousSettings() {
        settingsData = SaveSystem.LoadSettings();

        float[,] tempControllerPos = settingsData.controllerPos;
        if (tempControllerPos != null) {
            for (int i = 0; i < tempControllerPos.GetLength(0); i++) {
                Vector3 pos = Vector3.zero;

                pos.x = tempControllerPos[i, 0];
                pos.y = tempControllerPos[i, 1];
                pos.z = tempControllerPos[i, 2];

                movableObjects[i].transform.position = pos;
            }
        }

        joystickTypeToggles[(int)settingsData.joystickType].isOn = true;
        autoHideToggles[settingsData.autoHide].isOn = true;
    }
    public void SaveSettings() {

        for (int i = 0; i < controllerPos.GetLength(0); i++) {
            controllerPos[i, 0] = movableObjects[i].transform.position.x;
            controllerPos[i, 1] = movableObjects[i].transform.position.y;
            controllerPos[i, 2] = movableObjects[i].transform.position.z;
        }
        

        for (int i = 0; i < joystickTypeToggles.Length; i++) {
            if (joystickTypeToggles[i].isOn) {
                joystickType = (JoystickType)i;
                break;
            }
        }

        for (int i = 0; i < autoHideToggles.Length; i++) {
            if (autoHideToggles[i].isOn) {
                autoHide = i;
                break;
            }
        }

        settingsData.controllerPos = controllerPos;
        settingsData.joystickType = joystickType;
        settingsData.autoHide = autoHide;

        SaveSystem.SaveSettings(settingsData);
        SceneManager.LoadScene(0);
    }

    public void ResetSettings() {
        for (int i = 0; i < movableObjects.Length; i++) {
            movableObjects[i].transform.localPosition = movableObjects[i].initialPos;
        }
        autoHideToggles[1].isOn = true;
        joystickTypeToggles[0].isOn = true;
    }

    public void Cancel() {
        SceneManager.LoadScene(0);
    }
}
