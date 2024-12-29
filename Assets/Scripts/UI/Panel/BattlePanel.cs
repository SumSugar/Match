using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : UIPanel
{
    public GameObject avatarsContianer; // 角色信息容器

    public Transform allyAvatarsParent; // 友方角色信息的父物体
    public Transform enemyAvatarsParent; // 敌方角色信息的父物体
    public GameObject avatarPrefab; // 角色信息预制体

    public List<AvatarInfoView> avatars = new List<AvatarInfoView>(); // 角色信息控制器列表

    public GameObject preparetionPhaseEndButton;
    public GameObject showhandBreakersButton;

    [Header(header:"事件通知")]
    public ObjectEventSO AnimateCompletedEvent; // 面板元素完成事件
    public ObjectEventSO PreparationPhaseEndButtonClickEvent;

    public void Initialize()
    {
        preparetionPhaseEndButton.GetComponent<Button>().onClick.AddListener(ClickPreparetionPhaseEndButton);
        InitAvatarsData();
    }

    public void InitAvatarsData()
    {
        var units = BattleManager.Instance.GetAllBattleUnits();
        foreach (var avatar in avatars)
        {
            Destroy(avatar.gameObject);
        }
        avatars.Clear();

        foreach (var character in units)
        {
            AddAvatarToList(character);
        }

    }

    public void DisPlayAvatarsInfo()
    {
        foreach (var avatar in avatars)
        {
            avatar.AnimateDestinyTokenViews();
        }
    }

    public void AddAvatarToList(CharacterBase character)
    {
        Transform parent; 
        if (character.tag == "Ally")
        {
            parent = allyAvatarsParent;
        }
        else
        {
            parent = enemyAvatarsParent;
        }
        var avatar = Instantiate(avatarPrefab, parent).GetComponent<AvatarInfoView>();
        avatar.Initiliaze(character);
        avatars.Add(avatar);
    }

    public void RemoveAvatarFromList(CharacterBase character)
    {
        var avatar = avatars.Find(a => a.character == character);
        avatars.Remove(avatar);
    }

    public void OnAvatarUpdateComplete(object obj)
    {
        bool isCompleted = avatars.All(c => c.state == UIPanelState.Active);
        Debug.Log("isCompleted: " + isCompleted);
        if (isCompleted)
        {
            AnimateCompletedEvent.RaiseEvent(this, this);
        }
    }


    public void ClickPreparetionPhaseEndButton()
    {
        PreparationPhaseEndButtonClickEvent.RaiseEvent(this,this);
        preparetionPhaseEndButton.SetActive(false);
    }
    public void ShowPreparetionPhaseEndButton()
    {
        preparetionPhaseEndButton.SetActive(true);
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
