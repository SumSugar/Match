using UnityEngine;

public abstract class StatusEffect
{
    public string StatusName { get; protected set; }
    public StatusType Type { get; protected set; }
    public StatusDurationType DurationType { get; protected set; }
    public bool IsBuff { get; protected set; }
    public int RemainingTurns { get; protected set; }
    public bool IsStackable { get; protected set; }
    public int StackCount { get; private set; } = 1;
    public CharacterBase Source { get; private set; }

    protected CharacterBase character;

    public StatusEffect(CharacterBase character, CharacterBase source, StatusType type, StatusDurationType durationType, bool isBuff, int turns, bool isStackable)
    {
        this.character = character;
        Source = source;
        Type = type;
        DurationType = durationType;
        IsBuff = isBuff;
        RemainingTurns = turns;
        IsStackable = isStackable;
    }

    public void ApplyTurnEffect()
    {
        if (RemainingTurns > 0 || DurationType == StatusDurationType.Permanent)
        {
            ApplyImpact();

            if (DurationType == StatusDurationType.Temporary)
            {
                RemainingTurns--;
            }
        }
    }

    public virtual void StackEffect()
    {
        if (IsStackable)
        {
            StackCount++;
            RemainingTurns++; // 叠加状态时延长回合数
        }
    }

    public bool IsExpired()
    {
        return DurationType == StatusDurationType.Temporary && RemainingTurns <= 0;
    }

    protected abstract void ApplyImpact();  // 子类实现具体影响
}