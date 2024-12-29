using UnityEngine;
using UnityEngine.UI;

public class OverlappingCardsManager : MonoBehaviour
{
    public RectTransform cardContainer; // 卡牌容器
    public HorizontalLayoutGroup layoutGroup; // 布局组
    public int cardCount = 5; // 卡牌数量
    public float maxCardWidth = 150f; // 卡牌最大宽度
    public float overlapPercentage = 0.3f; // 卡牌重叠百分比 (0-1)
    public float aspectRatio = 1.5f; // 卡牌宽高比

    void Start()
    {
        UpdateCardLayout();
    }

    void UpdateCardLayout()
    {
        // 容器宽度
        float containerWidth = cardContainer.rect.width;

        // 卡牌重叠的宽度
        float overlapWidth = containerWidth / cardCount * overlapPercentage;

        // 每张卡牌的宽度（考虑重叠）
        float cardWidth = Mathf.Min((containerWidth + overlapWidth * (cardCount - 1)) / cardCount, maxCardWidth);

        // 设置布局组的间距为负值，实现重叠效果
        layoutGroup.spacing = -overlapWidth;

        // 更新每张卡牌的尺寸
        foreach (Transform child in cardContainer)
        {
            var rectTransform = child.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(cardWidth, cardWidth * aspectRatio);
        }
    }
}