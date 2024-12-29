using System.Collections;
using System.Collections.Generic;

public class BreakerQueueManager : Singleton<BreakerQueueManager>
{
    public Queue<Breaker> breakerQueue = new Queue<Breaker>();

    public void EnqueueBreaker(Breaker breaker)
    {
        breakerQueue.Enqueue(breaker);
    }

    public void StartProcessing()
    {
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        while (breakerQueue.Count > 0)
        {
            var breaker = breakerQueue.Dequeue();
        }

        // 通知 BattleManager 检查战斗状态
        //BattleManager.Instance.CheckBattleEnd();
    }

}

