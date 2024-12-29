using System.Collections.Generic;

public class StateMachine
{
    private Dictionary<CharacterStateType, State> states = new Dictionary<CharacterStateType, State>();
    private State currentState;

    // 添加状态到字典
    public void AddState(CharacterStateType type, State state)
    {
        if (!states.ContainsKey(type))
        {
            states[type] = state;
        }
    }

    // 切换状态
    public void ChangeState(CharacterStateType type)
    {
        if (states.ContainsKey(type))
        {
            currentState?.Exit(); // 退出当前状态
            currentState = states[type]; // 设置新状态
            currentState.Enter(); // 进入新状态
        }
    }

    // 更新当前状态
    public void Update()
    {
        currentState?.Update();
    }
}