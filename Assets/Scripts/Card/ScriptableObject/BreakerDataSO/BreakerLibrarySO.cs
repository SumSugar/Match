using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "BreakerLibrarySO", menuName = "Breaker/BreakerLibrarySO")]

public class BreakerLibrarySO : ScriptableObject
{
    public List<BreakerLibraryEntry> breakerLibraryList;

    // 根据 BreakerDataSO 获取对应的库条目
    public BreakerLibraryEntry GetEntry(BreakerDataSO breakerData)
    {
        return breakerLibraryList.Find(entry => entry.breakerData == breakerData);
    }

    // 减少卡牌数量
    public void DecreaseCardAmount(BreakerDataSO breakerData)
    {
        var entry = breakerLibraryList.Find(e => e.breakerData == breakerData);
        if (entry.breakerData != null && entry.amount > 0)
        {
            entry.amount--;
            // 如果数量为零，可以选择是否从列表中移除，或者保留为零
            //breakerLibraryList.Remove(entry); // 如果想要移除
        }
    }

    // 增加卡牌数量
    public void IncreaseCardAmount(BreakerDataSO breakerData)
    {
        var entry = breakerLibraryList.Find(e => e.breakerData == breakerData);
        if (entry.breakerData != null)
        {
            entry.amount++;
        }
        else
        {
            // 如果库中没有该卡牌，则添加新的条目
            breakerLibraryList.Add(new BreakerLibraryEntry { breakerData = breakerData, amount = 1 });
        }
    }
}
[System.Serializable]
public class BreakerLibraryEntry
{
    public BreakerDataSO breakerData;
    public int amount;
}
