using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TotalGemsCollected : MonoBehaviour, IPointerClickHandler
{
    private int clickTimes = 0;

    // Start is called before the first frame update
    void Start() {
        Refresh();
    }

    public void Refresh() {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Gems", 0).ToString();
    }

    public void SetCoins(int totalGems) {
        PlayerPrefs.SetInt("Gems", totalGems);
        Refresh();
    }

    public void OnPointerClick(PointerEventData eventData) {
        clickTimes++;

        if (clickTimes % 5 == 0) {
            int currentGems = PlayerPrefs.GetInt("Gems", 0);
            PlayerPrefs.SetInt("Gems", currentGems + 10);
            Refresh();
        }
    }
}
