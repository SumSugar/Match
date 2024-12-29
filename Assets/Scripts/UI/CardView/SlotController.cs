using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.U2D.Animation;

public class SlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotIndex; // 槽位编号
    public CharacterDataSO characterData; // 当前角色数据
    public Image slotImage; // 卡槽图片
    public Sprite defaultImage; // 默认空槽图片
    public GameObject highlightOverlay; // 高亮层

    private BreakerDataSO equippedCard;

    private void Start()
    {
        //ResetSlot();
    }

    public void EquipCard(BreakerDataSO cardData)
    {
        equippedCard = cardData;
        characterData.SetCardInSlot(slotIndex, cardData);

        if (cardData != null)
        {
            slotImage.sprite = cardData.Icon;
            slotImage.color = Color.white;
        }
        else
        {
            ResetSlot();
        }
    }

    public void ResetSlot()
    {
        equippedCard = null;
        slotImage.sprite = defaultImage;
        slotImage.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DragManager.Instance.IsDraggingCard && highlightOverlay != null)
        {
            highlightOverlay.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightOverlay != null)
        {
            highlightOverlay.SetActive(false);
        }
    }
}

