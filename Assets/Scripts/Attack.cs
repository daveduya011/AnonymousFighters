using UnityEngine;

[System.Serializable]
public class Attack
{
    [Header("Attack Data")]
    public AnimationClip animationName;

    public bool isLauncherAttack;

    [Header("Optional")]
    public Vector2 attackStep = Vector2.zero;
    public Vector2 knockForce = Vector2.right * 0.5f;
    public float damageInterval = 0;
    public float startOffset = 0f;
    public float lengthInSeconds = 0f;
    public float stunTime = 0.5f;

    [Header("Optional")]
    public AudioClip soundClip;
    public float soundInterval;

    public SkillEvent onCall;

    public bool IsSkill(Attack attack) {
        return attack is Skill;
    }

}