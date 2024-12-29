using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BreakerManager : Singleton<BreakerManager>
{
    public PoolTool poolTool;
    [Header(header: "卡牌数据")]
    public Vector3 originScale;

    public List<BreakerDataSO> breakerDataList;  //游戏中可能出现的卡牌

    public List<BreakerLibraryEntry> currentBreakerDataList;

    [Header(header:"卡牌库")]
    public BreakerLibrarySO newGameLibrary; //新游戏初始化卡牌库
    //public BreakerLibrarySO currentLibrary; //当前角色开牌库


    public ObjectEventSO assestLoadCompeleteEvnet;

    private int previousIndex;

    protected override void Awake()
    {
        base.Awake();
        poolTool = gameObject.GetComponent<PoolTool>();
        poolTool.Initialize(7);

        foreach (var item in newGameLibrary.breakerLibraryList)
        {
            currentBreakerDataList.Add(item);
            
        }
        //InitializeBreakerDataLisit();
    }
    
    private void OnDisable()
    {
        currentBreakerDataList.Clear();
    }


    private void InitializeBreakerDataLisit()
    {
        Addressables.LoadAssetsAsync<BreakerDataSO>("BreakerData", null).Completed += OnBreakerDataLoaded;
    }

    private void OnBreakerDataLoaded(AsyncOperationHandle<IList<BreakerDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            breakerDataList = new List<BreakerDataSO>(handle.Result);
        }
        else
        {
            Debug.Log("No breaker Found!");
        }
        assestLoadCompeleteEvnet.RaiseEvent(null , this);

    }

    /// <summary>
    ///抽卡时调用的函数获得卡牌GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetBreakerObject()
    {
        var breakerObj = poolTool.getObjectFromPool();
        breakerObj.transform.localScale = originScale;
        return breakerObj;
    }

    public void DiscardBreakerObject(GameObject BreakerObj)
    {
        poolTool.ReturnObjectToPool(BreakerObj);
    }

    public BreakerDataSO GetNewBreakerData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, breakerDataList.Count);
        }while (previousIndex == randomIndex);

        return breakerDataList[randomIndex];
    }

    /// <summary>
    /// 解锁新卡牌
    /// </summary>
    /// <param name="newBreakerData"></param>
    public void UnlockBreaker(BreakerDataSO newBreakerData)
    {
        var newBreaker = new BreakerLibraryEntry
        {
            breakerData = newBreakerData,
            amount = 1,
        };

        if (currentBreakerDataList.Contains(newBreaker))
        {
            var target = currentBreakerDataList.Find(x => x.breakerData == newBreakerData);
            target.amount++;
        }
        else
        {
            currentBreakerDataList.Add(newBreaker);
        }

    }
}
