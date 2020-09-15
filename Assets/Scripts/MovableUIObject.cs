using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableUIObject : MonoBehaviour, IDragHandler
{
    public Vector3 initialPos;


    public void OnDrag(PointerEventData eventData) {
        transform.position += (Vector3)eventData.delta;
    }

    void Awake() {
        initialPos = transform.localPosition;
    }
}
