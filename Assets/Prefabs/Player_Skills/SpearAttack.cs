using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class SpearAttack : SpawnedSkill
{
    // Start is called before the first frame update
    public override void OnSkillStart()
    {
        base.OnSkillStart();

        SpriteResolver resolver = GetComponent<SpriteResolver>();
        
        if (resolver == null) {
            return;
        }
        InventoryData inventoryData = SaveSystem.LoadInventory();
        if (inventoryData.spearEquippedItem != null) {
            resolver.SetCategoryAndLabel(inventoryData.spearEquippedItem[0], inventoryData.spearEquippedItem[1]);
        }
    }

}
