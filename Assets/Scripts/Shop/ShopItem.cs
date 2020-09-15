using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    public bool isOwned;
    public bool isEquipped;

    [Tooltip("if displayName is empty, then display label instead")]
    public string displayName = "";
    public string label;

    [TextArea(8, 20)]
    public string description;
    
    public int cost;
    public ShopManager.SidebarItem type = ShopManager.SidebarItem.UPGRADES;
    public ShopManager.TopbarItem hero = ShopManager.TopbarItem.KNIGHT;

    public Image image;
    public Text priceText;

    private RectTransform ownedText;
    private RectTransform equippedText;

    public SpriteResolver spriteResolver;
    public SpriteLibrary spriteLibrary;
    public ShopManager shopManager;

    public MobUpgrade mobUpgrade;

    [TextArea(8, 20)]
    public string upgradeDescription;

    public void Start() {

        if (displayName.Trim() == "") {
            displayName = label;
        }
    }

    public void Refresh() {
        gameObject.SetActive(false);
        priceText.gameObject.SetActive(false);

        if (shopManager == null)
            return;

        Toggle[] toggles = shopManager.sideBar.GetComponentsInChildren<Toggle>();

        ShopManager.SidebarItem sidebarItem = ShopManager.SidebarItem.UNSET;
        for(int i = 0; i < toggles.Length; i++) {
            if (toggles[i].isOn) {
                sidebarItem = (ShopManager.SidebarItem) i;
                break;
            }
        }

        toggles = shopManager.topBar.GetComponentsInChildren<Toggle>();
        ShopManager.TopbarItem topbarItem = ShopManager.TopbarItem.UNSET;
        for(int i = 0; i < toggles.Length; i++) {
            if (toggles[i].isOn) {
                topbarItem = (ShopManager.TopbarItem) i;
                break;
            }
        }

        if (topbarItem == hero && sidebarItem == type) {
            gameObject.SetActive(true);
        }

        if (ownedText != null)
            ownedText.gameObject.SetActive(false);
        if (equippedText != null)
            equippedText.gameObject.SetActive(false);

        if (isOwned) {
            if (ownedText == null)
                ownedText = Instantiate(shopManager.ownedText, transform);
            else
                ownedText.gameObject.SetActive(true);
        } else {
            priceText.gameObject.SetActive(true);
        }
        if (isEquipped) {
            if (equippedText == null)
                equippedText = Instantiate(shopManager.equippedText, transform);
            else
                equippedText.gameObject.SetActive(true);
        }

    }

#if UNITY_EDITOR
    public void OnValidate() {
        if (shopManager != null && !shopManager.isInEditMode)
            return;
        if (Application.isPlaying)
            return;
        if (spriteResolver == null)
            return;
        if (spriteLibrary == null)
            return;
        if (image != null)
            image.sprite = GetSprite();
        if (priceText != null)
            priceText.text = cost.ToString();
        label = GetLabel();

        Undo.RecordObject(this, label);
    }
#endif

    public Sprite GetSprite() {
        return spriteLibrary.GetSprite(GetCategory(), GetLabel());
    }

    public string GetCategory() {
        return spriteResolver.GetCategory();
    }

    public string GetLabel() {
        return spriteResolver.GetLabel();
    }


    public void OnPointerClick(PointerEventData eventData) {
        shopManager.ShowItemInfo(this);
    }
}
