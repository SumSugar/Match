using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinyTrail : MonoBehaviour
{
    public int MaxDestinies;
    public CharacterBase owner;
    public List<Destiny> destinies = new List<Destiny>(); // 命运骰子列表
    public bool IsVulnerable { get; private set; } // 是否进入暴露状态

    /// <summary>
    /// 初始化命轨数据。在角色生成完成后调用
    /// </summary>
    public void Initialize(CharacterBase chara)
    {
        MaxDestinies = chara.characterData.initialHandSize;
        IsVulnerable = false;
        owner = chara;

        for (int i = 0; i < owner.characterData.equippedDestinyDataList.Count; i++)
        {
            //TODO
            DestinyDataSO data = owner.characterData.equippedDestinyDataList[i];
            AddDestiny(data, chara.destinyTrail);
        }
    }

    public void AddDestiny(DestinyDataSO data , DestinyTrail ownerTrail)
    {
        Destiny destiny = new Destiny();
        destiny.Initialize(data , owner , ownerTrail);
        destinies.Add(destiny);  
    }

    // 破坏下一个未破坏的骰子
    public void BreakNextDestinyDestiny(CharacterBase source)
    {
        foreach (var destiny in destinies)
        {
            if (!destiny.IsBroken)
            {
                destiny.Broken(source);
                CheckAllDestiniesBroken();
                return;
            }
        }
    }

    // 检查是否所有命运骰子都被破坏
    private void CheckAllDestiniesBroken()
    {
        if (destinies.All(d => d.IsBroken))
        {
            IsVulnerable = true; // 所有命运骰子破坏，进入暴露状态
        }
    }

    // 检查命运骰子是否为空
    public bool IsDestinyEmpty()
    {
        if (destinies.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
