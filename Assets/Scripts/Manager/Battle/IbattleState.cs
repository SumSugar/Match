using System.Collections.Generic;
using UnityEngine;


public interface IState<T>
{
    void Enter(T owner); // 进入状态时调用
    void Update(); // 每帧更新逻辑
    void Exit(); // 离开状态时调用

}

public abstract class StateMachine<T, TEnum> where TEnum : System.Enum
{
    protected T owner;
    protected IState<T> currentState;
    protected Dictionary<TEnum, IState<T>> states;

    public StateMachine(T owner)
    {
        this.owner = owner;
        states = new Dictionary<TEnum, IState<T>>();
        InitializeStates();
    }

    // 子类实现：初始化状态
    protected abstract void InitializeStates();

    // 添加状态
    public void AddState(TEnum stateType, IState<T> state)
    {
        if (!states.ContainsKey(stateType))
        {
            states.Add(stateType, state);
        }
    }

    // 切换状态
    public void ChangeState(TEnum stateType)
    {
        if (states.ContainsKey(stateType))
        {
            currentState?.Exit();
            currentState = states[stateType];
            currentState.Enter(owner);
        }
        else
        {
            Debug.LogError($"State '{stateType}' not found in StateMachine.");
        }
    }

    // 更新当前状态
    public void Update()
    {
        currentState?.Update();
    }

    public IState<T> GetCurrentState()
    {
        return currentState;
    }
}

public class BattleStateMachine : StateMachine<BattleManager, BattleStateType>
{
    public BattleStateMachine(BattleManager owner) : base(owner) { }

    protected override void InitializeStates()
    {
        // 注册状态
        AddState(BattleStateType.PreBattle, new PreBattleState());
        AddState(BattleStateType.CardInit, new CardInitState());
        AddState(BattleStateType.CardDraw, new CardDrawState());
        AddState(BattleStateType.PreparationPhase, new PreparationPhaseState());
        AddState(BattleStateType.ActionPhase, new ActionPhaseState());
        AddState(BattleStateType.BattleEnd, new BattleEndState());

        // 设置初始状态
        //ChangeState(BattleStateType.PreBattle);
    }
}




