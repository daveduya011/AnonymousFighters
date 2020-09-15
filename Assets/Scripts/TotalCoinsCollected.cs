using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TotalCoinsCollected : MonoBehaviour, IPointerClickHandler
{
    private int clickTimes = 0;
    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh() {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    public void SetCoins(int totalCoins) {
        PlayerPrefs.SetInt("Coins", totalCoins);
        Refresh();
    }

    public void OnPointerClick(PointerEventData eventData) {
        clickTimes++;

        if (clickTimes % 5 == 0) {
            int currentCoins = PlayerPrefs.GetInt("Coins", 0);
            PlayerPrefs.SetInt("Coins", currentCoins + 10000);
            Refresh();
        }
    }
}
