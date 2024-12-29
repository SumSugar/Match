using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]

public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;

    [TextArea]
    public string description;

    //TODO：执行的效果
    public List<Effect> effects;
}