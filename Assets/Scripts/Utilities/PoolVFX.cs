using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public struct VFXPrefabEntry
{
    public int initCount;
    public VFXType vfxType; // 特效类型
    public GameObject prefab; // 对应的预制件
    public Vector3 offset; // 特效位置偏移
}

//[DefaultExecutionOrder(-100)]
public class PoolVFX : MonoBehaviour
{
    // 存储所有特效预制件的字典，Key 是特效类型
    [SerializeField]
    public List<VFXPrefabEntry> effectPrefabEntries = new List<VFXPrefabEntry>();

    public Dictionary<VFXType, VFXPrefabEntry> effectPrefabEntriesDir = new Dictionary<VFXType, VFXPrefabEntry>();
    public Dictionary<VFXType, ObjectPool<GameObject>> vfxPools = new Dictionary<VFXType, ObjectPool<GameObject>>();

    public void Initialize()
    {
        // 初始化对象池
        foreach (VFXPrefabEntry entry in effectPrefabEntries)
        {
            if (!vfxPools.ContainsKey(entry.vfxType))
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
                vfxPools.Add(entry.vfxType, pool);
                // 每种特效预加载 x 个实例
                PreFillPool(entry.vfxType, entry.initCount);

                effectPrefabEntriesDir.Add(entry.vfxType, entry);
            }
        }
    }

    public void PreFillPool(VFXType effectType,int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = vfxPools[effectType].Get();
        }

        foreach (var obj in preFillArray)
        {
            vfxPools[effectType].Release(obj);
        }
    }

    public GameObject getObjectFromPool(VFXType effectType)
    {
        return vfxPools[effectType].Get();
    }

    public void ReturnObjectToPool(VFXType effectType, GameObject obj)
    {
        vfxPools[effectType].Release(obj);
    }
}
