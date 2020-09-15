using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack_", menuName = "Mob/ComboAttack", order = 1)]
public class ComboAttack : ScriptableObject
{
    public Sprite icon;
    public Attack[] attacks;
}
