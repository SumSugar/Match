using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BreakerDeck : MonoBehaviour
{
    public int initBreakers;
    public CharacterBase owner;
    [Header(header: "卡牌库")]
    public Dictionary<BreakerDataSO, int> deck = new Dictionary<BreakerDataSO, int>(); // 使用字典存储卡牌和堆叠数量
    public List<BreakerDataSO> drawDeck = new(); //抽牌堆
    public List<BreakerDataSO> disDeck = new();//弃牌堆

    public List<Breaker> handBreakerList = new();//当前手牌（每回合）

    //public Breaker currentBreaker;
    public bool isHandleEmpty;
    public bool isAllow;

    /// <summary>
    /// 初始化卡组数据。在角色生成完成后调用，将角色已装备的卡牌加入drawDeck并洗牌。
    /// </summary>
    public void Initialize(CharacterBase chara)
    {
        initBreakers = chara.characterData.initialHandSize;
        isHandleEmpty = true;
        isAllow = true;
        owner = chara;
        drawDeck.Clear();
        disDeck.Clear();
        handBreakerList.Clear();

        var characterData = chara.characterData;
        if (characterData.equippedSlotsDataList.Count == 0)
        {
            return;
        }

        foreach (var data in characterData.equippedSlotsDataList)
        {
            drawDeck.Add(data);
        }
        //TODO:洗牌/更新抽牌堆or弃牌堆数字
        ShuffleDrawDeck();
    }

    /// <summary>
    /// 准备阶段开始时抽取初始手牌（initBreakers张）。可在OnPreparationPhaseStartEvent调用。
    /// </summary>
    public void PreparationPhaseStartDrawCard()
    {
        DrawBreaker(initBreakers);
    }


    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void DrawInitialHand()
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
            if (drawDeck.Count == 0)
            {
                RecycleDiscardedBreakers();
            }
            //Debug.Log(owner + ":" + drawDeck.Count);
            BreakerDataSO currentBreakerData = drawDeck[0];
            Breaker breaker = new Breaker();
            breaker.Initialize(currentBreakerData, owner, this);
            drawDeck.RemoveAt(0);
            handBreakerList.Add(breaker);


            if (isHandleEmpty)
            {
                isHandleEmpty = false;
            }
        }

    }
    public void HoldBreaker(Breaker breaker)
    {
        // 逻辑：将卡牌挂起,加入队列
        handBreakerList.Remove(breaker);
    }

    public void ResumeBreaker(Breaker breaker)
    {
        // 逻辑：取消挂起状态
        handBreakerList.Add(breaker);
    }

    /// <summary>
    /// 弃牌：将卡牌从手牌移入弃牌堆。
    /// 在Action Phase的执行结束或卡牌结算结束后调用。
    /// </summary>
    public void DisBreaker(object obj)
    {
        Breaker breaker = obj as Breaker;
        Debug.Log("舍弃："+breaker.data.Name);
        handBreakerList.Remove(breaker);
        disDeck.Add(breaker.data);
    }

    /// <summary>
    /// 将弃牌堆洗入抽牌堆，并清空弃牌堆。
    /// </summary>
    private void RecycleDiscardedBreakers()
    {
        // 将弃牌堆的牌全部加入抽牌堆，并清空弃牌堆
        foreach (var card in disDeck)
        {
            drawDeck.Add(card);
        }

        disDeck.Clear();
        ShuffleDrawDeck();
    }


    /// <summary>
    /// 洗牌算法
    /// </summary>
    private void ShuffleDrawDeck()
    {
        for (int i = 0;i < drawDeck.Count ; i++)
        {
            BreakerDataSO tmp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = tmp;

        }
    }

    /// <summary>
    /// 当角色死亡时，将手牌全部移入弃牌堆。
    /// 可在角色死亡事件中调用。
    /// </summary>
    public void OnDeath()
    {
        foreach (var breaker in handBreakerList)
        {
            disDeck.Add(breaker.data);

        }
        handBreakerList.Clear();
    }

    // Deck相关操作（Add/RemoveCardFromDeck/GetCardAmount）目前与运行逻辑关系不大，
    // 可以保留用于构建基础卡组或在游戏开始时初始化卡组数据。
    public void AddCardToDeck(BreakerDataSO breakerData)
    {
        if (deck.ContainsKey(breakerData))
        {
            deck[breakerData]++;
        }
        else
        {
            deck.Add(breakerData, 1);
        }
    }

    public void RemoveCardFromDeck(BreakerDataSO breakerData)
    {
        if (deck.ContainsKey(breakerData))
        {
            deck[breakerData]--;
            if (deck[breakerData] <= 0)
            {
                deck.Remove(breakerData);
            }
        }
    }
    public int GetCardAmount(BreakerDataSO breakerData)
    {
        return deck.ContainsKey(breakerData) ? deck[breakerData] : 0;
    }
}
