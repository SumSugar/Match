using UnityEngine;
public class CardInitState : IState<BattleManager>
{
    private BattleManager manager;

    public void Enter(BattleManager manager)
    {
        Debug.Log("初始化牌组");
        this.manager = manager;
        // 初始化每个角色的卡组
        foreach (var c in manager.playerCharacters)
        {
            c.breakerDeck.Initialize(c);
            //c.destinyTrail.Initialize(c);
            //c.destinyTrail.DrawDestiny(c.destinyTrail.initDestinys);
        }

        foreach (var c in manager.enemyCharacters)
        {
            c.breakerDeck.Initialize(c);
            //c.destinyTrail.Initialize(c);
            //c.destinyDeck.DrawDestiny(c.destinyDeck.initDestinys);
        }

        // 初始化完成后进入CardDrawState（抽牌阶段）
        manager.stateMachine.ChangeState(BattleStateType.CardDraw);
    }

    public void Update() { }
    public void Exit() 
    {
    }
}
