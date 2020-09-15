using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalGems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        GetComponent<Text>().text = GameManager.Instance.tempGems.ToString();
    }
}
