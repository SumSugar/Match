using UnityEngine;
public class CardDrawState : IState<BattleManager>
{
    private BattleManager manager;

    public void Enter(BattleManager manager)
    {
        Debug.Log("抽牌");
        this.manager = manager;

        int i = 0;
        // 每回合开始时，玩家和敌人抽初始手牌数量（可以由配置决定）
        foreach (var c in manager.playerCharacters)
        {
            i++;
            Debug.Log($"角色{i}抽牌");
            c.breakerDeck.DrawBreaker(c.breakerDeck.initBreakers);
        }

        foreach (var c in manager.enemyCharacters)
        {
            i++;
            Debug.Log($"角色{i}抽牌");
            c.breakerDeck.DrawBreaker(c.breakerDeck.initBreakers);
        }

        // 角色抽牌完成后进入准备阶段（）
        manager.stateMachine.ChangeState(BattleStateType.PreparationPhase);
    }

    public void Update() {}
    public void Exit() {}
}

