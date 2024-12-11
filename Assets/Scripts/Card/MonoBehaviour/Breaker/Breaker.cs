using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Breaker : MonoBehaviour,IPointerClickHandler
{
    [Header(header:"组件")]
    public SpriteRenderer breakerSprite;
    public GameObject breakerBack;
    public TextMeshPro costText, descriptionText, typeText, breakerName;
    public BreakerDataSO breakerData;

    [Header(header: "原始数据")]
    public Vector3 originalLoaclPosition;
    public Quaternion originalquaternion;
    public Vector3 originalScale;
    public int originalLayerOrder;

    public bool isAnimating;
    public bool isAbiliable;
    public CardViewController cardViewController;
    public CharacterBase owner;

    public BreakerState breakerState;
    [Header(header: "广播事件")]
    public ObjectEventSO discardBreakerEvent;
    public IntEventSO costEvent;

    private void Awake()
    {
        cardViewController = GetComponentInChildren<CardViewController>();
    }

    public void Init(BreakerDataSO data)
    {
        breakerData = data;
        breakerSprite.sprite = data.breakerIcon;
        descriptionText.text = data.description;
        breakerName.text = data.breakerName;
        typeText.text = data.breakerType switch
        {
            BreakerType.Attack => "Attack",
            BreakerType.Abilities => "Ability",
            _ => throw new System.NotImplementedException(),
        };

        breakerState = BreakerState.InHand;

    }

    public void UpdatePositionRotation(Vector3 positon , Quaternion rotation)
    {
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
        originalLoaclPosition = positon;
        originalquaternion = rotation;
    }
    public void UpdateScale(Vector3 scale)
    {
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
        originalScale = scale;
        
    }

    public void RestBreakertransform()
    {
        transform.SetLocalPositionAndRotation(originalLoaclPosition, originalquaternion);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExcuteBreakerEffects(CharacterBase form , CharacterBase target)
    {
        //todo减少对应能量，通知回收卡牌
        //costEvent.RaiseEvent(breakerData.cost, this);
        discardBreakerEvent.RaiseEvent(this, this);
        foreach (var effect in breakerData.effects)
        {
            effect.Excute(form, target);
        }
    }

    public void ChangeToBack()
    {
        breakerBack.SetActive(true);
    }

    public void ChangeToFront()
    {
        breakerBack.SetActive(false);
    }

    public void UpdateBreakerState()
    {
        //开始计算冷却
        //isAbailiable =
        //
        //
        //
        //Data.cooldown <= owner.CurrentMana;
        costText.color = isAbiliable ? Color.green : Color.red;
    }

    public void ToggleSelection()
    {
        switch (breakerState)
        {
            case BreakerState.Selected:
                transform.DOLocalMoveY(originalLoaclPosition.y, 0.1f);
                breakerState = BreakerState.InHand;
                break;
            case BreakerState.Inactive:
                break;
            case BreakerState.Drawn:
                break;
            case BreakerState.InHand:
                transform.DOLocalMoveY(originalLoaclPosition.y + 1f, 0.1f);
                breakerState = BreakerState.Selected;
                break;
            case BreakerState.Played:
                break;
            case BreakerState.Destroyed:
                break;
            case BreakerState.Highlighted:
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleSelection();
    }
}
