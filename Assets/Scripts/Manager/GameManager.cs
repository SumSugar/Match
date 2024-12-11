using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerDataSO playerData;

    protected override void Awake()
    {
        base.Awake();
        DOTween.SetTweensCapacity(500, 50);

        CharacterManager.Instance.Initialize();
    }

    public void Start()
    {
        NewGameStart();
    }
    /// <summary>
    /// 更新房间的事件监听函数
    /// </summary>
    /// <param name="roomVector"></param>

    public List<CharacterDataSO> GetOwnerCharacterData()
    {
        return playerData.ownedCharacters;
    }

    public void NewGameStart()
    {
        SceneLoadManager.Instance.LoadMenu();
    }
}
