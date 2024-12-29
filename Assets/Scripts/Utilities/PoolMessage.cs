using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


[System.Serializable]
public struct MessageBoxPrefabEntry
{
    public int initCount;
    public LogType logType; // 消息类型
    public GameObject prefab; // 对应的预制件
}
//[DefaultExecutionOrder(-100)]
public class PoolMessage : MonoBehaviour
{
    // 存储所有特效预制件的字典，Key 是特效类型
    [SerializeField]
    private List<MessageBoxPrefabEntry> messageBoxPrefabEntry = new List<MessageBoxPrefabEntry>();
    public Dictionary<LogType, ObjectPool<GameObject>> messagePools = new Dictionary<LogType, ObjectPool<GameObject>>();

    public void initialize()
    {
        //初始化对象池
        foreach (MessageBoxPrefabEntry entry in messageBoxPrefabEntry)
        {
            if (!messagePools.ContainsKey(entry.logType))
            {
                ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                    createFunc: () => Instantiate(entry.prefab, transform),
                    actionOnGet: (obj) => obj.SetActive(true),
                    actionOnRelease: (obj) => obj.SetActive(false),
                    actionOnDestroy: (obj) => obj.SetActive(false),
                    collectionCheck: false,
                    defaultCapacity: 10,
                    maxSize: 20
                );
                messagePools.Add(entry.logType, pool);
                //每种特效预加载x个实例
                PreFillPool(entry.logType, entry.initCount);
            }
        }
    }

    public void PreFillPool(LogType logType, int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = messagePools[logType].Get();
        }

        foreach (var obj in preFillArray)
        {
            messagePools[logType].Release(obj);
        }
    }

    public GameObject getObjectFromPool(LogType logType)
    {
        return messagePools[logType].Get();
    }

    public void ReturnObjectToPool(LogType logType, GameObject obj)
    {
        messagePools[logType].Release(obj);
    }
}
