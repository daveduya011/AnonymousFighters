using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data_", menuName = "Mob/Stats", order = 1)]
public class MobStats : ScriptableObject
{
    [Header("Health")]
    public float HP = 0;

    [Header("Health Regen")]
    public float MP5 = 0;

    [Header("Attack Damage")]
    public float AD = 0;

    [Header("Attack Speed")]
    public float ASPD = 0;

    [Header("Armor: 17 - 47")]
    public float AR = 0;

    [Header("Movement Speed: 4 - 8")]
    public float MSPD = 0;

    [Header("Other Stats")]
    public bool isKnockable = true;
}
