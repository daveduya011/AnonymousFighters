using UnityEngine;

public class KnightSpearSkill2 : SpearAttack
{
    public float bulletInterval;
    public int numOfBullets = 3;

    public override void ExecuteSkillEffect(Transform t, Mob mob) {
        this.t = t;
        this.mob = mob;

        for (int i = 0; i < numOfBullets; i++) {
            SpawnedSkill skill = Instantiate(this, t.position, transform.rotation);
            skill.gameObject.SetActive(false);
            skill.Invoke("OnSkillStart", delay + (bulletInterval * i));
        }
    }
}

