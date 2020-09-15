using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;

public class HeroShopItem : MonoBehaviour
{
    public ShopManager.TopbarItem heroType = ShopManager.TopbarItem.UNSET;
    public Sprite image;
    public bool isOwned;
    public bool isPayInGems;
    public string heroName = "";
    public int cost;
    public Material unavailableMaterial;
    public Material availableMaterial;

    void Awake() {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        InventoryData data = SaveSystem.LoadInventory();

        if (data.isMageOwned && heroType == ShopManager.TopbarItem.MAGE ||
            data.isSpearOwned && heroType == ShopManager.TopbarItem.SPEARMAN ||
            heroType == ShopManager.TopbarItem.KNIGHT
            ) {
            isOwned = true;
            foreach (SpriteRenderer sprite in sprites) {
                sprite.material = availableMaterial;
            }
        } else {
            foreach (SpriteRenderer sprite in sprites) {
                sprite.material = unavailableMaterial;
            }
        }
    }
}
