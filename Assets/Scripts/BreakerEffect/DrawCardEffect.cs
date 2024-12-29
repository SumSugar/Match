using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        //从卡组中抽取一张牌
        drawCardEvent?.RaiseEvent(value , this);
    }

}
