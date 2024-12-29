using UnityEngine;


public class PoisonCondition : StatusEffect
{
    private int damagePerTurn;

    public PoisonCondition(CharacterBase character, CharacterBase source, int turns, int damagePerTurn)
        : base(character, source, StatusType.Poison, StatusDurationType.Temporary, false, turns, false)
    {
        this.damagePerTurn = damagePerTurn;
    }

    protected override void ApplyImpact()
    {
        Debug.Log($"{character.Name} 受到 {damagePerTurn} 点中毒伤害");
        VFXManager.Instance.TriggerEffect(VFXType.Posion, character.vfxTransform.position);
        character.TakeDotDamage(damagePerTurn);
    }
}