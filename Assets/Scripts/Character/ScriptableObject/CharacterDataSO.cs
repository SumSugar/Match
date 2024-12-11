using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    public string characterName;
    public Sprite characterAvatar;
    public int MaxSlots;
    public GameObject characterPrefab;

    [Header("装备卡槽")]
    [SerializeField]
    public List<BreakerDataSO> equippedSlotsDataList;

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
