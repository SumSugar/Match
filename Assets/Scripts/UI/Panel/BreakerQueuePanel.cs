
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CartoonFX.ExpressionParser;

public class BreakerQueuePanel : UIPanel
{
    public Transform queueElementsContainer;

    public List<BreakerQueueView> breakerQueueViewList = new List<BreakerQueueView>();
    public BreakerQueueView currentBreakerQueueView = null;
    public PoolTool poolTool;
    private Dictionary<BreakerQueueView, TransformPropertyChange> changes = new Dictionary<BreakerQueueView, TransformPropertyChange>();

    [Header("Animation Settings")]
    public float animationDuration = 0.5f; // 动画时长
    public float animationOffset = 200f; // 动画偏移量（X轴）
    public float spacing = 10f; // 元素间距
    public void Initialize()
    {
        poolTool = GetComponent<PoolTool>();
        poolTool.Initialize(10);
    }

    public void AddBreakerToQueue(object obj)
    {
        Breaker breaker = obj as Breaker;
        var breakerView = CreateBreakerQueueViewElement(breaker);

        SortAndRefreshLayout();

        // 播放入场动画
        var childRectTransform = breakerView.continer.GetComponent<RectTransform>();
        var canvasGroup = breakerView.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.3f;
        canvasGroup.DOFade(1, animationDuration);

        childRectTransform.anchoredPosition = new Vector2(-animationOffset, 0);
        childRectTransform.DOAnchorPos(Vector2.zero, animationDuration);

    }

