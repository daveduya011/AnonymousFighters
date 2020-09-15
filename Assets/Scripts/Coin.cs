using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Triggerable
{
    public int coinValue = 10;
    public float destroyTime = 20f;
    private Rigidbody2D rb;
    public override void OnTriggerEntered(Collider2D col) {
        
    }

    public override void OnTriggerExited(Collider2D col) {
    }

    public override void OnTriggerStayed(Collider2D col) {
        if (col.tag == "Player") {
            GameManager.Instance.AddCoin(coinValue);
            FXSoundSystem.Instance.PlaySound(FXSoundSystem.Instance.coinSound, 0.5f);
            gameObject.SetActive(false);
        }
    }
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Start() {
        base.Start();
        float randX = Random.Range(-4, 4f);
        rb.AddForce(Vector3.up * 4f + Vector3.right * randX, ForceMode2D.Impulse);

        Invoke("Destroy", destroyTime);

    }
    void Destroy() {
        gameObject.SetActive(false);
    }

}