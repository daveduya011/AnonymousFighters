using System;
using UnityEngine;

public class Enemy : Mob
{
    // Initialize variables
    private PlayerController player;
    public Collider2D damageTrigger;
    private Collider2D initialTrigger;
    private Vector3 direction;

    public override void Start() {
        base.Start();
        // Set variable values
        player = GameObject.FindGameObjectWithTag(targetMobTag).GetComponent<PlayerController>();
        initialTrigger = trigger2D;

        initialHealth *= (0.2f * GameManager.Instance.difficulty)  * GameManager.Instance.levelsFinished + 1;
        strength *= (0.2f * GameManager.Instance.difficulty) * GameManager.Instance.levelsFinished + 1;
        health = initialHealth;
        damageText = GameManager.Instance.enemyDamageText;
    }

    public override void Update() {
        base.Update();
        if (isDead)
            return;

        // Get the distance of enemy to player
        direction = player.transform.position - transform.position;

        // Follow the direction of the player
        // if it is not attacking
        if (!isStunned && !isAttacking && isGrounded) {
            direction.y = 0;
            if (currentAttackInterval <= 0) {
                rb.velocity = direction.normalized * moveSpeed;
                animator.SetBool("isWalking", true);
            }
            else {
                animator.SetBool("isWalking", false);
            }
        }
        if (!isStunned && !isAttacking) {
            FlipEnemy();
            if (trigger2D != initialTrigger)
                trigger2D = initialTrigger;
        }
    }

    private void FlipEnemy() {
        // Flip the enemy as necessarily to the direction
        if (direction.x < 0) {
            FlipCharacter(true);
        }
        else {
            FlipCharacter(false);
        }
    }

    public override void Attack() {
        base.Attack();
        if (!isStunned) {
            trigger2D = damageTrigger;
        }
    }

    public override void EndAttack() {
        base.EndAttack();
        trigger2D = initialTrigger;
        FlipEnemy();
    }
    //This method is called when another object has been collided within its trigger area
    public override void OnTriggerStayed(Collider2D col) {

        if (col.tag == targetMobTag && trigger2D) {
            if (trigger2D == initialTrigger) {
                Attack();
            } else {
                base.OnTriggerStayed(col);
            }
        }
    }

    public override void Die() {
        base.Die();
        Invoke("Destroy", 1.5f);
        GameManager.Instance.EnemyDied();
    }

}
