using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    private List<CharacterDataSO> characters; // 所有角色
    public CharacterDataSO currentCharacter; // 当前选中角色

    public ObjectEventSO CharacterChangedEvnet; // 角色切换事件

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        characters = GameManager.Instance.GetOwnerCharacterData();
        foreach (var character in characters)
        {
            character.Initialize();
        }
    }
    /// <summary>
    /// 选择默认角色
    /// </summary>
    public void SetDefaultCharacter()
    {
        if (characters.Count > 0)
        {
            SetCurrentCharacter(characters[0]); // 默认选择第一个角色
        }
    }

    /// <summary>
    /// 切换当前角色
    /// </summary>
    public void SetCurrentCharacter(CharacterDataSO character)
    {
        currentCharacter = character;

        // 触发角色切换事件
        CharacterChangedEvnet.RaiseEvent(this,this);

        Debug.Log($"当前角色切换为：{character.Name}");
    }
}
