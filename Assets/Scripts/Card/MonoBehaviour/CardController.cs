using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BreakerDataSO cardData; // 当前卡牌数据
    public GlobalLibraryPanel libraryPanel; // 所属的全局牌库
    private CanvasGroup canvasGroup;

    public void Initialize(BreakerDataSO data, GlobalLibraryPanel library)
    {
        cardData = data;
        libraryPanel = library;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 拖动开始
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // 禁用射线检测
        DragManager.Instance.StartDragging(this, libraryPanel.draggableCardPrefab); // 通知 DragManager 开始拖动
    }

    /// <summary>
    /// 拖动中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.UpdateDragging(Input.mousePosition); // 更新拖动实例位置
    }

    /// <summary>
    /// 拖动结束
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // 恢复射线检测
        DragManager.Instance.StopDragging(); // 通知 DragManager 停止拖动
    }
}









