using System.Net;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Destiny : MonoBehaviour/*,IPointerEnterHandler,IPointerExitHandler*/
{
    [Header(header:"组件")]
    public SpriteRenderer destinySprite;
    public TextMeshPro costText, descriptionText, typeText, destinyName;
    public DestinyDataSO destinyData;

    [Header(header: "原始数据")]
    public Vector3 originalLoaclPosition;
    public Quaternion originalquaternion;
    public int originalLayerOrder;

    public bool isAnimating;
    public bool isAbailiable;

    public CharacterBase owner;
    [Header(header: "广播事件")]
    public ObjectEventSO discardDestinyEvent;
    public IntEventSO costEvent;

    //private void Start()
    //{
    //    Init(destinyData);
    //}

    public void Init(DestinyDataSO data)
    {
        destinyData = data;
        destinySprite.sprite = data.destinyImage;
        costText.text = data.cooldown.ToString();
        descriptionText.text = data.description;
        destinyName.text = data.destinyName;
        typeText.text = data.destinyType switch
        {
            DestinyType.Attack => "Attack",
            DestinyType.Abilities => "Ability",
            _ => throw new System.NotImplementedException(),
        };

    }

    public void UpdateSorting(int order)
    {
        
    }

    public void UpdatePositionRotation(Vector3 positon , Quaternion rotation)
    {
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
        originalLoaclPosition = positon;
        originalquaternion = rotation;
    }

    public void RestDestinytransform()
    {
        transform.SetLocalPositionAndRotation(originalLoaclPosition, originalquaternion);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExcuteDestinyEffects(CharacterBase form , CharacterBase target)
    {
        //todo减少对应能量，通知回收卡牌
        //costEvent.RaiseEvent(destinyData.cost, this);
        discardDestinyEvent.RaiseEvent(this, this);
        foreach (var effect in destinyData.effects)
        {
            effect.Excute(form, target);
        }

        
    }

    public void UpdateDestinyState()
    {
        //开始计算冷却
        //isAbailiable =
        //
        //
        //
        //Data.cooldown <= owner.CurrentMana;
        costText.color = isAbailiable ? Color.green : Color.red;
    }
    
}
