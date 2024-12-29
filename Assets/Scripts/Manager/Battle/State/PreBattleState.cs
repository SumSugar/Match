using UnityEngine;
public class PreBattleState : IState<BattleManager>
{

    public void Enter(BattleManager manager)
    {
        Debug.Log("角色生成");
        // 使用Spawner生成玩家和敌人
        manager.spawner.SpawnCharacters(manager.currentLevelData);

        manager.BattleStartEvent.RaiseEvent(manager, manager);
        // 生成完成后进入卡组初始化阶段
        manager.stateMachine.ChangeState(BattleStateType.CardInit);
    }

    public void Update() { }
    public void Exit() 
    {
    }

}
