//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public interface ICommand
//{
//     public void Execute();
//}

//public class BreakerActionCommand : ICommand
//{
//    private CharacterBase user;
//    private Breaker breaker;
//    //private List<CharacterBase> possibleTargets;
//    private CharacterBase target;
//    public BreakerActionCommand(CharacterBase user, Breaker breaker, CharacterBase target)
//    {
//        this.user = user;
//        this.breaker = breaker;
//        this.target = target;
//    }

//    public void Execute()
//    {
//        Debug.Log($"{user.characterName} 使用卡牌 {breaker.breakerName}");
//        breaker.ExcuteBreakerEffects(user, target);
//    }
//}
