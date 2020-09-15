using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
    [HideInInspector]
    public Collider2D trigger2D;
    private Collider2D[] colliders;
    private List<Collider2D> lastColliders;
    [HideInInspector]
    public bool isTriggerEnabled = true;
    public LayerMask mobMask;
    Triggerable2D[] otherColliders;


    public Collider2D[] triggerObjects;
    private ContactFilter2D mobFilter;

    public virtual void Start() {
        colliders = GetComponents<Collider2D>();
        lastColliders = new List<Collider2D>();

        foreach (Collider2D col in colliders) {
            if (col.isTrigger) {
                trigger2D = col;
            }
        }
        mobFilter = new ContactFilter2D();
        mobFilter.SetLayerMask(mobMask);
    }

    public virtual void Update() {
        if (!isTriggerEnabled)
            return;

        otherColliders = new Triggerable2D[30];
        if (triggerObjects != null && triggerObjects.Length > 0) {
            foreach (Collider2D col in triggerObjects) {
                col.OverlapCollider(mobFilter, otherColliders);
            }
        }
        else {
            if (trigger2D != null) {
                trigger2D.OverlapCollider(mobFilter, otherColliders);
            }
        }

        // check every collision for enemy
        foreach (Triggerable2D col in otherColliders) {
            if (col != null) {
                OnTriggerStayed(col);
                if (!lastColliders.Contains(col)) {
                    if (!col.hasEntered) {
                        OnTriggerEntered(col);
                        col.hasEntered = true;
                    }
                    else {
                        OnTriggerExited(col);
                        col.hasEntered = false;
                    }
                }
            }
        }

        lastColliders.Clear();
        foreach (Triggerable2D col in otherColliders) {
            lastColliders.Add(col);
        }
    }

    public abstract void OnTriggerStayed(Collider2D col);
    public abstract void OnTriggerExited(Collider2D col);
    public abstract void OnTriggerEntered(Collider2D col);

    public void RemoveAllTriggers() {
        isTriggerEnabled = false;
    }
}

public class Triggerable2D : Collider2D
{
    public bool hasEntered;
}