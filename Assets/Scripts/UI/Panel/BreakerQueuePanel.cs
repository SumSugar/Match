using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BreakerQueuePanel : MonoBehaviour
{
    public GameObject breakerQueueViewPrefab;
    public Transform queueElementsContainer;
    public List<BreakerDataSO> breakerDataQueue;
    //public List<BreakerQueueView> breakerElementObjectQueue;

    public ObjectEventSO RemoveBreakerFromQueueEvent;

    public void OnAddBreakerToQueue(object obj)
    {
        //breakerDataQueue.Clear();
        var breakerList = BreakerDeckManager.Instance.handBreakerObjectList;
        foreach (var breaker in breakerList)
        {
            if(breaker.breakerState == BreakerState.Selected)
                breakerDataQueue.Add(breaker.breakerData);
        }
        DisplayQueue();
    }

    public void OnAddEnemyBreakerToQueue(object obj)
    {
        //breakerDataQueue.Clear();
        var breakerList = BreakerDeckManager.Instance.handBreakerObjectList;
        foreach (var breaker in breakerList)
        {
            if (breaker.owner.tag == "Enemy")
                breakerDataQueue.Add(breaker.breakerData);
        }
        DisplayQueue();
    }

    public void RemoveBreakerFromQueue()
    {
        RemoveBreakerFromQueueEvent.RaiseEvent(this,this);
    }

    public void DisplayQueue()
    {
        foreach (Transform child in queueElementsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var breaker in breakerDataQueue)
        {
            CreateQueueElement(breaker);
        }
    }

    public void CreateQueueElement(BreakerDataSO data)
    {
        GameObject QueueElementInstance = Instantiate(breakerQueueViewPrefab, queueElementsContainer);
        var breakerQueueView = QueueElementInstance.GetComponent<BreakerQueueView>();
        breakerQueueView.SetBreakerData(data);
    }
}
