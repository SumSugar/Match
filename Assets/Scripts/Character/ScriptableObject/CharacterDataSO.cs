using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    public string Name;
    public Sprite avatarSprite;
    public Sprite Icon;
    public int MaxSlots;
    public GameObject prefab;
    public int initialHandSize; 

    [Header("装备卡槽")]
    [SerializeField]
    public List<BreakerDataSO> equippedSlotsDataList;
    public List<DestinyDataSO> equippedDestinyDataList;

    public void Initialize()
    {
        MaxSlots = equippedSlotsDataList.Count;
    }

    public void SetCardInSlot(int slotIndex, BreakerDataSO cardData)
    {
        if (slotIndex >= 0 && slotIndex < equippedSlotsDataList.Count)
        {
            equippedSlotsDataList[slotIndex] = cardData;
        }

    }

    public BreakerDataSO GetCardInSlot(int slotIndex)
    {
        return slotIndex >= 0 && slotIndex < equippedSlotsDataList.Count ? equippedSlotsDataList[slotIndex] : null;
    }
}
