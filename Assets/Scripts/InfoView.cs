using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoView : MonoBehaviour
{
    public Image image;
    public Text title;
    public Text price;
    public Text description;
    public Text upgrade;
    public Button buyButton;
    public Button equipButton;

    [HideInInspector]
    public ShopItem currentItem;

    void Start() {
    }

    public void ShowInfoView(ShopItem shopItem) {
        currentItem = shopItem;
        gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        equipButton.interactable = true;

        this.title.text = shopItem.displayName.ToUpper();
        this.price.text = shopItem.cost.ToString();
        this.description.text = shopItem.description.ToString();
        this.image.sprite = shopItem.image.sprite;

        if (shopItem.isOwned) {
            equipButton.gameObject.SetActive(true);
            if (shopItem.isEquipped) {
                equipButton.interactable = false;
            }
        } else {
            buyButton.gameObject.SetActive(true);
        }

        this.upgrade.text = shopItem.upgradeDescription.ToString();

        if (currentItem.mobUpgrade.HP != 0f) {
            this.upgrade.text += "+ " + shopItem.mobUpgrade.HP + " HP\n";
        }
        if (currentItem.mobUpgrade.AD != 0f) {
            this.upgrade.text += "+ " + shopItem.mobUpgrade.AD + " Strength\n";
        }
        if (currentItem.mobUpgrade.ASPD != 0f) {
            this.upgrade.text += "+ " + shopItem.mobUpgrade.ASPD + " Attack Speed\n";
        }
        if (currentItem.mobUpgrade.AR != 0f) {
            this.upgrade.text += "+ " + shopItem.mobUpgrade.AR + " Armor\n";
        }
        if (currentItem.mobUpgrade.MSPD != 0f) {
            this.upgrade.text += "+ " + shopItem.mobUpgrade.MSPD + " Movement Speed\n";
        }
    }
}
