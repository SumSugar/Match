using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class VFXManager : MonoBehaviour
{
    // 单例实例
    public static VFXManager Instance { get; private set; }
    public PoolVFX poolVFX;

    private void Awake()
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("多个 VFXManager 实例存在，销毁重复的实例。");
            Destroy(gameObject); // 销毁重复的实例，确保只有一个实例
            return;
        }

        poolVFX = gameObject.GetComponent<PoolVFX>();
        // 初始化对象池
        poolVFX.Initialize();
    }

    // 根据卡牌类型触发特效
    public void TriggerEffect(VFXType vfxType, Vector3 position)
    {
        if (poolVFX.vfxPools.ContainsKey(vfxType))
        {
            GameObject effect = poolVFX.getObjectFromPool(vfxType);
            if (effect != null)
            {
                Vector3 offset = poolVFX.effectPrefabEntriesDir[vfxType].offset;
                effect.transform.position = position + offset;
                effect.SetActive(true);
                StartCoroutine(DeactivateEffect(effect , vfxType, 2f));  // 假设特效持续2秒
            }
        }
        else
        {
            Debug.LogWarning("Effect type not found for card: " + vfxType);
        }
    }

    // 停用特效并将其返回对象池
    private IEnumerator DeactivateEffect(GameObject effect , VFXType effectType, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.SetActive(false);
        poolVFX.ReturnObjectToPool(effectType,effect);
    }
}
