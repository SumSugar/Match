using UnityEngine;

public class IdleState : State
{
    public IdleState(CharacterBase character) : base(character) { }

    public override void Enter()
    {
        Debug.Log($"{character.Name} 进入空闲状态");
    }

    public override void Update()
    {
        // 可在此处理空闲状态下的行为（如待机动画等）
    }

    public override void Exit()
    {
        Debug.Log($"{character.Name} 离开空闲状态");
    }
}