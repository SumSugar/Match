using System.Collections;
using UnityEngine;

public class ActionPhaseState : IState<BattleManager>
{
    private BattleManager manager;
    //private bool isShowComplete = false;

    public void Enter(BattleManager manager)
    {
        Debug.Log("进入行动阶段");
        this.manager = manager;

        manager.CharacterActionComplete += OnCharacterActionComplete;
        // 开始行动阶段演出逻辑
        StartActionShow();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        // 清理
        manager.CharacterActionComplete -= OnCharacterActionComplete;
    }

    private void StartActionShow()
    {
        // 模拟动画或特效演出
        //manager.StartCoroutine(WaitForActionShowComplete());
        StartBreakerQueue();
    }

    private IEnumerator WaitForActionShowComplete()
    {
        yield return new WaitForSeconds(3f);
        OnShowComplete();
        StartBreakerQueue();
    }

    private void OnShowComplete()
    {
        //isShowComplete = true;
    }

    // 当角色动作完成时，由角色回调此函数
    public void OnCharacterActionComplete(CharacterBase character)
    {
        // 上一个卡牌完成，继续处理下一个卡牌
        if (manager.CheckBattleEnd())
        {
            manager.stateMachine.ChangeState(BattleStateType.BattleEnd);
        }
        else
        {
            ProcessNextBreaker();
        }
    }

    // 开始处理卡牌队列
    public void StartBreakerQueue()
    {
        ProcessNextBreaker();
    }

    // 处理下一个卡牌
    private void ProcessNextBreaker()
    {
        var breaker = BreakerDeckManager.Instance.GetCurrentBreaker() ;
        if (breaker != null)
        {
            Debug.Log("触发卡牌：" + breaker.data.Name);
            CharacterBase character = breaker.ownerCharacter;
            // 假设Card有一个方法StartCardAction，启动卡牌的效果、动画、伤害结算等
            // 完成后会调用BattleManager.OnCardActionComplete(...)通知管理器
            character.StartAction(breaker);

        }
        else
        {
            Debug.Log("行动队列完成");
            manager.stateMachine.ChangeState(BattleStateType.CardDraw);
        }
    }

}