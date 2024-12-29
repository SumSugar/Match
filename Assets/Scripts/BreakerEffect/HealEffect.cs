using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/HealEffect")]

public class HealEffect : Effect
{
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        if(targetType == EffectTargetType.Self)
        {
            PoisonCondition poisonCondition = new PoisonCondition(from, from, 3, 1); // 持续3回合，每回合1点伤害
            from.AddStatusEffect(poisonCondition);
            VFXManager.Instance.TriggerEffect(VFXType.Cursed, from.vfxTransform.position);
            //from.HealHealth(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            target.HealHealth(value);
        }
    }
}
