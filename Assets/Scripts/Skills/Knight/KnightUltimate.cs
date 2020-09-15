using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightUltimate : Triggerable
{
    public float destroyTime;
    public float attackInterval = 0.4f;
    [HideInInspector]
    public Transform t;
    [HideInInspector]
    public Mob mob;

    private float currentTime;
    protected int numOfEnemies;
    protected int attackCount;
    protected List<Collider2D> lastMobs;
    public bool useCharacterTrigger;


    public virtual void ExecuteSkillEffect(Transform t, Mob mob) {
        this.t = t;
        this.mob = mob;

        KnightUltimate skill = Instantiate(this, t.position, transform.rotation);
        skill.currentTime = attackInterval;
        skill.lastMobs = new List<Collider2D>();
        skill.numOfEnemies = GameManager.Instance.numOfEnemies;
        skill.Invoke("Destroy", destroyTime);

        if (!useCharacterTrigger)
            mob.isTriggerEnabled = false;
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        transform.position = mob.transform.position;
        currentTime -= Time.deltaTime;
        if (currentTime > 0) {
        }
    }


    public virtual void Destroy() {
        mob.isTriggerEnabled = true;
        mob.EndAttack();
        gameObject.SetActive(false);
    }

    public override void OnTriggerEntered(Collider2D col) {
    }

    public override void OnTriggerExited(Collider2D col) {
    }

    public override void OnTriggerStayed(Collider2D col) {
        if (col.tag == mob.targetMobTag) {
            if (numOfEnemies == 1) {
                if (currentTime <= 0) {
                    Attack(col);
                    mob.isHitLanded = false;
                }
                if (attackCount == 3) {
                    Invoke("Destroy", 0.1f);
                }
            } else {
                if (currentTime <= 0 && !lastMobs.Contains(col)) {
                    if (attackCount < numOfEnemies) {
                        Attack(col);
                    }
                    if (attackCount == numOfEnemies) {
                        Invoke("Destroy", 0.1f);
                    }
                }
            }
        }
    }

    private void Attack(Collider2D col) {
        float distanceX = mob.transform.position.x - col.transform.position.x;
        mob.transform.position = col.transform.position;

        if (distanceX < 0) {
            mob.FlipCharacter(true);
        }
        else {
            mob.FlipCharacter(false);
        }
        attackCount++;
        currentTime = attackInterval;
        lastMobs.Add(col);
        mob.animator.Play(mob.currentAttack.animationName.name, 0, 0);
        FXSoundSystem.Instance.PlaySound(mob.currentAttack.soundClip);
        Mob mob2 = col.GetComponent<Mob>();
        if (!mob2.isDead) {
            mob.OnHitOtherMob(col, Vector2.zero, mob2, mob.currentAttack);
        }
    }
}
