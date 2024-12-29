using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class BreakerDeckManager : Singleton<BreakerDeckManager>
{
    public BreakerLayoutManager breakerLayoutManager;

    public List<BreakerDeck> breakerDecks;
    public List<BreakerPresenter> handBreakerObjectList;
    public List<Breaker> playerHandBreakerList;
    public List<Breaker> enemyHandBreakerList;

    public List<Breaker> toBePlayedQueue;
    public Breaker currentQueueBreaker;

    [Header(header: "动画参数")]
    public Transform deckOrginTransform;
    public float delay = 0.05f;  // 延迟时间
    public float moveDuration = 1f;  // 移动时间
    public float flipDuration = 0.5f; // 翻转时间

    [Header(header: "事件通知")]
    public ObjectEventSO AddBreakersToQueueEvent;
    public ObjectEventSO RemoveBreakerFormQueueEvent;

    public ObjectEventSO BreakersQueueCompletedEvent;//执行队列完成,开始战斗（待定）
    protected override void Awake()
    {
        base.Awake();

        breakerLayoutManager = GetComponent<BreakerLayoutManager>();

        Initialize();
    }

    public void Initialize()
    {

    }

    public void LoadPlayerHandBreakers()
    {
        foreach (var deck in breakerDecks)
        {
            if (deck.owner.tag == "Ally")
            {
                foreach (var handbreaker in deck.handBreakerList)
                {
                    //将卡牌暂时从角色手牌添加到统一玩家手牌列表
                    playerHandBreakerList.Add(handbreaker);
                }
            }
            else
            {
                foreach (var handbreaker in deck.handBreakerList)
                {
                    //将卡牌暂时从敌人手牌添加到统一敌人手牌列表
                    enemyHandBreakerList.Add(handbreaker);
                }
            }

            deck.handBreakerList.Clear();
        }
    }

    #region Breaker卡组转移操作，角色手牌->玩家手牌->执行队列
    /// <summary>
    /// 当前执行卡牌->角色弃牌库
    /// </summary>
    public void DiscardQueueBreaker(Breaker breaker)
    {
        breaker.ownerDeck.disDeck.Add(breaker.data);
        toBePlayedQueue.Remove(breaker);
    }
    /// <summary>
    /// 玩家统一手牌->角色弃牌库
    /// </summary>
    /// <param name="breaker"></param>
    public void DiscardPlayerHandBreaker(Breaker breaker)
    {
        breaker.ownerDeck.disDeck.Add(breaker.data);
        playerHandBreakerList.Remove(breaker);
    }

    /// <summary>
    /// 敌人统一手牌->角色弃牌库
    /// </summary>
    /// <param name="breaker"></param>
    public void DiscardEnemyHandBreaker(Breaker breaker)
    {
        breaker.ownerDeck.disDeck.Add(breaker.data);
        enemyHandBreakerList.Remove(breaker);
    }

    /// <summary>
    /// 角色统一手牌->执行队列
    /// </summary>
    /// <param name="breaker"></param>
    public void TransferBreakerFromHandToActionQueue(Breaker breaker)
    {
        toBePlayedQueue.Add(breaker);
        if (breaker.ownerCharacter.tag == "Ally")
        {
            playerHandBreakerList.Remove(breaker);
        }
        else
        {
            enemyHandBreakerList.Remove(breaker);
        }

    }
    #endregion


    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void DisplayPlayerHnadBreakers(object obj)
    {
        foreach (var breaker in playerHandBreakerList)
        {
            var breakerPresenter = BreakerManager.Instance.GetBreakerObject().GetComponent<BreakerPresenter>();
            //初始化
            breakerPresenter.Initialize(breaker);
            breakerPresenter.transform.position = deckOrginTransform.position;
            //卡牌实体
            handBreakerObjectList.Add(breakerPresenter);
            //卡牌数据

        }

        //TODO:敌人卡牌逻辑

        SetBreakerLayout();
    }

    /// <summary>
    /// 按下准备阶段结束的按钮
    /// </summary>
    /// <param name="obj"></param>
    public void OnPreparationPhaseEndButtonClickEvent(object obj)
    {
        AddPlayerBreakersToQueue();

        AddEnemyBreakersToQueue(3);

        BreakersQueueCompletedEvent.RaiseEvent(this, this);
    }


    private void AddPlayerBreakersToQueue()
    {
        foreach (var breakerPresenter in handBreakerObjectList)
        {
            Breaker breaker = breakerPresenter.breaker;
            if (breakerPresenter.breakerState == BreakerState.Selected)
            {
                //选中手牌移动到执行队列
                TransferBreakerFromHandToActionQueue(breaker);
                //触发事件通知
                //AddBreakersToQueueEvent.RaiseEvent(breaker, this);
            }
            else
            {
                //未选中手牌移动到角色弃牌库
                DiscardPlayerHandBreaker(breaker);
            }

            BreakerManager.Instance.DiscardBreakerObject(breakerPresenter.gameObject);
        }
        //清空手牌，手牌实体()
        playerHandBreakerList.Clear();
        handBreakerObjectList.Clear();

        toBePlayedQueue.Sort((a, b) => b.ownerCharacter.speed.CompareTo(a.ownerCharacter.speed));
        ////触发事件通知
        //AddBreakersToQueueEvent.RaiseEvent(this, this);
    }

    private void AddEnemyBreakersToQueue(int numberToSelect)
    {
        // 检查要选的牌数是否合理
        if (numberToSelect > enemyHandBreakerList.Count)
        {
            // 如果请求的数量超过现有手牌数量，则只选择所有手牌
            numberToSelect = enemyHandBreakerList.Count; 
        }

        //随机选择要加入执行队列的牌
        List<Breaker> tmpBreakerList = new List<Breaker>();
        for (int i = 0; i < numberToSelect; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, numberToSelect);
            var breaker = enemyHandBreakerList[randomIndex];
            tmpBreakerList.Add(breaker);
        }
        foreach (var breaker in tmpBreakerList)
        {
            TransferBreakerFromHandToActionQueue(breaker);
            //toBePlayedQueue.Sort((a, b) => b.ownerCharacter.speed.CompareTo(a.ownerCharacter.speed));
            // 触发事件通知
            //AddBreakersToQueueEvent.RaiseEvent(breaker, this);
        }

        // 处理未选中的牌（加入弃牌库）
        List<Breaker> unselectBreakers = new List<Breaker>();
        unselectBreakers.AddRange(enemyHandBreakerList);
        foreach (var breaker in unselectBreakers)
        {
            DiscardEnemyHandBreaker(breaker);
            enemyHandBreakerList.Remove(breaker);
        }
        enemyHandBreakerList.Clear();

        toBePlayedQueue.Sort((a, b) => b.ownerCharacter.speed.CompareTo(a.ownerCharacter.speed));
        //// 触发事件通知
        //AddBreakersToQueueEvent.RaiseEvent(this, this);
    }


    private void SetBreakerLayout()
    {
        for (int i = 0; i < handBreakerObjectList.Count; i++)
        {
            BreakerPresenter currentBreaker = handBreakerObjectList[i];

            CardTransform breakerTransform = breakerLayoutManager.GetBreakerTransform(i, handBreakerObjectList.Count);

            //卡牌能量判断
            currentBreaker.UpdateBreakerState();

            currentBreaker.isAnimating = true;
            currentBreaker.transform.localScale = Vector3.one* 0.4f;

            // 动画序列
            float animationDelay =  i * 0.05f; // 依次延迟
            Sequence cardSequence = DOTween.Sequence();
            currentBreaker.ChangeToBack();
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
                currentBreaker.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.2f)
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

    /// <summary>
    /// 从队列移除卡牌，发送事件给UI
    /// </summary>
    /// <param name="breaker"></param>
    public void RemoveBreakerFromBePlayedQueue(Breaker breaker)
    {
        //TODO:移除卡牌
        //TODO:发送移除卡牌事件
        DiscardQueueBreaker(breaker);
        RemoveBreakerFormQueueEvent.RaiseEvent(breaker, this);
    }

    /// <summary>
    /// 从队列移除移除死亡角色的卡牌
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveDeadCharacterBreakers(object obj)
    {
        CharacterBase character = obj as CharacterBase;

        List<Breaker> breakers = toBePlayedQueue.Where(breaker => breaker.ownerCharacter == character).ToList();
        foreach (var breaker in breakers)
        {
            RemoveBreakerFromBePlayedQueue(breaker);
        }
    }

    /// <summary>
    /// 移除队列所有卡牌
    /// </summary>
    public void RemoveBreakerBattleEnd()
    {
        for (int i = toBePlayedQueue.Count - 1; i >= 0; i--)
        {
            var breaker = toBePlayedQueue[i];
            RemoveBreakerFromBePlayedQueue(breaker);
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

    public Breaker GetCurrentBreaker()
    {
        if (toBePlayedQueue.Count > 0)
        {
            currentQueueBreaker = toBePlayedQueue[0];
            return currentQueueBreaker;
        }
        return null;
    }

}
