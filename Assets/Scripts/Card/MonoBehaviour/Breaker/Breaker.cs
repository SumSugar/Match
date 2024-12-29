using System;
using UnityEngine;

[System.Serializable]
public class Breaker
{
    public BreakerDataSO data;

    public CharacterBase ownerCharacter;
    public BreakerDeck ownerDeck;
    public bool isAbiliable;

    public void Initialize(BreakerDataSO data ,CharacterBase ownerCharacter, BreakerDeck ownerDeck)
    {
        this.ownerCharacter = ownerCharacter;
        this.ownerDeck = ownerDeck;
        this.data = data;
    }

    public void ExcuteBreakerEffects(CharacterBase form, CharacterBase target)
    {
        // 逻辑处理：比如通知回收卡牌
        //onDiscard?.Invoke();

        foreach (var effect in data.effects)
        {
            effect.Excute(form, target);
        }
    }

    public void UpdateBreakerState()
    {
        // todo: 根据状态逻辑决定 isAbiliable 的值
        // 例如：
        // isAbiliable = breakerData.cooldown <= owner.CurrentMana;
    }
}
