using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DestinyDeck : MonoBehaviour
{
    public DestinyManager destinyManager;

    public DestinyLayoutManager destinyLayoutManager;

    public Vector3 deckPosition;

    public List<DestinyDataSO> drawDeck = new(); //抽牌堆

    public int initDestinys;

    private List<DestinyDataSO> disDeck = new();//弃牌堆

    private List<Destiny> handDestinyObjectList = new();//当前手牌（每回合）

    [Header(header: "事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO disDestinyCountEvent;

    private void Awake()
    {
        destinyManager = gameObject.GetComponent<DestinyManager>();
        destinyLayoutManager = gameObject.GetComponent<DestinyLayoutManager>();
    }

    private void OnEnable()
    {
        Debug.Log("DestinyDeck OnEnable");
    }

    private void Start()
    {
        //InitializeDeck();
        Debug.Log("DestinyDeck OnStart");
    }

    public void Initialize()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        if (destinyManager.currentDestinyDataList.Count == 0)
        {
            return;
        }
        drawDeck.Clear();
        foreach (var entry in destinyManager.currentDestinyDataList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.destinyData);
            }
        }


        //TODO:洗牌/更新抽牌堆or弃牌堆数字
        ShuffleDeck();
    }


    [ContextMenu("测试抽牌")]
    public void TestDrawDestiny()
    {
        DrawDestiny(1);
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void OnPreparationPhaseStartEvent(object obj)
    {
        //DrawDestiny(initDestinys);
    }
    public void OnBattleStartEvent(object obj)
    {
        DrawDestiny(initDestinys);
    }

    public void DrawDestiny(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DestinyDataSO currentDestinyData = drawDeck[0];
            drawDeck.RemoveAt(0);
            if(drawDeck.Count == 0)
            {
                //TODO:洗牌/更新抽牌堆or弃牌堆数字
                foreach (var item in disDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            //更新UI数字
            //drawCountEvent.RaiseEvent(drawDeck.Count, this);

            var destiny = destinyManager.GetDestinyObject().GetComponent<Destiny>();
            //初始化
            destiny.Init(currentDestinyData);
            destiny.transform.localPosition = deckPosition; 

            handDestinyObjectList.Add(destiny);
            var delay = i * 0.2f;
            SetDestinyLayout(delay);
        }

    }

    private void SetDestinyLayout(float delay)
    {
        for (int i = 0; i < handDestinyObjectList.Count ; i++)
        {
            Destiny currentDestiny = handDestinyObjectList[i];

            CardTransform destinyTransform = destinyLayoutManager.GetDestinyTransform(i, handDestinyObjectList.Count);

            //currentDestiny.transform.SetPositionAndRotation(destinyTransform.pos,destinyTransform.rotation);

            //卡牌能量判断
            currentDestiny.UpdateDestinyState();

            currentDestiny.isAnimating = true;

            //currentDestiny.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).SetEase(Ease.OutElastic).SetDelay(delay).onComplete = () =>
            //{
            //    currentDestiny.transform.DOLocalMove(destinyTransform.pos, 0.3f).onComplete = () => currentDestiny.isAnimating = false;
            //    //currentDestiny.transform.DOLocalRotateQuaternion(destinyTransform.rotation, 0.3f);
            //};

            currentDestiny.transform.DOLocalMove(destinyTransform.pos, 0.3f).onComplete = () =>
            {
                currentDestiny.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 1.0f).SetEase(Ease.OutElastic).SetDelay(delay).onComplete = () => currentDestiny.isAnimating = false;
                //currentDestiny.transform.DOLocalRotateQuaternion(destinyTransform.rotation, 0.3f);
            };


            //currentDestiny.transform.SetLocalPositionAndRotation(destinyTransform.pos , destinyTransform.rotation);

            //设置卡牌顺序
            currentDestiny.GetComponent<SortingGroup>().sortingOrder = i;
            currentDestiny.UpdatePositionRotation(destinyTransform.pos, destinyTransform.rotation);
        }
    }

    /// <summary>
    /// 弃牌逻辑，事件函数
    /// </summary>
    /// <param name="destiny"></param>
    public void DisDestiny(object obj)
    {
        Destiny destiny = obj as Destiny;
        disDeck.Add(destiny.destinyData);
        handDestinyObjectList.Remove(destiny);

        destinyManager.DiscardDestiny(destiny.gameObject);
        disDestinyCountEvent.RaiseEvent(disDeck.Count, this);
        SetDestinyLayout(0f);
    }
    public void DisDestiny(Destiny destiny)
    {
        disDeck.Add(destiny.destinyData);
        handDestinyObjectList.Remove(destiny);

        destinyManager.DiscardDestiny(destiny.gameObject);
        disDestinyCountEvent.RaiseEvent(disDeck.Count, this);
        SetDestinyLayout(0f);
    }
    private void ShuffleDeck()
    {
        disDeck.Clear();
        //todu：更新UI显示数量
        drawCountEvent.RaiseEvent(drawDeck.Count,this);
        disDestinyCountEvent.RaiseEvent(disDeck.Count, this);

        for (int i = 0;i < drawDeck.Count ; i++)
        {
            DestinyDataSO tmp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = tmp;

        }
    }

    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handDestinyObjectList.Count ; i++)
        {
            disDeck.Add(handDestinyObjectList[i].destinyData);
            destinyManager.DiscardDestiny(handDestinyObjectList[i].gameObject);
        }

        handDestinyObjectList.Clear();
        disDestinyCountEvent.RaiseEvent(disDeck.Count, this);
    }

    public void ReleaseAllDestinys(object obj)
    {
        foreach (var destiny in handDestinyObjectList)
        {
            destinyManager.DiscardDestiny(destiny.gameObject);
        }

        handDestinyObjectList.Clear();
        InitializeDeck();
    }

    public void OnHit()
    {
        Destiny destiny = handDestinyObjectList[0];
        DisDestiny(destiny);
    }

    public bool IsDestinyEmpty()
    {
        if (handDestinyObjectList.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
