using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;

    private GameObject draggableInstance; // 当前拖动实例
    private CardController currentCard; // 当前被拖动的卡牌控制器
    private DraggableCardController draggableController; // 当前拖动实例的控制器

    public bool IsDraggingCard => draggableInstance != null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CharacterManager.Instance.SetDefaultCharacter();
    }

    /// <summary>
    /// 开始拖动
    /// </summary>
    public void StartDragging(CardController cardController, GameObject draggablePrefab)
    {
        if (cardController == null || draggablePrefab == null)
        {
            Debug.LogError("Invalid parameters passed to StartDragging.");
            return;
        }

        currentCard = cardController;

        // 创建拖动实例
        draggableInstance = Instantiate(draggablePrefab, transform);
        draggableController = draggableInstance.GetComponent<DraggableCardController>();
        draggableController.Initialize(cardController);

        Debug.Log($"Started dragging: {cardController.cardData.Name}");
    }

    /// <summary>
    /// 拖动过程中更新实例位置
    /// </summary>
    public void UpdateDragging(Vector3 mousePosition)
    {
        if (draggableInstance != null)
        {
            draggableInstance.transform.position = mousePosition;
        }
    }

    /// <summary>
    /// 停止拖动
    /// </summary>
    public void StopDragging()
    {
        if (draggableInstance == null || draggableController == null)
        {
            Debug.LogWarning("StopDragging called with no active drag.");
            return;
        }

        // 检测目标槽位
        var targetSlot = GetHoveredSlot();
        if (targetSlot != null)
        {
            // 装备卡牌到槽位
            targetSlot.EquipCard(draggableController.GetCardData());
            currentCard.libraryPanel.ConfirmRemoveCard(draggableController.GetCardData());
            Debug.Log($"Card equipped to slot {targetSlot.slotIndex}");
        }

        // 销毁拖动实例
        Destroy(draggableInstance);

        // 重置状态
        currentCard = null;
        draggableInstance = null;
        draggableController = null;
    }

    /// <summary>
    /// 获取鼠标悬停的槽位
    /// </summary>
    private SlotController GetHoveredSlot()
    {
        var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<SlotController>(out var slot))
            {
                return slot;
            }
        }

        return null;
    }

}


