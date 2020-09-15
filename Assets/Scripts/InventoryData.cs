using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<string[]> ownedItems;
    public string[] knightEquippedItem;
    public string[] spearEquippedItem;
    public string[] mageEquippedItem;
    public bool isSpearOwned;
    public bool isMageOwned;
    public MobUpgrade knightMobUpgrade;
    public MobUpgrade mageMobUpgrade;
    public MobUpgrade spearMobUpgrade;
    
    public InventoryData() {
        ownedItems = new List<string[]>();
}

    public InventoryData(InventoryData inventoryData) {
        ownedItems = inventoryData.ownedItems;
        knightEquippedItem = inventoryData.knightEquippedItem;
        spearEquippedItem = inventoryData.spearEquippedItem;
        mageEquippedItem = inventoryData.mageEquippedItem;
        knightMobUpgrade = inventoryData.knightMobUpgrade; 
        mageMobUpgrade = inventoryData.mageMobUpgrade;
        spearMobUpgrade = inventoryData.spearMobUpgrade;

        isSpearOwned = inventoryData.isSpearOwned;
        isMageOwned = inventoryData.isMageOwned;
    }

}
