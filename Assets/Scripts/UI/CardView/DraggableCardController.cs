using UnityEngine;
using UnityEngine.UI;

public class DraggableCardController : MonoBehaviour
{
    private CardController originalCard;
    [Header(header: "组件")]
    public Image cardImage;
    /// <summary>
    /// 初始化拖动实例
    /// </summary>
    public void Initialize(CardController cardController)
    {
        originalCard = cardController;
        cardImage.sprite = originalCard.cardData.Icon;
        // 禁用射线检测，避免遮挡其他UI
        var canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// 获取原始卡牌数据
    /// </summary>
    public BreakerDataSO GetCardData()
    {
        return originalCard.cardData;
    }

    /// <summary>
    /// 销毁拖动实例
    /// </summary>
    public void DestroyCard()
    {
        Destroy(gameObject);
    }
}




