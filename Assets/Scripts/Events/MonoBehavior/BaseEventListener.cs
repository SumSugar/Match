using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T>: MonoBehaviour
{
    public BaseEventSo<T> eventSO;
    public UnityEvent<T> response;

    private void OnEnable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }

    private void OnDisable()
    {
        eventSO.OnEventRaised -= OnEventRaised;
    }


    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }

}
