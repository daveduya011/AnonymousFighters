using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUICanvas : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthText;
    public Color normalHealthColor;
    public Color lowHealthColor;
    public RectTransform[] movableObjects;
    public VariableJoystick joystick;
    public ReviveInfoView reviveInfoView;

    void Start() {
        SettingsData settingsData = SaveSystem.LoadSettings();

        float[,] tempControllerPos = settingsData.controllerPos;
        if (tempControllerPos != null) {
            for (int i = 0; i < tempControllerPos.GetLength(0); i++) {
                Vector3 pos = Vector3.zero;

                pos.x = tempControllerPos[i, 0];
                pos.y = tempControllerPos[i, 1];
                pos.z = tempControllerPos[i, 2];

                movableObjects[i].transform.position = pos;
            }
            joystick.isAutoHide = settingsData.autoHide == 0;
            if (settingsData.joystickType == ControllerSetup.JoystickType.DYNAMIC)
                joystick.SetMode(JoystickType.Dynamic);
            else
                joystick.SetMode(JoystickType.Fixed);
        }
    }
}
