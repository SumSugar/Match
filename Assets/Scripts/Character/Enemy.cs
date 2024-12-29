using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public StateMachine stateMachine;//状态机

    protected override void Awake()
    {
        base.Awake();

        isAttacking = false;

        // 添加状态到状态机
        stateMachine = new StateMachine();
        stateMachine.AddState(CharacterStateType.Idle, new IdleState(this));
        stateMachine.AddState(CharacterStateType.Attack, new AttackState(this));


    }

    protected override void Start()
    {
        base.Start();
        stateMachine.ChangeState(CharacterStateType.Idle); // 初始状态为Idle
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.Update();
        //TimeCounter();
    }

    public override void StartAction(Breaker breaker)
    {
        base.StartAction(breaker);

        stateMachine.ChangeState(CharacterStateType.Attack); // 切换到攻击状态
    }

    public override void CompleteAction()
    {
        Debug.Log($"{Name} 动作完成");
        stateMachine.ChangeState(CharacterStateType.Idle); // 返回到Idle状态
        CharacterActionCompleteEvent.RaiseEvent(this, this); // 通知 BattleManager 当前角色已完成行动
    }

}