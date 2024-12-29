using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AvatarInfoView : UIPanel
{
    public CharacterBase character;
    public Image avatarImage;
    public Transform destinyTokenParent; // 命运石父物体

    public GameObject destinyTokenViewPrefab; // 命运石预制体
    public List<DestinyTokenView> destinyTokenViews; // 命运石视图数组
    public float scaleDuration = 0.5f; // 弹出动画持续时间
    public float delayBetweenAnimations = 0.2f; // 动画之间的延迟

    [Header(header: "事件通知")]
    public ObjectEventSO AvatarUpdateCompeletedEvent;

    public void Initiliaze (CharacterBase character)
    {
        this.character = character;
        avatarImage.sprite = character.characterData.Icon;
        RestTokenList();
        InilizeTokenList();
    }

    public void InilizeTokenList()
    {
        Debug.Log("初始化token");
        foreach (var destiny in character.destinyTrail.destinies)
        {
            Debug.Log("token" + destiny);
            var destinyTokenView = Instantiate(destinyTokenViewPrefab, destinyTokenParent).GetComponent<DestinyTokenView>();
            destinyTokenViews.Add(destinyTokenView);
            destinyTokenView.RegisterDestiny(destiny);
        }
    }


    public void AnimateDestinyTokenViews()
    {
        List<Transform> transforms = new List<Transform>();
        foreach (var item in destinyTokenViews)
        {
            transforms.Add(item.gameObject.transform);
        }

        PlaySequentialAnimations(transforms);
    }


    public void RestTokenList()
    {
        foreach (var item in destinyTokenViews)
        {
            Destroy(item.gameObject);
        }
        destinyTokenViews.Clear();
    }


    public void PlaySequentialAnimations(List<Transform> icons)
    {
        //创建动画序列
        Sequence sequence = DOTween.Sequence();

        foreach (Transform icon in icons)
        {
            // 初始缩放设置为 0
            icon.localScale = Vector3.zero;

            // 添加动画：缩放到 1
            sequence.Append(icon.DOScale(1f, scaleDuration).SetEase(Ease.InOutQuad)) // 使用平滑曲线
                    .AppendInterval(delayBetweenAnimations); // 添加延迟
        }

        sequence.OnComplete(() => AvatarUpdateCompelete());
    }

    public void AvatarUpdateCompelete()
    {
        //state = UIPanelState.Active;
        AvatarUpdateCompeletedEvent.RaiseEvent(this,this);
    }


}
