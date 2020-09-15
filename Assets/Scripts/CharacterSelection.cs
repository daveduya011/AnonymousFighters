using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public PlayerController characterPrefab;
    public ShopManager.TopbarItem heroType;
    public SpriteResolver weaponSprite;
    public HeroShopItem heroShopItem;
    public HeroInfoView heroInfoView;


    void Start() {
        InventoryData inventoryData = SaveSystem.LoadInventory();
        if (heroType == ShopManager.TopbarItem.KNIGHT) {
            if (inventoryData.knightEquippedItem != null) {
                weaponSprite.SetCategoryAndLabel(inventoryData.knightEquippedItem[0], inventoryData.knightEquippedItem[1]);
            }

        }
        else if (heroType == ShopManager.TopbarItem.MAGE) {
            if (inventoryData.mageEquippedItem != null) {
                weaponSprite.SetCategoryAndLabel(inventoryData.mageEquippedItem[0], inventoryData.mageEquippedItem[1]);
            }
        }
        else if (heroType == ShopManager.TopbarItem.SPEARMAN) {
            if (inventoryData.spearEquippedItem != null) {
                weaponSprite.SetCategoryAndLabel(inventoryData.spearEquippedItem[0], inventoryData.spearEquippedItem[1]);
            }
        }

        RefreshItem();
    }

    public void RefreshItem() {
        Toggle toggle = GetComponent<Toggle>();

        if (heroShopItem.isOwned) {
            toggle.interactable = true;
        } else {
            toggle.interactable = false;
        }
    }

    public void CheckCharacter() {
        if (!heroShopItem.isOwned) {
            heroInfoView.ShowInfoView(heroShopItem);
        }
    }
}
