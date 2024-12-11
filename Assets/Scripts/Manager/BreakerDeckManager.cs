using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BreakerDeckManager : Singleton<BreakerDeckManager>
{
    public List<BreakerDeck> breakerDecks;
    public List<Breaker> handBreakerObjectList;
    public Transform deckOrginTransform;

    [Header(header: "动画参数")]
    public float delay = 0.05f;  // 延迟时间
    public float moveDuration = 1f;  // 移动时间
    public float flipDuration = 0.5f; // 翻转时间

    [Header(header: "事件通知")]
    public ObjectEventSO AddEnemyBreakersToQueueEvent;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public void Initialize()
    {

    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void OnPreparationPhaseStartEvent(object obj)
    {
        foreach (var deck in breakerDecks)
        {
            deck.PreparationPhaseStartDrawCard();
            if (deck.owner.tag == "Ally")
            {
                foreach (var handbreakerData in deck.handBreakerDataList)
                {
                    var breaker = BreakerManager.Instance.GetBreakerObject().GetComponent<Breaker>();
                    //初始化
                    breaker.Init(handbreakerData);
                    breaker.transform.position = deckOrginTransform.position;
                    handBreakerObjectList.Add(breaker);
                }
            }
        }
        SetBreakerLayout();

        AddEnemyBreakersToQueueEvent.RaiseEvent(this, this);
    }

    public void OnPreparationPhaseEndEvent(object obj)
    {
        foreach (var breaker in handBreakerObjectList)
        {
            BreakerManager.Instance.DiscardBreaker(breaker.gameObject);
        }
    }

    private void SetBreakerLayout()
    {
        for (int i = 0; i < handBreakerObjectList.Count; i++)
        {
            Breaker currentBreaker = handBreakerObjectList[i];

            CardTransform breakerTransform = BreakerLayoutManager.Instance.GetBreakerTransform(i, handBreakerObjectList.Count);

            //卡牌能量判断
            currentBreaker.UpdateBreakerState();

            currentBreaker.isAnimating = true;
            currentBreaker.transform.localScale = Vector3.one* 0.5f;

            // 动画序列
            float animationDelay =  i * 0.05f; // 依次延迟
            Sequence cardSequence = DOTween.Sequence();

            // 1. 移动卡牌到目标位置
            cardSequence.Append(
                currentBreaker.transform.DOLocalMove(breakerTransform.pos, 0.3f)
                    .SetEase(Ease.OutCubic)
                    .SetDelay(animationDelay)
            );

            // 2. 翻转第一步：显示背面
            cardSequence.Append(
                currentBreaker.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f)
                    .SetEase(Ease.InCubic)
            );

            // 3. 更换为背面
            cardSequence.AppendCallback(() => currentBreaker.ChangeToFront());

            // 4. 翻转第二步：显示正面
            cardSequence.Append(
                currentBreaker.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f)
                    .SetEase(Ease.OutCubic)
            );

            // 5. 缩放弹性效果（可选）
            cardSequence.Append(
                currentBreaker.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f)
                    .SetEase(Ease.OutElastic)
            );

            // 动画完成
            cardSequence.OnComplete(() =>
            {
                currentBreaker.isAnimating = false;
                currentBreaker.UpdatePositionRotation(breakerTransform.pos, breakerTransform.rotation);
            });

            //设置卡牌顺序
            currentBreaker.GetComponent<SortingGroup>().sortingOrder = i;
            currentBreaker.UpdatePositionRotation(breakerTransform.pos, breakerTransform.rotation);
        }
    }



    public void AddDeck(BreakerDeck deck)
    {
        if (deck != null)
        {
            breakerDecks.Add(deck);
        }
    }

    public void RemoveDeck(BreakerDeck deck)
    {
        if (deck != null)
        {
            breakerDecks.Remove(deck);
        }
    }

}
