using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DestinyManager : MonoBehaviour
{
    public PoolTool poolTool;

    [Header(header: "卡牌数据")]
    public Vector3 originScale;

    public List<DestinyDataSO> destinyDataList;  //游戏中可能出现的卡牌

    public List<DestinyLibraryEntry> currentDestinyDataList;

    [Header(header:"卡牌库")]
    public DestinyLibrarySO newGameLibrary; //新游戏初始化卡牌库
    //public DestinyLibrarySO currentLibrary; //当前角色开牌库


    public ObjectEventSO assestLoadCompeleteEvnet;

    private int previousIndex;

    private void Awake()
    {

        poolTool = gameObject.GetComponent<PoolTool>();
        poolTool.Initialize(7);

        foreach (var item in newGameLibrary.destinyLibraryList)
        {
            currentDestinyDataList.Add(item);
            
        }
        //InitializeDestinyDataLisit();
    }
    
    private void OnDisable()
    {
        currentDestinyDataList.Clear();
    }


    private void InitializeDestinyDataLisit()
    {
        Addressables.LoadAssetsAsync<DestinyDataSO>("DestinyData", null).Completed += OnDestinyDataLoaded;
    }

    private void OnDestinyDataLoaded(AsyncOperationHandle<IList<DestinyDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            destinyDataList = new List<DestinyDataSO>(handle.Result);
        }
        else
        {
            Debug.Log("No destiny Found!");
        }
        assestLoadCompeleteEvnet.RaiseEvent(null , this);

    }

    /// <summary>
    ///抽卡时调用的函数获得卡牌GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetDestinyObject()
    {

        var destinyObj = poolTool.getObjectFromPool();
        destinyObj.transform.localScale = originScale;
        return destinyObj;
    }

    public void DiscardDestiny(GameObject DestinyObj)
    {
        //poolTool.ReturnObjectToPool(DestinyObj);
    }

    public DestinyDataSO GetNewDestinyData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, destinyDataList.Count);
        }while (previousIndex == randomIndex);

        return destinyDataList[randomIndex];
    }

    /// <summary>
    /// 解锁新卡牌
    /// </summary>
    /// <param name="newDestinyData"></param>
    public void UnlockDestiny(DestinyDataSO newDestinyData)
    {
        var newDestiny = new DestinyLibraryEntry
        {
            destinyData = newDestinyData,
            amount = 1,
        };

        if (currentDestinyDataList.Contains(newDestiny))
        {
            var target = currentDestinyDataList.Find(x => x.destinyData == newDestinyData);
            target.amount++;
        }
        else
        {
            currentDestinyDataList.Add(newDestiny);
        }

    }
}
