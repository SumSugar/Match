using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;

    public List<CardDataSO> cardDataList;  //游戏中可能出现的卡牌

    [Header(header:"卡牌库")]
    public CardLibrarySO newGameLibrary; //新游戏初始化卡牌库
    public CardLibrarySO currentLibrary; //当前玩家开牌库

    private int previousIndex;

    private void Awake()
    {
        InitializeCardDataLisit();

        foreach (var item in newGameLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();   
    }


    private void InitializeCardDataLisit()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.Log("No Card Found!");
        }


    }

    /// <summary>
    ///抽卡时调用的函数获得卡牌GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {

        var cardObj = poolTool.getObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReturnObjectToPool(cardObj);
    }

    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardDataList.Count);
        }while (previousIndex == randomIndex);

        return cardDataList[randomIndex];
    }

    /// <summary>
    /// 解锁新卡牌
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnlockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1,
        };

        if (currentLibrary.cardLibraryList.Contains(newCard))
        {
            var target = currentLibrary.cardLibraryList.Find(x => x.cardData == newCardData);
            target.amount++;
        }
        else
        {
            currentLibrary.cardLibraryList.Add(newCard);
        }

    }
}
