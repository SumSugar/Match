using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewController : MonoBehaviour
{
    //[System.Serializable]
    //public struct CardVFXMapping
    //{
    //    public CardState state;
    //    public GameObject vfxObject;
    //}

    //public List<CardVFXMapping> vfxMappings;
    //private Dictionary<CardState, GameObject> cardStateVFXMap;

    //public void Awake()
    //{
    //    // 将列表转换为字典
    //    cardStateVFXMap = new Dictionary<CardState, GameObject>();

    //    foreach (CardVFXMapping mapping in vfxMappings)
    //    {
    //        if (!cardStateVFXMap.ContainsKey(mapping.state))
    //        {
    //            cardStateVFXMap.Add(mapping.state, mapping.vfxObject);
    //        }
    //    }

    //}

    //// 切换卡牌的显示状态
    //public void SwitchCardState(CardState newState)
    //{
    //    foreach (var vfx in cardStateVFXMap.Values)
    //    {
    //        vfx.SetActive(false);
    //    }

    //    // 启动新的粒子效果
    //    if (cardStateVFXMap.ContainsKey(newState))
    //    {
    //        GameObject vfx = cardStateVFXMap[newState];
    //        vfx.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("该卡牌状态不存在粒子效果映射");
    //    }

    //    // 启动协程，延迟0.2秒后启动新的粒子效果
    //    //StartCoroutine(ActivateVFXWithDelay(newState, 0.2f));
    //}

    //// 协程方法：延迟启动粒子效果
    //private IEnumerator ActivateVFXWithDelay(CardState newState, float delay)
    //{
    //    // 等待指定的时间
    //    yield return new WaitForSeconds(delay);

    //    // 启动新的粒子效果
    //    if (cardStateVFXMap.ContainsKey(newState))
    //    {
    //        GameObject vfx = cardStateVFXMap[newState];
    //        vfx.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("该卡牌状态不存在粒子效果映射");
    //    }
    //}
}
