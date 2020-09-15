using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroInfoView : MonoBehaviour
{
    public Image image;
    public Image gemIcon;
    public Image coinIcon;
    public Text title;
    public Text price;
    public Button buyButton;
    private HeroShopItem currentItem;
    public TotalCoinsCollected totalCoinsCollected;
    public TotalGemsCollected totalGemsCollected;

    void Start() {
    }

    public void ShowInfoView(HeroShopItem item) {
        gameObject.SetActive(true);
        currentItem = item;

        this.title.text = item.heroName.ToUpper();
        this.price.text = item.cost.ToString();
        this.image.sprite = item.image;

        gemIcon.gameObject.SetActive(false);
        coinIcon.gameObject.SetActive(false);

        if (item.isPayInGems) {
            gemIcon.gameObject.SetActive(true);
        } else {
            coinIcon.gameObject.SetActive(true);
        }
    }
    public void BuyItem() {
        int totalCoins = PlayerPrefs.GetInt("Coins", 0);
        int totalGems = PlayerPrefs.GetInt("Gems", 0);

        if ((totalCoins >= currentItem.cost && !currentItem.isPayInGems) || (totalGems >= currentItem.cost && currentItem.isPayInGems)) {
            currentItem.isOwned = true;

            if (!currentItem.isPayInGems) {
                totalCoins -= currentItem.cost;
                totalCoinsCollected.SetCoins(totalCoins);

            } else {
                totalGems -= currentItem.cost;
                totalGemsCollected.SetCoins(totalGems);
            }
            

            InventoryData data = SaveSystem.LoadInventory();
            if (currentItem.heroType == ShopManager.TopbarItem.MAGE) {
                data.isMageOwned = true;
            } else if (currentItem.heroType == ShopManager.TopbarItem.SPEARMAN) {
                data.isSpearOwned = true;
            }
            SaveSystem.SaveInventory(data);
            HideInfoView();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else {
            GameManager.Instance.ShowTextPrompt("Can't buy this item. Insufficient number of coins or gems");
        }
    }

    public void HideInfoView() {
        gameObject.SetActive(false);
    }
}
