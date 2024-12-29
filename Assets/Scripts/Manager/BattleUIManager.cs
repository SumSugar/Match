using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [Header("子面板引用")]
    [SerializeField] private BattlePanel battlePanel;
    [SerializeField] private BreakerQueuePanel breakerQueuePanel;
    [SerializeField] private GameObject battleEndPanel;
    // 你可以在Inspector中将BattlePanel拖拽到这里或通过代码获取引用
    // 如果有更多子面板 (技能面板、结算面板、卡牌面板等) 也同样以字段形式引用

    // 如果有UI显示事件，如ShowBattlePanelEvent，也可在这里注册
    // public ObjectEventSO ShowBattlePanelEvent;



    #region Event Handlers

    public void OnBattleStarted(object obj)
    {
        // 战斗开始时显示基本战斗UI框架
        // 此时可以进行战斗UI的初步初始化
        Debug.Log("BattleUIManager: OnBattleStarted");
        // 如果有需要，这里可以Show主战斗面板
        battlePanel.Initialize();
        breakerQueuePanel.Initialize();

        breakerQueuePanel.Show();
        battlePanel.Show();
        battlePanel.DisPlayAvatarsInfo();
    }

    public void OnPreparationPhaseStarted(object obj)
    {
        // 准备阶段开始
        Debug.Log("BattleUIManager: OnPreparationPhaseStarted");
        battlePanel.Show();

        battlePanel.ShowPreparetionPhaseEndButton();
        //battlePanel.DisPlayAvatarsInfo();
    }

    public void OnActionPhaseStarted(object obj)
    {
        // 行动阶段开始，可能需要隐藏一些交互UI，聚焦在角色行动展示上
        Debug.Log("BattleUIManager: OnActionPhaseStarted");
        // 如果在行动阶段需要隐藏或更新UI，可在这里处理
        // battlePanel 可以继续显示角色状态，也可以根据需要更新
    }

    public void OnBattleEnded(object obj)
    {
        // 战斗结束，显示结算UI或隐藏战斗UI
        Debug.Log("BattleUIManager: OnBattleEnded");
        // 可选择隐藏battlePanel或显示结算面板
        battlePanel.Hide();
        battleEndPanel.SetActive(true);
        // 如果有结算面板，可以在这里Show结算面板
    }

    // 如有需要处理显示战斗面板事件
    /*
    private void OnShowBattlePanel(object sender, object args)
    {
        battlePanel.Show();
    }
    */

    public void OnAddBreakersToQueueEvent(object obj)
    {
        breakerQueuePanel.AddBreakerToQueue(obj);
    }

    public void OnRemoveBreakerFromQueueEvent(object obj)
    {
        breakerQueuePanel.RemoveBreakerFromQueue(obj);
    }

    public void OnBreakersQueueCompleted(object obj)
    {
        breakerQueuePanel.ActionStartSetUpQueue();
    }

    public void OnDestinyBroken()
    {

    }

    #endregion
}
