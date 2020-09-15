using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public bool isInEditMode;
    public ToggleGroup topBar;
    public ToggleGroup sideBar;
    public RectTransform scrollbarContent;
    public InfoView infoView;
    public RectTransform ownedText;
    public RectTransform equippedText;
    public TotalCoinsCollected totalCoinsCollected;

    public ShopItem initialKnightItem;
    public ShopItem initialSpearItem;
    public ShopItem initialMageItem;

    private List<string[]> ownedItems;

    public enum SidebarItem
    {
        UPGRADES,
        HEROES,
        GEMS,
        UNSET
    }
    public enum TopbarItem
    {
        KNIGHT,
        SPEARMAN,
        MAGE,
        UNSET
    }
    void Start() {
        Refresh();
    }
    public void Refresh() {
        ShopItem[] items = scrollbarContent.GetComponentsInChildren<ShopItem>(true);
        
        InventoryData data = LoadSavedInventory();
        ownedItems = data.ownedItems;

        if (data.knightEquippedItem == null) {
            data.knightEquippedItem = FindItem(initialKnightItem);
        }
        if (data.mageEquippedItem == null) {
            data.mageEquippedItem = FindItem(initialMageItem);
        }
        if (data.spearEquippedItem == null) {
            data.spearEquippedItem = FindItem(initialSpearItem);
        }

        foreach (ShopItem item in items) {

            if (ownedItems != null) {
                string[][] array = ownedItems.ToArray();
                foreach (string[] ownedItem in array) {
                    if (Search(ownedItem, item)) {
                        item.isOwned = true;
                        break;
                    }
                }
            }
            if (Search(data.knightEquippedItem, item)) {
                item.isEquipped = true;
            }
            else if (Search(data.mageEquippedItem, item)) {
                item.isEquipped = true;
            }
            else if (Search(data.spearEquippedItem, item)) {
                item.isEquipped = true;
            } else {
                item.isEquipped = false;
            }

            item.Refresh();
        }
    }

    private InventoryData LoadSavedInventory() {
        return SaveSystem.LoadInventory();
    }

    private string[] FindItem(ShopItem item) {
        return new string[] { item.GetCategory(), item.GetLabel() };
    }

    public void ShowItemInfo(ShopItem shopItem) {
        infoView.ShowInfoView(shopItem);
    }

    private bool Search(string[] searchItem, ShopItem item) {
        return searchItem[0] == item.GetCategory() && searchItem[1] == item.GetLabel();
    }

    public void HideItemInfo() {
        infoView.gameObject.SetActive(false);
    }

    public void BuyItem() {
        ShopItem item = infoView.currentItem;
        int totalCoins = PlayerPrefs.GetInt("Coins", 0);

        if (item.isOwned)
            return;

        if (totalCoins >= item.cost) {
            item.isOwned = true;
            totalCoins -= item.cost;

            totalCoinsCollected.SetCoins(totalCoins);

            ownedItems.Add(FindItem(item));

            InventoryData data = LoadSavedInventory();
            data.ownedItems = ownedItems;
            SaveSystem.SaveInventory(data);

            EquipItem();

        } else {
            GameManager.Instance.ShowTextPrompt("Can't buy this item. Insufficient number of coins");
        }
    }

    public void EquipItem() {
        ShopItem item = infoView.currentItem;

        if (!item.isOwned)
            return;

        InventoryData data = LoadSavedInventory();
        switch(item.hero) {
            case TopbarItem.KNIGHT:
                data.knightEquippedItem = FindItem(item);
                data.knightMobUpgrade = item.mobUpgrade;
                break;
            case TopbarItem.MAGE:
                data.mageEquippedItem = FindItem(item);
                data.mageMobUpgrade = item.mobUpgrade;
                break;
            case TopbarItem.SPEARMAN:
                data.spearEquippedItem = FindItem(item);
                data.spearMobUpgrade = item.mobUpgrade;
                break;
        }
        SaveSystem.SaveInventory(data);

        HideItemInfo();
        Refresh();
    }

}

[System.Serializable]
public class Upgrade
{
    
}