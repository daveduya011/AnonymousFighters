using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableHealth : Triggerable
{
    public float healthValue = 100;
    public float destroyTime = 20f;
    private Rigidbody2D rb;
    public override void OnTriggerEntered(Collider2D col) {
        
    }

    public override void OnTriggerExited(Collider2D col) {
    }

    public override void OnTriggerStayed(Collider2D col) {
        if (col.tag == "Player") {
            gameObject.SetActive(false);
            Mob mob = col.GetComponent<Mob>();
            mob.IncreaseHealth(healthValue);
            mob.SpawnHealthText(healthValue);
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
