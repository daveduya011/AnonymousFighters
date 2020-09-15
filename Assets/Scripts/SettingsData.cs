using UnityEngine.UI;

[System.Serializable]
public class SettingsData
{
    public float sfxVol = 1;
    public float bgMusicVol = 0.3f;
    public int graphics = 1;
    public int difficulty = 1;
    public float[,] controllerPos;

    public ControllerSetup.JoystickType joystickType = ControllerSetup.JoystickType.DYNAMIC;
    public int autoHide = 1;

    public SettingsData(Settings settings) {
        sfxVol = settings.sfxVol;
        bgMusicVol = settings.bgMusicVol;
        graphics = settings.graphics;
    }
    public SettingsData() {

    }
    public SettingsData(SettingsData settings) {
        sfxVol = settings.sfxVol;
        bgMusicVol = settings.bgMusicVol;
        graphics = settings.graphics;
        controllerPos = settings.controllerPos;
        joystickType = settings.joystickType;
        autoHide = settings.autoHide;
        difficulty = settings.difficulty;
    }
}
