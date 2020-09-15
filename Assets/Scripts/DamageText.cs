using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public void ShowHealth(int health) {
        gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = health <= 0 ? health.ToString() : 
            "+" + health.ToString();
        Invoke("Destroy", 1f);
    }

    public void Destroy() {
        gameObject.SetActive(false);
    }
}
