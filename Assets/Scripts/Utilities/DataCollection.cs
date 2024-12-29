using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;

    public int itemBaseValue;
    public int itemRare;
    public int itemPrice;

    [Range(0f, 1f)]
    public float sellPercentage;
}
