using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public List<CharacterBase> playerCharacters;
    public List<CharacterBase> enemyCharacters;

    private Queue<CharacterBase> actionQueue;


    // 定义事件，用于不同阶段的切换
    [Header(header: "广播")]
    public ObjectEventSO UnitSpawnEvent;

    public ObjectEventSO BattleStartEvent;
    public ObjectEventSO PreparationPhaseStartEvent;
    public ObjectEventSO PreparationPhaseEndEvent;
    public ObjectEventSO ActionPhaseStartEvent;
    public ObjectEventSO ActionPhaseEndEvent;

    //private void Start()
    //{
    //    StartBattleLoop();
    //}

    // 主循环控制战斗流程
    private void StartBattleLoop()
    {
        if (IsBattleActive())
        {
            StartPreparationPhase();
        }
        else
        {
            EndBattle();
        }
    }

    // 准备阶段
    private void StartPreparationPhase()
    {
        Debug.Log("准备阶段开始");
        PreparationPhaseStartEvent.RaiseEvent(this,this);

        // 模拟准备阶段延迟
        //Invoke(nameof(EndPreparationPhase), 5f); // 5秒准备阶段时间
    }

    public void OnEndPreparationPhase(object obj)
    {
        Debug.Log("准备阶段结束");
        //PreparationPhaseEndEvent.RaiseEvent(this, this);
        //StartActionPhase();
    }

    // 行动阶段
    private void StartActionPhase()
    {
        Debug.Log("行动阶段开始");
        ActionPhaseStartEvent.RaiseEvent(this, this);

        InitializeActionQueue();
        ProcessNextCharacterAction();
    }

    private void InitializeActionQueue()
    {
        actionQueue = new Queue<CharacterBase>();

        // 合并角色列表并按速度排序
        List<CharacterBase> allCharacters = new List<CharacterBase>();
        allCharacters.AddRange(playerCharacters);
        allCharacters.AddRange(enemyCharacters);
        allCharacters.Sort((a, b) => b.speed.CompareTo(a.speed)); // 按速度从高到低排序

        foreach (var character in allCharacters)
        {
            if (!character.isDead)
            {
                actionQueue.Enqueue(character);
            }
        }
    }

    // 处理每个角色的动作
    private void ProcessNextCharacterAction()
    {
        if (actionQueue.Count > 0)
        {
            var character = actionQueue.Dequeue();
            character.StartAction(); // 让角色开始动作，完成后触发 OnCharacterActionComplete
        }
        else
        {
            EndActionPhase();
        }
    }

    public void OnCharacterActionComplete(object obj)
    {
        CharacterBase character = obj as CharacterBase;
        ProcessNextCharacterAction();
    }

    private void EndActionPhase()
    {
        Debug.Log("行动阶段结束");
        ActionPhaseEndEvent.RaiseEvent(this, this);

        // 所有角色行动结束后重新进入准备阶段
        StartBattleLoop();
    }

    private bool IsBattleActive()
    {
        bool playersAlive = playerCharacters.Exists(c => !c.isDead);
        bool enemiesAlive = enemyCharacters.Exists(c => !c.isDead);
        return playersAlive && enemiesAlive;
    }

    private void EndBattle()
    {
        Debug.Log("战斗结束");
        // 在这里可以执行战斗结束的逻辑，比如显示胜利/失败界面
    }

    public void OnTargetRequestEvent(object requester)
    {
        // 根据请求者确定敌人列表
        CharacterBase character = requester as CharacterBase;

        List<CharacterBase> enemies = playerCharacters.Contains(character) ? enemyCharacters : playerCharacters;

        // 获取敌人列表中的第一个存活角色
        character.currentTarget = enemies.Find(enemy => !enemy.isDead);
    }

    public void OnAfterRoomLoadedEvent(object obj)
    {
        UnitSpawnEvent.RaiseEvent(obj, this);


        BattleStartEvent.RaiseEvent(this, this);
        StartBattleLoop();
    }

    public void RemoveCharacterFromQueue(object obj)
    {
        CharacterBase deadCharacter = obj as CharacterBase;
        // 将死亡角色从队列中移除
        Queue<CharacterBase> updatedQueue = new Queue<CharacterBase>();

        foreach (var character in actionQueue)
        {
            if (character != deadCharacter)
            {
                updatedQueue.Enqueue(character);
            }
        }

        actionQueue = updatedQueue;
    }

    public void AddUnits(CharacterBase unit)
    {
        if(unit.tag == "Ally")
        {
            playerCharacters.Add(unit);
        }
        else if (unit.tag == "Enemy")
        {
            enemyCharacters.Add(unit);
        }
    }

}