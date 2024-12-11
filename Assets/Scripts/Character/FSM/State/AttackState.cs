using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AttackState : State
{
    private Animator animator;

    public AttackState(CharacterBase character) : base(character)
    {
        animator = character.animator;
    }

    public override void Enter()
    {
        Debug.Log($"{character.characterName} 进入攻击状态");
        character.StartCoroutine(PlayBreaskerActions());
    }

    private IEnumerator PlayBreaskerActions()
    {
        var handBreakers = character.breakerDeck.handBreakerDataList;
        while (handBreakers.Count > 0)
        {

            if (!character.TryGetTarget())
            {
                Debug.Log("没有目标");
                character.CompleteAction();
                yield break;
            }

            var breaker = handBreakers[0];
            //animator.SetTrigger(card.actionTriggerName);
            animator.SetTrigger("attack");
            character.isAttacking = true;
            Debug.Log($"{character.characterName} 正在打出卡牌：{breaker.breakerName}");

            //Debug.Log("当前状态名称: " + animator.GetCurrentAnimatorStateInfo(0).IsName("attack"));
            //Debug.Log("当前动画进度: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f >= 0.6f
                            && !animator.IsInTransition(0)
                            && animator.GetCurrentAnimatorStateInfo(0).IsName("attack"));

            //character.AttackEvent.RaiseEvent(this, this);
            //breaker.ExcuteBreakerEffects(character, character.currentTarget);
            character.breakerDeck.DisBreaker(breaker);

            yield return null; // 等待一帧以确保状态切换生效
            yield return new WaitUntil(() => !animator.IsInTransition(0)); // 等待过渡结束
            yield return new WaitUntil(() =>
                !animator.GetCurrentAnimatorStateInfo(0).IsName("attack") ||
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            character.isAttacking = false;
        }

        character.CompleteAction();
    }

    public override void Exit()
    {
        Debug.Log($"{character.characterName} 离开攻击状态");
    }
}
