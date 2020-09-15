using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName = "Mob/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public Skill[] skills;
}
