using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BalttlePanel : UIPanel
{
    public GameObject AvatarsContianer; // 角色信息容器
    public Transform AvatarsParent; // 角色信息的父物体
    public GameObject AvatarPrefab; // 角色信息预制体
    public List<AvatarInfoController> avatars = new List<AvatarInfoController>(); // 角色信息控制器列表

    public void InitAvatarsData(object obj)
    {
        var playerCharacters = BattleManager.Instance.playerCharacters;
        
        foreach (var avatar in avatars)
        {
            Destroy(avatar.gameObject);
        }
        avatars.Clear();

        foreach (var character in playerCharacters)
        {
            AddAvatarToList(character);
        }
    }

    public void AddAvatarToList(CharacterBase character)
    {
        var avatar = Instantiate(AvatarPrefab, AvatarsParent).GetComponent<AvatarInfoController>();
        avatar.UpdateAvatarData(character);
        avatars.Add(avatar);
    }
    
    public void RemoveAvatarFromList(CharacterBase character)
    {
        var avatar = avatars.Find(a => a.character == character);
        avatars.Remove(avatar);
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
