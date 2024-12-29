using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class BreakerPresenter : MonoBehaviour, IPointerClickHandler
{
    [Header("组件")]
    public SpriteRenderer breakerSprite;
    public TextMeshPro costText, descriptionText, typeText, breakerName;

    public BreakerState breakerState;
    [Header("原始数据")]
    public Vector3 originalLoaclPosition;
    public Quaternion originalquaternion;
    public Vector3 originalScale;
    public int originalLayerOrder;
    public bool isAnimating = false;

    // 将数据和逻辑部分放到纯数据类中
    public Breaker breaker;

    // 初始化方法，将数据注入并更新界面显示
    public void Initialize(Breaker breaker)
    {
        this.breaker = breaker;
        breakerState = BreakerState.InHand;
        UpdateViewFromModel();
    }
    private void UpdateViewFromModel()
    {
        var data = breaker.data;
        breakerSprite.sprite = data.Icon;
        descriptionText.text = data.description;
        breakerName.text = data.Name;
        typeText.text = data.Type switch
        {
            BreakerType.Attack => "Attack",
            BreakerType.Abilities => "Ability",
            _ => throw new System.NotImplementedException(),
        };
    }

    public void UpdatePositionRotation(Vector3 positon, Quaternion rotation)
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

    public void ExecuteBreakerEffects(CharacterBase from, CharacterBase target)
    {
        //// 调用纯数据逻辑类中的方法
        //breaker.ExcuteBreakerEffects(from, target, () =>
        //{
        //    //discardBreakerEvent.RaiseEvent(this, this);
        //});
    }

    public void ChangeToBack()
    {
        breakerSprite.sprite = breaker.data.back;
    }

    public void ChangeToFront()
    {
        breakerSprite.sprite = breaker.data.Icon;
    }

    public void UpdateBreakerState()
    {
        breaker.UpdateBreakerState();
        costText.color = breaker.isAbiliable ? Color.green : Color.red;
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
