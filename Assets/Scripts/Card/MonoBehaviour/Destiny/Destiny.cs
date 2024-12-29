using System;

[System.Serializable]
public class Destiny 
{
    public DestinyDataSO destinyData;
    public CharacterBase owner;
    public bool IsBroken;
    
    public Action OnDestoryAction;
    //[Header(header: "广播事件")]
    //public ObjectEventSO discardDestinyEvent;
    //public IntEventSO costEvent;

    public void Initialize(DestinyDataSO destinyData, CharacterBase owner = null , DestinyTrail trail = null)
    {
        this.IsBroken = false;
        this.owner = owner;
        this.destinyData = destinyData;
    }


    public void ExcuteDestinyEffects(CharacterBase form , CharacterBase target)
    {
        //todo减少对应能量，通知回收卡牌
        //costEvent.RaiseEvent(destinyData.cost, this);
        //discardDestinyEvent.RaiseEvent(this, this);
        foreach (var effect in destinyData.effects)
        {
            effect.Excute(form, target);
        }
    }

    public void Broken(CharacterBase target)
    {
        ExcuteDestinyEffects(owner, target);
        IsBroken = true;
        OnDestoryAction?.Invoke();//通知UI
    }

    public void UpdateDestinyState()
    {
        //Data.cooldown <= owner.CurrentMana;
        //costText.color = isAbailiable ? Color.green : Color.red;
    }
    
}
