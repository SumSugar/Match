using System.Collections;
using UnityEngine;

public class PreparationPhaseState : IState<BattleManager>
{
    private BattleManager battleManager;
    //private bool isShowComplete = false;

    public void Enter(BattleManager manager)
    {
        Debug.Log("进入准备阶段");
        battleManager = manager;

        // 开始准备阶段的演出逻辑 (比如播放UI动画、角色入场动画等)
        // 当演出结束时调用OnShowComplete()
        StartPreparationShow();
    }

    public void Update()
    {
        //// 如果不需要在Update中检查，也可以直接在回调中调用
        //if (isShowComplete)
        //{
        //    // 当演出结束后，告诉BattleManager进入下一个状态
        //    battleManager.ChangeState(new ActionPhaseState());
        //}
    }

    public void Exit()
    {
        // 清理阶段
        Debug.Log("准备阶段结束");
    }

    private void StartPreparationShow()
    {
        Debug.Log("播放动画");

        battleManager.PreparationPhaseStartEvent.RaiseEvent(battleManager, battleManager);

        BreakerDeckManager.Instance.LoadPlayerHandBreakers();
        BreakerDeckManager.Instance.DisplayPlayerHnadBreakers(battleManager);

        //battleManager.StartCoroutine(WaitForShowComplete());
    }

    private IEnumerator WaitForShowComplete()
    {
        // 等待2秒模拟演出耗时
        yield return new WaitForSeconds(2f);
        OnShowComplete();
    }

    private void OnShowComplete()
    {
        //isShowComplete = true;
    }
}

