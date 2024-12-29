using UnityEngine;
using UnityEngine.Pool;

//[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject objectPrefab;
    private ObjectPool<GameObject> pool;

    public void Initialize(int count)
    {
        //初始化对象池
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objectPrefab, transform),
            actionOnGet: (obj) => 
            {
                obj.SetActive(true);
                // 确保新取出的 UI 对象被放置在父级的最后
                obj.transform.SetAsLastSibling();
            },
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => obj.SetActive(false),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        PreFillPool(count);
    }

    private void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] =  pool.Get();
        }

        foreach (var obj in preFillArray)
        {
            pool.Release(obj);
        }
    }

    public GameObject getObjectFromPool()
    {
        return pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
