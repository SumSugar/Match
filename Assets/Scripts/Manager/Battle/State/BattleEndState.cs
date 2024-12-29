using UnityEngine;

public class BattleEndState : IState<BattleManager>
{
    private BattleManager manager;

    public void Enter(BattleManager manager)
    {
        this.manager = manager;
        // 显示胜利/失败UI，等待玩家确认或直接跳转场景
        Debug.Log("Battle End !");
        BreakerDeckManager.Instance.RemoveBreakerBattleEnd();
        manager.BattleEndEvent.RaiseEvent(manager, manager);
    }

    public void Update() { }
    public void Exit() { }
}

