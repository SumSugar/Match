using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseEventSo<>))]
public class BaseEventSoEditor<T> : Editor
{

    private BaseEventSo<T> baseEventSo;

    private void OnEnable()
    {
        if (baseEventSo == null)
        {
            baseEventSo = target as BaseEventSo<T>;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量:" + GetListeners().Count);
        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());//显示器监听的名称
        }
    }

    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new();

        if (baseEventSo == null || baseEventSo.OnEventRaised == null) 
        { 
            return listeners;
        }

        var sbucribers = baseEventSo.OnEventRaised.GetInvocationList();

        foreach ( var sbucriber in sbucribers ) 
        { 
            var obj = sbucriber.Target as MonoBehaviour;   
            if(!listeners.Contains( obj ))
            {
                listeners.Add( obj );

            }

        }

        return listeners;

    }
}