    public void RemoveBreakerFromQueue(object obj)
    {
        Breaker breaker = obj as Breaker;

        BreakerQueueView breakerView = breakerQueueViewList.Find(bqv => bqv.breaker == breaker);

        if (breakerView != null)
        {
            breakerQueueViewList.Remove(breakerView);

            // 播放退场动画
            var childRectTransform = breakerView.continer.GetComponent<RectTransform>();
            var canvasGroup = breakerView.GetComponent<CanvasGroup>();

            childRectTransform.localScale = Vector2.one;
            childRectTransform.DOScale(Vector3.one*1.3f , animationDuration).SetEase(Ease.OutCirc);
            canvasGroup.DOFade(0, animationDuration).SetEase(Ease.OutCirc); 
            childRectTransform.DOAnchorPos(new Vector2(-animationOffset, childRectTransform.anchoredPosition.y), animationDuration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    DiscardBreakerQueueViewObject(breakerView.gameObject);
                    // 更新并触发布局动画
                    SortAndRefreshLayout();
                });
        }
    }

    public BreakerQueueView CreateBreakerQueueViewElement(Breaker breaker)
    {
        GameObject QueueElementInstance = GetBreakerQueueViewObject();
        var breakerQueueView = QueueElementInstance.GetComponent<BreakerQueueView>();
        breakerQueueView.SetBreakerData(breaker);
        breakerQueueViewList.Add(breakerQueueView);
        return breakerQueueView;
    }



    public void ActionStartSetUpQueue()
    {
        // 清空队列显示
        foreach (var breakerQueueView in breakerQueueViewList)
        {
            DiscardBreakerQueueViewObject(breakerQueueView.gameObject);
        }
        breakerQueueViewList.Clear();

        // 重新生成队列显示
        foreach (var breaker in BreakerDeckManager.Instance.toBePlayedQueue)
        {
            CreateBreakerQueueViewElement(breaker);
        }

        SortAndRefreshLayout();


        float delay = 0.1f;

        foreach (var breakerView in breakerQueueViewList)
        {
            delay += 0.1f;
            //canvasGroup.alpha = 0f;
            //canvasGroup.DOFade(1, animationDuration);

            // 播放入场动画
            var childRectTransform = breakerView.continer.GetComponent<RectTransform>();
            var canvasGroup = breakerView.GetComponent<CanvasGroup>();

            childRectTransform.localScale = Vector2.one;
            canvasGroup.alpha = 0f;

            canvasGroup.DOFade(1, animationDuration).SetEase(Ease.OutCirc).SetDelay(delay);

            //childRectTransform.DOScale(Vector3.one, animationDuration).SetEase(Ease.InCirc).SetDelay(delay);

            childRectTransform.anchoredPosition = new Vector2(-animationOffset, 0);
            childRectTransform.DOAnchorPos(Vector2.zero, animationDuration).SetEase(Ease.OutQuad).SetDelay(delay);
        }
    }

    public GameObject GetBreakerQueueViewObject()
    {
        var breakerObj = poolTool.getObjectFromPool();
        breakerObj.transform.SetParent(transform);
        breakerObj.GetComponent<RectTransform>().localScale = Vector3.one;
        breakerObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        breakerObj.GetComponent<CanvasGroup>().alpha = 1f;
        return breakerObj;
    }

    public void DiscardBreakerQueueViewObject(GameObject BreakerObj)
    {
        poolTool.ReturnObjectToPool(BreakerObj);
    }

    ///// <summary>
    ///// 更新布局并为子元素添加动画
    ///// </summary>
    //private void UpdateAndAnimateLayout()
    //{
        //Sequence updateLayoutSequence = DOTween.Sequence();
        // 更新变化数据
        //SortAndRefreshLayout();
        //VerticalLayoutGroup layoutGroup = GetComponent<VerticalLayoutGroup>();
        //// 遍历所有子元素
        //foreach (var kvp in changes)
        //{
        //    var breakerView = kvp.Key;
        //    var change = kvp.Value;

        //    RectTransform rectTransform = breakerView.GetComponent<RectTransform>();
        //    // 设置回旧状态
        //    rectTransform.anchoredPosition = change.OldPosition;

        //    // 动画移动到新位置
        //    rectTransform.DOAnchorPos(change.NewPosition, animationDuration).SetEase(Ease.OutCubic).OnComplete(() =>
        //    {
        //        //Debug.Log("Animation completed.");
        //    }); 
        //}

   // }

    private void SortAndRefreshLayout()
    {
        breakerQueueViewList.Sort((a, b) => b.breaker.ownerCharacter.speed.CompareTo(a.breaker.ownerCharacter.speed));
        foreach (var breakerQueueView in breakerQueueViewList)
        {
            breakerQueueView.transform.SetAsLastSibling();
        }

        //changes.Clear();

        //// 保存强制刷新前的旧状态
        //foreach (var breakerView in breakerQueueViewList)
        //{
        //    var rectTransform = breakerView.GetComponent<RectTransform>();
        //    var canvasGroup = breakerView.GetComponent<CanvasGroup>() ?? breakerView.gameObject.AddComponent<CanvasGroup>();

        //    // 记录当前旧状态
        //    changes[breakerView] = new TransformPropertyChange
        //    {
        //        OldPosition = rectTransform.anchoredPosition,
        //        OldScale = rectTransform.localScale,
        //        OldAlpha = canvasGroup.alpha
        //    };
        //    Debug.Log("New Position: " + changes[breakerView].OldPosition);
        //}

        //// 强制刷新布局，更新所有元素的新状态
        //LayoutRebuilder.ForceRebuildLayoutImmediate(queueElementsContainer.GetComponent<RectTransform>());

        //// 记录强制刷新后的新状态
        //foreach (var breakerView in breakerQueueViewList)
        //{
        //    var rectTransform = breakerView.GetComponent<RectTransform>();
        //    var canvasGroup = breakerView.GetComponent<CanvasGroup>() ?? breakerView.gameObject.AddComponent<CanvasGroup>();

        //    // 更新变化数据的新状态
        //    if (changes.TryGetValue(breakerView, out var change))
        //    {
        //        change.NewPosition = rectTransform.anchoredPosition;
        //        change.NewScale = rectTransform.localScale;
        //        change.NewAlpha = canvasGroup.alpha;
        //        Debug.Log("New Position: " + change.NewPosition);
        //    }
        //}
    }
}
