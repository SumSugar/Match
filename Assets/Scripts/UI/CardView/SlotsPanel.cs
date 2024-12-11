using UnityEngine;

public class SlotsPanel : MonoBehaviour
{
    public GameObject slotPrefab; // 装备卡槽预制件
    public Transform slotsContainer; // 卡槽的父容器

    /// <summary>
    /// 初始化装备卡槽
    /// </summary>
    /// <param name="characterData">当前角色数据</param>
    public void OnChangeCharacter(object obj)
    {
        var currentCharacter = CharacterManager.Instance.currentCharacter;
        // 清空旧卡槽
        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }

        // 动态生成新卡槽
        for (int i = 0; i < currentCharacter.MaxSlots; i++)
        {
            var slot = Instantiate(slotPrefab, slotsContainer);
            var slotController = slot.GetComponent<SlotController>();
            slotController.slotIndex = i;
            slotController.characterData = currentCharacter;

            // 设置已装备卡牌
            var equippedCard = currentCharacter.GetCardInSlot(i);
            if (equippedCard != null)
            {
                slotController.EquipCard(equippedCard);
            }
        }
    }
}


