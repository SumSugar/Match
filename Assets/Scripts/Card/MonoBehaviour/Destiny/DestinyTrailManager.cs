using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinyTrailManager : Singleton<DestinyTrailManager>
{
    private Dictionary<CharacterBase, DestinyTrail> characterTrails = new Dictionary<CharacterBase, DestinyTrail>();

    // 初始化角色的 DestinyTrail
    public void RegisterCharacter(CharacterBase character, DestinyTrail destinyTrail)
    {
        if (!characterTrails.ContainsKey(character))
        {
            characterTrails.Add(character, destinyTrail);
            // 此处可订阅destinyTrail的事件，比如dice破坏或进入暴露状态的回调
            // destinyTrail.OnDiceBroken += HandleDiceBroken;
            // destinyTrail.OnBecomeVulnerable += HandleCharacterVulnerable;
        }
    }

    /// <summary>
    /// 卸载角色，当角色死亡或离开战场调用
    /// </summary>
    public void UnregisterCharacter(CharacterBase character)
    {
        if (characterTrails.ContainsKey(character))
        {
            // 可在此处取消事件订阅
            characterTrails.Remove(character);
        }
    }

    /// <summary>
    /// 获取角色的DestinyTrail
    /// </summary>
    public DestinyTrail GetDestinyTrail(CharacterBase character)
    {
        if (characterTrails.TryGetValue(character, out var trail))
        {
            return trail;
        }
        return null;
    }

    /// <summary>
    /// 检查角色是否处于暴露状态
    /// </summary>
    public bool IsCharacterVulnerable(CharacterBase character)
    {
        if (characterTrails.TryGetValue(character, out var trail))
        {
            return trail.IsVulnerable;
        }
        return false;
    }

    /// <summary>
    /// 获取当前所有处于暴露状态的角色
    /// </summary>
    public List<CharacterBase> GetAllVulnerableCharacters()
    {
        return characterTrails.Where(kv => kv.Value.IsVulnerable).Select(kv => kv.Key).ToList();
    }
    // 根据需要添加事件回调函数
    // private void HandleDiceBroken(CharacterBase character, Destiny brokenDestiny) { ... }
    // private void HandleCharacterVulnerable(CharacterBase character) { ... }

    //如有需要，可以在 DestinyTrailManager 中实现对所有角色命运骰子状态的统一存取。
}
