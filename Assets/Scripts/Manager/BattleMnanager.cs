
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public List<CharacterBase> playerCharacters;
    public List<CharacterBase> enemyCharacters;
    public SpawnerBase spawner;
    public LevelDataSO currentLevelData;
    public BattleStateMachine stateMachine;

    public bool isBattleEnd;

    public Action<CharacterBase> CharacterActionComplete;
    public Action ActionShowComplete;

    [Header("广播事件")]
    public ObjectEventSO BattleStartEvent;
    public ObjectEventSO PreparationPhaseStartEvent;
    public ObjectEventSO ActionPhaseStartEvent;
    public ObjectEventSO BattleEndEvent;

    public ObjectEventSO ShowUIBattlePanelEvent;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new BattleStateMachine(this);
        isBattleEnd = false;

    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void OnTargetRequestEvent(object requester)
    {
        // 根据请求者确定敌人列表
        CharacterBase character = requester as CharacterBase;

        List<CharacterBase> enemies = playerCharacters.Contains(character) ? enemyCharacters : playerCharacters;

        // 获取敌人列表中的第一个存活角色
        character.currentTarget = enemies.Find(enemy => !enemy.isDead);
    }

    public void RemoveCharacterFromList(CharacterBase character)
    {
        // 将角色从队伍中移除
        //ToDo:
        if (character.tag == "Ally")
        {
            playerCharacters.Remove(character);
        }
        else
        {
            enemyCharacters.Remove(character);
        }

    }

    public void OnCharacterDead(object obj)
    {
        CharacterBase character = obj as CharacterBase;
        RemoveCharacterFromList(character);
        BreakerDeckManager.Instance.RemoveDeadCharacterBreakers(character);
        BreakerDeckManager.Instance.RemoveDeck(character.breakerDeck);
    }

    public void OnAfterLevelLoadedEvent(object obj)
    {
        LevelDataSO data = obj as LevelDataSO;
        currentLevelData = data;
        stateMachine.ChangeState(BattleStateType.PreBattle);
    }


    public void OnBreakersQueueComplete(object obj)
    {
        stateMachine.ChangeState(BattleStateType.ActionPhase);
    }

    // 当角色动作完成时，由角色回调此函数
    public void OnCharacterActionComplete(object obj)
    {
        // 上一个卡牌完成，继续处理下一个卡牌
        CharacterBase character = obj as CharacterBase;

        BreakerDeckManager.Instance.RemoveBreakerFromBePlayedQueue(character.currentBreaker);
        //从待使用队列中移除当前卡牌后，将角色currentBreaker置为null
        character.currentBreaker = null;
        CharacterActionComplete?.Invoke(character);
    }

    public void OnActionShowComplete(object obj)
    {
        // 上一个卡牌完成，继续处理下一个卡牌
        ActionShowComplete?.Invoke();
    }

    // 当所有卡牌执行完毕时结束此阶段
    private void EndActionPhase()
    {
        // 可选择在此检查战斗结束或回到准备阶段
        CheckBattleEnd();
    }

    public bool CheckBattleEnd()
    {
        bool playersAlive = playerCharacters.Exists(c => !c.isDead);
        bool enemiesAlive = enemyCharacters.Exists(c => !c.isDead);

        if (!playersAlive || !enemiesAlive)
        {
            isBattleEnd = true;
            return true;
        }

        return false;
    }

    public void AddUnit(CharacterBase unit)
    {
        if (unit.tag == "Ally")
        {
            playerCharacters.Add(unit);
        }
        else if (unit.tag == "Enemy")
        {
            enemyCharacters.Add(unit);
        }
        
    }

    public List<CharacterBase> GetAllBattleUnits()
    {
        List<CharacterBase> units = new List<CharacterBase>();
        units.AddRange(playerCharacters);
        units.AddRange(enemyCharacters);
        return units;
    }
}
