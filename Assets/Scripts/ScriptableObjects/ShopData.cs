using UnityEngine;

[CreateAssetMenu(fileName = "ShopData_", menuName = "Shop/ShopData", order = 1)]
public class ShopData : ScriptableObject
{
    public ShopItem[] shopItems;
}
