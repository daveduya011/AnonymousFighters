using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill : Attack
{
    [Header("Skill Data")]
    public float damage = 10;
    public float waitTime;
    public bool walkWhileAttacking;
    public Sprite icon;

    [HideInInspector]
    public bool isOn;
    [HideInInspector]
    public float currentTime = 0;
}
