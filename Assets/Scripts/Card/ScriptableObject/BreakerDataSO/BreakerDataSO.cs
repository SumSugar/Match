using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BreakerDataSO", menuName = "Breaker/BreakerDataSO")]

public class BreakerDataSO : ScriptableObject
{
    public string breakerID;
    public string breakerName;
    public BreakerType breakerType;
    public Sprite breakerIcon;
    public Sprite breakerOnWorldSprite;
    public CharacterBase owner;
    public int baseValue;

    [TextArea]
    public string description;

    //TODO：执行的效果
    public List<Effect> effects;
}
