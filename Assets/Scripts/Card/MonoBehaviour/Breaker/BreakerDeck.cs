using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BreakerDeck : MonoBehaviour
{
    public int initBreakers;
    public CharacterBase owner;
    [Header(header: "卡牌库")]
    public List<BreakerDataSO> breakers;
    public Dictionary<BreakerDataSO, int> deck = new Dictionary<BreakerDataSO, int>(); // 使用字典存储卡牌和堆叠数量
    public List<BreakerDataSO> drawDeck = new(); //抽牌堆
    private List<BreakerDataSO> disDeck = new();//弃牌堆

    public List<BreakerDataSO> handBreakerDataList = new();//当前手牌（每回合）

    public Breaker currentBreaker;
    public bool isHandleEmpty;
    public bool isAllow;

    private void Awake()
    {
        isHandleEmpty = true;
        isAllow = true;
    }

    public void Initialize(CharacterBase chara)
    {
        owner = chara;
        var characterData = chara.characterData;
        if (characterData.equippedSlotsDataList.Count == 0)
        {
            return;
        }
        drawDeck.Clear();
        foreach (var data in characterData.equippedSlotsDataList)
        {
            drawDeck.Add(data);
        }
        //TODO:洗牌/更新抽牌堆or弃牌堆数字
        ShuffleDeck();
    }

    public void PreparationPhaseStartDrawCard()
    {
        DrawBreaker(initBreakers);
    }

    // 向牌库中添加卡牌
    public void AddCardToDeck(BreakerDataSO breakerData)
    {
        if (deck.ContainsKey(breakerData))
        {
            deck[breakerData]++; // 增加堆叠数量
        }
        else
        {
            deck.Add(breakerData, 1); // 新卡牌添加并设置堆叠为1
        }
    }

    // 从牌库中移除卡牌
    public void RemoveCardFromDeck(BreakerDataSO breakerData)
    {
        if (deck.ContainsKey(breakerData))
        {
            if (deck[breakerData] > 1)
            {
                deck[breakerData]--; // 减少堆叠数量
            }
            else
            {
                deck.Remove(breakerData); // 移除卡牌
            }
        }
    }

    // 获取卡牌数量
    public int GetCardAmount(BreakerDataSO breakerData)
    {
        return deck.ContainsKey(breakerData) ? deck[breakerData] : 0;
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void OnPreparationPhaseStartEvent(object obj)
    {
        if (isAllow)
        {
            DrawBreaker(initBreakers);
        }
    }

    public void DrawBreaker(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(drawDeck.Count == 0)
            {
                //TODO:洗牌/更新抽牌堆or弃牌堆数字
                foreach (var item in disDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }
            BreakerDataSO currentBreakerData = drawDeck[0];
            drawDeck.RemoveAt(0);

            //更新UI数字
            //drawCountEvent.RaiseEvent(drawDeck.Count, this);

            //var breaker = breakerManager.GetBreakerObject().GetComponent<Breaker>();
            ////初始化
            //breaker.Init(currentBreakerData);
            //breaker.transform.localPosition = deckPosition;

            handBreakerDataList.Add(currentBreakerData);
            //设置当前卡牌
            //SelectCurrentCard();
            //currentBreaker = breaker;
            //currentBreaker.cardViewController.SwitchCardState(CardState.Inactive);

            if (isHandleEmpty)
            {
                isHandleEmpty = false;
            }
            //var delay = i * 0.2f;
            //SetBreakerLayout(delay);
        }

    }

    //private void SetBreakerLayout(float delay)
    //{
    //    for (int i = 0; i < handBreakerObjectList.Count ; i++)
    //    {
    //        Breaker currentBreaker = handBreakerObjectList[i];

    //        CardTransform breakerTransform = breakerLayoutManager.GetBreakerTransform(i, handBreakerObjectList.Count);

    //        //currentBreaker.transform.SetPositionAndRotation(breakerTransform.pos,breakerTransform.rotation);

    //        //卡牌能量判断
    //        currentBreaker.UpdateBreakerState();

    //        currentBreaker.isAnimating = true;

    //        //currentBreaker.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 1.5f).SetEase(Ease.OutElastic).SetDelay(delay).onComplete = () => currentBreaker.isAnimating = false;

    //        currentBreaker.transform.DOLocalMove(breakerTransform.pos, 0.2f).onComplete = () =>
    //        {
    //            currentBreaker.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 1.0f).SetEase(Ease.OutElastic).SetDelay(delay).onComplete = () => currentBreaker.isAnimating = false;
    //            //currentDestiny.transform.DOLocalRotateQuaternion(destinyTransform.rotation, 0.3f);
    //        };

    //        //currentBreaker.transform.SetLocalPositionAndRotation(breakerTransform.pos , breakerTransform.rotation);

    //        //设置卡牌顺序
    //        currentBreaker.GetComponent<SortingGroup>().sortingOrder = i;
    //        currentBreaker.UpdatePositionRotation(breakerTransform.pos, breakerTransform.rotation);
    //    }
    //}

    /// <summary>
    /// 弃牌逻辑，事件函数
    /// </summary>
    /// <param name="breaker"></param>
    public void DisBreaker(object obj)
    {
        BreakerDataSO data = obj as BreakerDataSO;
        disDeck.Add(data);
        handBreakerDataList.Remove(data);
        //breakerManager.DiscardBreaker(breaker.gameObject);
        //disBreakerCountEvent.RaiseEvent(disDeck.Count, this);
        //SetBreakerLayout(0f);
    }
    //public void DisBreaker(Breaker breaker)
    //{
    //    disDeck.Add(breaker.breakerData);
    //    handBreakerObjectList.Remove(breaker);

    //    breakerManager.DiscardBreaker(breaker.gameObject);
    //    //disBreakerCountEvent.RaiseEvent(disDeck.Count, this);
    //    SetBreakerLayout(0);

    //    //设置当前卡牌
    //    if (handBreakerObjectList.Count > 0)
    //    {
    //        //currentBreaker.cardViewController.SwitchCardState(CardState.InHand);
    //        //currentBreaker = handBreakerObjectList[handBreakerObjectList.Count - 1];
    //        //currentBreaker.cardViewController.SwitchCardState(CardState.Inactive);
    //        SelectCurrentCard();
    //    }
    //    else
    //    {
    //        Debug.Log("empty");
    //        isHandleEmpty = true;
    //        //if (isAutoDraw)
    //        //{
    //        //    DrawBreaker(initBreakers);
    //        //}
    //    }
       
    //}

    //public void SelectCurrentCard()
    //{
    //    if (!isHandleEmpty )
    //    {
    //        currentBreaker.cardViewController.SwitchCardState(CardState.InHand);
    //    }
    //    currentBreaker = handBreakerObjectList[handBreakerObjectList.Count - 1];
    //    currentBreaker.cardViewController.SwitchCardState(CardState.Inactive);
    //}

    private void ShuffleDeck()
    {
        disDeck.Clear();
        //todu：更新UI显示数量
        //drawCountEvent.RaiseEvent(drawDeck.Count,this);
        //disBreakerCountEvent.RaiseEvent(disDeck.Count, this);

        for (int i = 0;i < drawDeck.Count ; i++)
        {
            BreakerDataSO tmp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = tmp;

        }
    }

    public void OnDeath()
    {
        for (int i = 0; i < handBreakerDataList.Count ; i++)
        {
            disDeck.Add(handBreakerDataList[i]);
            //breakerManager.DiscardBreaker(handBreakerObjectList[i].gameObject);
        }

        handBreakerDataList.Clear();
        //disBreakerCountEvent.RaiseEvent(disDeck.Count, this);
    }

    //public void ReleaseAllBreakers(object obj)
    //{
    //    foreach (var breaker in handBreakerObjectList)
    //    {
    //        breakerManager.DiscardBreaker(breaker.gameObject);
    //    }

    //    handBreakerObjectList.Clear();
    //    InitializeDeck();
    //}

    //public bool IsBreakerEmpty()
    //{
    //    if (handBreakerObjectList.Count == 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
