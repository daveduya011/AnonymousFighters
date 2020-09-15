using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : Mob
{
    [Header("Game Object")]
    [HideInInspector]
    public Text healthBarText;
    public float jumpForce = 5;
    public int maxJumps = 2;
    public TrailRenderer trail;
    public ShopManager.TopbarItem heroType;
    public SpriteResolver weaponSprite;
    private Joystick joystick;

    private bool canJump;
    private int jumps = 0;

    private ReviveInfoView infoView;


    // Start is called before the first frame update
    public override void Start() {
        base.Start();


        GameManager.Instance.highScore = PlayerPrefs.GetInt("HighScore", 0);
        healthBarText = GameObject.FindGameObjectWithTag("MainUICanvas").GetComponent<MainUICanvas>().healthText;
        SetInitialWeapon();
        infoView = GameObject.FindGameObjectWithTag("MainUICanvas").GetComponent<MainUICanvas>().reviveInfoView;

        if (joystick == null) {
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        }
        // Enable the trail initially
        if (trail != null) {
            trail.enabled = false;
        }

        healthBarText.text = health + " / " + initialHealth;
    }

    public void Revive() {
        isDead = false;
        health = initialHealth;
        Invoke("ShowCharacter", 0.2f);
    }

    public void ShowCharacter() {
        gameObject.SetActive(true);
    }

    public void GameOver() {
        GameManager.Instance.GameOver();
    }

    private void SetInitialWeapon() {
        InventoryData inventoryData = SaveSystem.LoadInventory();
        MobUpgrade upgrade = null;
        if (heroType == ShopManager.TopbarItem.KNIGHT) {
            if (inventoryData.knightEquippedItem != null) {
                weaponSprite.SetCategoryAndLabel(inventoryData.knightEquippedItem[0], inventoryData.knightEquippedItem[1]);
            }

            upgrade = inventoryData.knightMobUpgrade;
        }
        else if (heroType == ShopManager.TopbarItem.MAGE) {
            if (inventoryData.mageEquippedItem != null) {
                weaponSprite.SetCategoryAndLabel(inventoryData.mageEquippedItem[0], inventoryData.mageEquippedItem[1]);
            }
            upgrade = inventoryData.mageMobUpgrade;
        }
        else if (heroType == ShopManager.TopbarItem.SPEARMAN) {
            if (inventoryData.spearEquippedItem != null) {
                weaponSprite.SetCategoryAndLabel(inventoryData.spearEquippedItem[0], inventoryData.spearEquippedItem[1]);
            }
            upgrade = inventoryData.spearMobUpgrade;
        }

        if (upgrade != null) {
            health += upgrade.HP;
            strength += upgrade.AD;
            moveSpeed += upgrade.MSPD;
            attackSpeed += upgrade.ASPD;
            armor += upgrade.AR;
            finalAttackSpeed = 1 / attackSpeed;

            initialHealth = health;
            healthSlider.value = 1 / (initialHealth / health);
        }
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        if (isDead) {
            return;
        }
        if (isPerformingSkill && !skillData.skills[currentSkillIndex].walkWhileAttacking)
            return;
        if (isStunned)
            return;
        if (isAttacking && (!isPerformingSkill))
            return;
        // Set the value of speed and movement based on input
        // To customize the inputs, go to unity > Edit > Project Settings > Input
        float speed = moveSpeed;

        float movementX = 0;
        if (joystick.Horizontal > 0.3f) {
            movementX = speed;
        }
        else if (joystick.Horizontal < -0.3f) {
            movementX = -speed;
        }
        else {
            movementX = Input.GetAxis("Horizontal") * speed;
        }

        Vector3 mousePos = Vector3.zero;

        // If Ground is touched, then enable jumping
        if (isGrounded) {
            // Reset number of jumps and enable jumping again
            jumps = 0;
            canJump = true;
            // disable jump animation 
            animator.SetBool("isJumping", false);

            // If the character is not attacking
            // Move the character based on input
            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }

        // Else if the player is not on the ground, 
        // then move freely based on input
        else {
            if (joystick.Horizontal != 0) {
                movementX = joystick.Horizontal * speed;
            }
            rb.velocity = new Vector2(movementX, rb.velocity.y);
        }



        // Enable walk animation if horizontal input is more not zero
        if (movementX != 0) {
            animator.SetBool("isWalking", true);

            // Flip the character necessarily based on direction of walk
            if ((!isStunned && !isAttacking) || (isPerformingSkill && skillData.skills[currentSkillIndex].walkWhileAttacking)) {
                if (movementX < 0) {
                    FlipCharacter(true);
                }
                else if (movementX > 0) {
                    FlipCharacter(false);
                }
            }
        }
        // otherwise disable
        else {
            animator.SetBool("isWalking", false);
        }


        // If jump is pressed
        if (Input.GetAxis("Jump") > 0 || joystick.Vertical > 0.3f) {
            // Then jump as necessarily
            if (canJump) {
                canJump = false;
                // move the character y axis based on jump force
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                // Enable jump animation
                animator.SetBool("isJumping", true);

                //Wait for another jump, if double jumping is enabled
                if (maxJumps > 1) {
                    StartCoroutine(WaitForJump(0.5f));
                }
            }
        }

    }

    public override void UpdateHealthBar() {
        float healthTxt = health < 0 ? 0 : health;
        healthBarText.text = (int)healthTxt + " / " + (int)initialHealth;
        base.UpdateHealthBar();
    }
    public override void Attack() {
        base.Attack();
        // Enable sword trails
        if (trail != null) {
            trail.enabled = true;
        }
    }

    private IEnumerator WaitForJump(float v) {
        // Wait for x seconds before allowing player to have another jump
        yield return new WaitForSeconds(v);
        if (!canJump) {
            jumps++;

            // if total jumps performed is less than maximum jumps, then enable jump again
            if (jumps < maxJumps) {
                canJump = true;
            }
        }
    }

    public override void EndAttack() {
        base.EndAttack();
        // disables the trail after 0.1 seconds to have decent effects
        Invoke("EndTrail", 0.1f);
    }
    public override void Die() {
        base.Die();
        ShowRevive();
    }

    private void ShowRevive() {
        infoView.ShowInfoView();
    }

    // Ends the trail effect if only the 
    private void EndTrail() {
        if (!isAttacking) {
            if (trail != null)
                trail.enabled = false;
        }
    }

}
