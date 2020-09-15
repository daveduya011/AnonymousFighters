using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveInfoView : MonoBehaviour
{
    public Text price;
    [HideInInspector]
    public PlayerController player;
    public void ShowInfoView() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gameObject.SetActive(true);

        this.price.text = GameManager.Instance.reviveCost.ToString();

        Debug.Log("Gems: " + GameManager.Instance.gems);
        Debug.Log("Gems in prefs: " + PlayerPrefs.GetInt("Gems", 0));
    }
    public void BuyItem() {
        int totalGems = PlayerPrefs.GetInt("Gems", 0);
        if (totalGems + GameManager.Instance.gems >= GameManager.Instance.reviveCost) {
            GameManager.Instance.gems -= GameManager.Instance.reviveCost;

            if (GameManager.Instance.gems < 0) {
                totalGems += GameManager.Instance.gems;
                GameManager.Instance.gems = 0;
            }
            PlayerPrefs.SetInt("Gems", totalGems);

            player.Revive();

            gameObject.SetActive(false);
        }
        else {
            GameManager.Instance.ShowTextPrompt("Not enough gems");
        }
    }

    public void HideInfoView() {
        player.GameOver();
        gameObject.SetActive(false);
    }
}
