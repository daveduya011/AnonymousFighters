using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSkill : Triggerable
{
    public float delay, destroyTime;
    public Vector3 positionOffset = Vector3.zero;
    public bool isPenetrating;
    public float moveSpeedX;
    protected Attack attack;
    public bool isReplaySound;
    public bool useCharacterTrigger;

    [HideInInspector]
    public Transform t;
    [HideInInspector]
    public Mob mob;
    private float initialScaleX;

    public virtual void ExecuteSkillEffect(Transform t, Mob mob) {
        this.t = t;
        this.mob = mob;

        SpawnedSkill skill = Instantiate(this, t.position, transform.rotation);
        skill.gameObject.SetActive(false);
        skill.Invoke("OnSkillStart", delay);
    }

    public virtual void OnSkillStart() {
        if (mob.isStunned || mob.isDead)
            return;

        gameObject.SetActive(true);
        initialScaleX = t.localScale.x;
        positionOffset.x *= initialScaleX;
        transform.position = t.position;
        transform.position += positionOffset;
        mob.lastMobHits.Clear();

        attack = mob.currentAttack;
        if (isReplaySound)
            FXSoundSystem.Instance.PlaySound(attack.soundClip);
        if (!useCharacterTrigger)
            mob.isTriggerEnabled = false;


            // Adjust skill scale
            Vector3 scale = new Vector3(transform.localScale.x * t.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = scale;
        Invoke("Destroy", destroyTime);
    }

    public override void Update() {
        if (moveSpeedX != 0) {
            float movementX = moveSpeedX * initialScaleX * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + movementX, transform.position.y, transform.position.z);
        }
        base.Update();
    }


    public virtual void Destroy() {
        if (!useCharacterTrigger)
            mob.isTriggerEnabled = true;
        gameObject.SetActive(false);
    }


    public override void OnTriggerStayed(Collider2D col) {
        if (col.tag == mob.targetMobTag) {
            Vector2 distance = Vector2.one;
            distance.x = col.transform.position.x - mob.transform.position.x;
            Mob mob2 = col.GetComponent<Mob>();
            if (!mob2.isDead) {
                mob.OnHitOtherMob(col, distance, mob2, attack);

                if (!isPenetrating) {
                    Invoke("Destroy", 0.02f);
                }
            }
        }
    }


    public override void OnTriggerEntered(Collider2D col) {
    }

    public override void OnTriggerExited(Collider2D col) {
    }

}
