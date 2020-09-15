using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class Mob : Triggerable
{

    // Initialization of variables

    [Header("Mob")]
    public string targetMobTag = "Player";
    public float nextComboAttackSpan = 0.25f;

    [HideInInspector]
    public Slider healthSlider;

    [Header("Data")]
    public ComboAttack attacksData;
    public SkillData skillData;
    public MobStats mobStats;

    [Header("Attacks")]

    public AnimationClip hitAnimation;


    public AnimationClip hitLaunch;
    public AnimationClip dieAnimation;
    public AnimationClip idleAnimation;

    protected DamageText damageText;
    protected DamageText insDamageText;
    protected DamageText insHealthText;
    protected float moveSpeed = 0;
    protected float finalAttackSpeed = 0;
    protected float attackSpeed = 0;
    protected float health = 0;
    protected float armor = 0;
    protected float strength = 0;
    protected float currentDamageInterval = 0;
    protected float initialHealth = 0;

    [HideInInspector]
    public float currentKnockedTime = 0;
    protected float currentAttackDuration = 0;
    protected float currentAttackInterval = 0;
    protected float currentSoundInterval = 0;

    public LayerMask platformMask;
    [HideInInspector]
    public Animator animator;

    protected new Collider2D collider2D;
    protected Rigidbody2D rb;

    protected bool isGrounded;
    protected bool isTouched;
    protected bool isHit;
    protected bool isLaunched;
    protected bool isInCombo;
    [HideInInspector]
    public bool isHitLanded;
    protected bool isHitHasAttackOnCall;

    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool isStunned;
    [HideInInspector]
    public List<Mob> lastMobHits = null;


    [HideInInspector]
    public bool isPerformingSkill;

    protected int directionX = 1;
    [HideInInspector]
    public Attack currentAttack;
    [HideInInspector]
    public Attack currentHit;
    protected int currentAttackIndex = 0;
    protected int currentSkillIndex = 0;

    protected float distToGround;
    protected Vector2 attackStepDestination;
    protected Vector2 hitStepDestination;
    [HideInInspector]
    public Vector2 currentKnockStep;
    public float minCoinsDrops = 1;
    public float maxCoinDrops = 4;
    public float healthCoinChance = 0.15f;
    public float gemChance = 0.01f;
    private Vector3 initialScale;

    [HideInInspector]
    //public event EventHandler AttackEnded;
    //public delegate void EventHandler(EventArgs e);

    public override void Start() {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();

        if (healthSlider == null) {
            healthSlider = GameObject.FindGameObjectWithTag("MainUICanvas").GetComponent<MainUICanvas>().healthSlider;
        }

        lastMobHits = new List<Mob>();
        initialScale = transform.localScale;
        damageText = GameManager.Instance.playerDamageText;
        // Set variables
       

        distToGround = collider2D.bounds.extents.y;

        moveSpeed = mobStats.MSPD;
        attackSpeed = mobStats.ASPD;
        finalAttackSpeed = 1 / attackSpeed;
        initialHealth = mobStats.HP;
        armor = mobStats.AR;
        strength = mobStats.AD;
        health = initialHealth;

        healthSlider.value = 1 / (initialHealth / health);

        foreach (Attack attack in attacksData.attacks) {
            attack.onCall.AddListener((t, mob) => {
                mob.isHitHasAttackOnCall = true;
            });
        }
    }

    public override void Update() {
        // Detect if the character is in the ground
        isTouched = Physics2D.IsTouchingLayers(collider2D, platformMask);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.3f, platformMask) && isTouched;
        

        // If dead, play animation and prevent character from doing anything
        if (isDead) {
            animator.Play(dieAnimation.name);
            return;
        }

        if (isAttacking) {
            WhileAttacking();
        }
        else {
            animator.speed = 1;
        }

        if (isStunned) {
            if (currentKnockedTime > 0) {
                currentKnockedTime -= Time.deltaTime;
            }
            else {
                DisableStun();
            }
        }

        PerformHitStep(hitStepDestination, 0.1f);
        //If launched
        // check for launch status
        if (isLaunched && !isHit) {
            Invoke("CheckLaunch", 0.1f);
        }

        if (health <= 0) {
            Die();
        }
        if (currentAttackInterval > 0) {
            currentAttackInterval -= Time.deltaTime;
        }

        base.Update();
    }

    public void WhileAttacking() {
        // Attack Counters

        if (currentAttack.soundClip != null) {
            if (currentSoundInterval > 0)
                currentSoundInterval -= Time.deltaTime;
            else {
                if (currentAttack.soundInterval > 0) {
                    currentSoundInterval = currentAttack.soundInterval;
                }
                else {
                    currentSoundInterval = float.PositiveInfinity;
                }
                FXSoundSystem.Instance.PlaySound(currentAttack.soundClip);
            }
        }
        
        // Count for attack duration and end it manually
        if (currentAttackDuration > 0) 
            currentAttackDuration -= Time.deltaTime;
        else 
            EndAttack();

        // Count for damage interval
        if (currentDamageInterval > 0) 
            currentDamageInterval -= Time.deltaTime;
        
        // Enable attack step only if walkWhileAttacking is true or if not performing skill
        if ((isPerformingSkill && !skillData.skills[currentSkillIndex].walkWhileAttacking) || !isPerformingSkill) 
            PerformAttackStep(attackStepDestination, 0.075f);
    }

    public virtual void Die() {
        isDead = true;

        float randNum = Random.Range(minCoinsDrops, maxCoinDrops);
        for (int i = 0; i < randNum; i++) {
            float randType = Random.Range(0f, maxCoinDrops);
            Vector3 pos = transform.position;
            pos.y += 1;
            if (randType <= maxCoinDrops * gemChance) {
                Instantiate(GameManager.Instance.gemPrefab, pos, Quaternion.identity);
            } else if (randType <= maxCoinDrops * (healthCoinChance + gemChance)) {
                Instantiate(GameManager.Instance.pickableHealthPrefab, pos, Quaternion.identity);
            }
            else {
                Instantiate(GameManager.Instance.coinPrefab, pos, Quaternion.identity);
            }
        }
    }
    private void Destroy() {
        gameObject.SetActive(false);
    }

    public void PerformAttackStep(Vector2 step, float speed) {
        Vector2 pos = Vector2.Lerp(transform.position, step, speed);
        if (pos.y != 0 && pos.x != 0) {
            transform.position = pos;
        }
    }
    public void PerformHitStep(Vector2 step, float speed) {
        if (isHit && !isLaunched && mobStats.isKnockable) {
            Vector2 pos = Vector2.Lerp(transform.position, step, speed);
            if (pos.y != 0 && pos.x != 0) {
                transform.position = pos;
            }
        }
    }

    public virtual void EndAttack() {

        // End attack as necessarily
        animator.SetBool("isAttacking", false);
        animator.Play(idleAnimation.name);
        isAttacking = false;
        isInCombo = true;
        isHitLanded = false;
        lastMobHits.Clear();

        if (isPerformingSkill) {
            isPerformingSkill = false;
            animator.Play(idleAnimation.name);
        }
        float nextComboSpeed = finalAttackSpeed > currentAttack.lengthInSeconds ?
            nextComboAttackSpan + (finalAttackSpeed - currentAttack.lengthInSeconds) : nextComboAttackSpan;
        Invoke("EndAttackCombo", nextComboSpeed);
    }


    public virtual void Attack() {
        if (currentAttackInterval > 0)
            return;

        if (!isStunned && !isAttacking) {
            // Enable the attack animation
            animator.SetBool("isAttacking", true);
            isAttacking = true;

            if (isInCombo && !isPerformingSkill) {
                currentAttackIndex++;
                if (currentAttackIndex == attacksData.attacks.Length) {
                    currentAttackIndex = 0;
                }
            }
            else {
                currentAttackIndex = 0;
            }


            if (isPerformingSkill) {
                currentAttack = skillData.skills[currentSkillIndex];
                skillData.skills[currentSkillIndex].currentTime = skillData.skills[currentSkillIndex].waitTime;

                float attackLength = currentAttack.lengthInSeconds;
                if (currentAttack.lengthInSeconds == 0) {
                    attackLength = currentAttack.animationName.length;
                }
                else {
                    attackLength = currentAttack.lengthInSeconds;
                }
                animator.speed = 1;
                currentAttackDuration = attackLength;
            }
            else {
                currentAttack = attacksData.attacks[currentAttackIndex];
                currentAttackInterval = finalAttackSpeed;

                float attackLength = currentAttack.lengthInSeconds;

                if (currentAttack.lengthInSeconds == -1) {
                    attackLength = currentAttack.lengthInSeconds;
                    animator.speed = 1;
                    currentAttackDuration = currentAttack.animationName.length;
                } else {

                    if (currentAttack.lengthInSeconds == 0) {
                        attackLength = currentAttack.animationName.length;
                    }
                    else {
                        attackLength = currentAttack.lengthInSeconds;
                    }
                    animator.speed = finalAttackSpeed < attackLength ? attackLength / finalAttackSpeed : 1;

                    currentAttackDuration = finalAttackSpeed < attackLength ?
                    finalAttackSpeed : attackLength;
                }
                
                    
            }
            currentSoundInterval = 0;
            currentDamageInterval = currentAttack.damageInterval;
            Vector3 attackStep = currentAttack.attackStep;
            attackStep.x *= directionX;
            attackStepDestination = CalculateStepDestination(attackStep);
            animator.Play(currentAttack.animationName.name, 0, currentAttack.startOffset);


            // Call events together with the attack
            currentAttack.onCall.Invoke(transform, this);
        }
    }

    private Vector3 CalculateStepDestination(Vector2 step) {
        Vector2 attackDest = step;
        return (Vector2)transform.localPosition + attackDest;
    }

    public void SpawnDamageText(float damage) {
        Vector3 pos = transform.position;
        pos.y += 1.3f;
        if (insDamageText == null) {
            insDamageText = Instantiate(damageText, pos, Quaternion.identity);
        } else {
            insDamageText.gameObject.SetActive(false);
        }
        insDamageText.ShowHealth((int)damage);
        insDamageText.transform.position = pos;
    }

    public void SpawnHealthText(float health) {
        Vector3 pos = transform.position;
        pos.y += 1.3f;
        if (insHealthText == null) {
            insHealthText = Instantiate(GameManager.Instance.healthIncreaseText, pos, Quaternion.identity);
        }
        else {
            insHealthText.gameObject.SetActive(false);
        }
        insHealthText.ShowHealth((int)health);
        insHealthText.transform.position = pos;
    }
    public virtual void Hit(Vector3 distance, float attackDuration, Attack hitAttack, Mob mob) {
        if (isDead)
            return;


        float damage;
        if (hitAttack.IsSkill(hitAttack)) {
            damage = CalculateDamage(mob.strength * mob.skillData.skills[mob.currentSkillIndex].damage);
        } else {
            damage = CalculateDamage(mob.strength);
        }
        SpawnDamageText(-damage);
        health -= damage;

        isHit = true;
        currentHit = hitAttack;
        currentKnockedTime = hitAttack.stunTime;


        if (mobStats.isKnockable) {
            isStunned = true;

            animator.SetBool("isAttacking", false);
            animator.SetBool("isHit", true);


            currentKnockStep = hitAttack.knockForce;
            currentKnockStep.x *= distance.normalized.x;
            hitStepDestination = CalculateStepDestination(currentKnockStep);

            if (!hitAttack.isLauncherAttack) {
                // Enable hit 
                animator.Play(hitAnimation.name, 0, 0);
            }
            else {
                HitLaunch(hitAttack);
            }
            
        }

        UpdateHealthBar();
        Invoke("EndHit", attackDuration + 0.1f);
    }

    public virtual void UpdateHealthBar() {
        healthSlider.value = 1 / (initialHealth / health);
    }

    public void HitLaunch(Attack attack) {
        // Launch enemy in the air
        if (!isLaunched) {
            isLaunched = true;
            animator.SetBool("isLaunched", true);
            animator.Play(hitLaunch.name);
            currentKnockedTime = float.PositiveInfinity;

            rb.AddForce(attack.knockForce, ForceMode2D.Impulse);
        }
    }

    private void CheckLaunch() {
        // If ground has been reached, then set isLaunched to false
        if (isGrounded) {
            animator.SetBool("isLaunched", false);
            animator.Play(hitAnimation.name);
            isLaunched = false;
            currentKnockedTime = currentHit.stunTime;
        }
        if (isHit) {
            animator.Play("Enemy_Fall");
            rb.AddForce(Vector3.down * 1f, ForceMode2D.Impulse);
        }
    }

    private void EndHit() {
        if (isHit) {
            isHit = false;
        }
        isHitHasAttackOnCall = false;
    }

    private void EndAttackCombo() {
        if (isInCombo) {
            isInCombo = false;
        }
    }

    private void DisableStun() {
        isStunned = false;
        currentKnockedTime = currentHit.stunTime;
        animator.SetBool("isHit", false);
        EndHit();
    }

    public virtual void FlipCharacter(bool v) {
        if (v) {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            directionX = -1;
        }
        else {
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            directionX = 1;
        }
    }

    public override void OnTriggerStayed(Collider2D col) {
        // if is attacking, then check for collisions
        if (col.tag == targetMobTag) {
            if (isAttacking) {
                Vector2 distance = Vector2.one;
                distance.x = col.transform.position.x - transform.position.x;
                Mob mob = col.GetComponent<Mob>();
                OnHitOtherMob(col, distance, mob, currentAttack);
            }
        }
    }
    public override void OnTriggerEntered(Collider2D col) {
        
    }

    public override void OnTriggerExited(Collider2D col) {
    }


    public virtual void OnHitOtherMob(Collider2D otherMobCollider, Vector2 distance, Mob mob, Attack attack) {
        if (mob.isPerformingSkill)
            return;
        if (isDead || mob.isDead)
            return;

        float attackDuration = currentAttackDuration - attack.startOffset;
        if (currentAttack.damageInterval != 0) {
            if (currentDamageInterval <= 0) {
                mob.Hit(distance, attackDuration, attack, this);
            }
        }
        else {
            if (!mob.IsHitLanded(this, lastMobHits)) {
                lastMobHits.Add(mob);
                isHitLanded = true;
                mob.Hit(distance, attackDuration, attack, this);
            }
        }

    }

    public bool IsHitLanded(Mob mob, List<Mob> lastHits) {
        return mob.isHitLanded && (lastHits.Contains(this));
    }

    public void ExecuteSkill(AnimationClip skill, int skillIndex) {
        currentSkillIndex = skillIndex;
        isPerformingSkill = true;
        Attack();
    }

    private float CalculateDamage(float attackDamage) {
        // Combine attack damage received to the strength of the enemy
        return attackDamage * (100 / (100 + armor));
    }

    public void IncreaseHealth(float healthValue) {
        health = Mathf.Clamp(health + healthValue, 0, initialHealth);
        UpdateHealthBar();
    }
}
