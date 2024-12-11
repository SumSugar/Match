using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "DestinyDataSO", menuName = "Destiny/DestinyDataSO")]

public class DestinyDataSO : ScriptableObject
{
    public string destinyName;
    public Sprite destinyImage;
    public float cooldown;
    public DestinyType destinyType;
    public CharacterBase owner;
    public int baseValue;

    [TextArea]
    public string description;

    //TODO：执行的效果
    public List<Effect> effects;
}
