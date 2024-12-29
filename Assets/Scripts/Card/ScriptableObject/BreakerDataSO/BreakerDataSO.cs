using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BreakerDataSO", menuName = "Breaker/BreakerDataSO")]

public class BreakerDataSO : ScriptableObject
{
    public string ID;
    public string Name;
    public BreakerType Type;
    public Sprite Icon;
    public Sprite back;
    public Sprite OnWorldSprite;
    public CharacterBase owner;
    public int baseValue;

    [TextArea]
    public string description;

    //TODO：执行的效果
    public List<Effect> effects;
}

