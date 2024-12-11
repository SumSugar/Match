using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public List<GameObject> logBox; // 你的消息UI预设
    //public Transform messageContainer; // 放置消息UI的父容器
    public float messageDisplayTime = 2f; // 每条消息显示的时间
    public PoolMessage poolMessage;

    private List<BaseMessage> messageQueue = new List<BaseMessage>(); // 存储消息预设的队列

    private bool isDisplayingMessage = false;



    private void Awake()
    {
        poolMessage = GetComponent<PoolMessage>();
        poolMessage.initialize();
    }

    // 添加消息预设到队列
    public void AddMessage(BaseMessage entry)
    {
        // 实例化消息预设并设置内容
        //BaseMessage newMessage = Instantiate(messageQueuePrefab, messageContainer);
        messageQueue.Add(entry);
        messageQueue.Sort((a, b) => b.priority.CompareTo(a.priority)); // 高优先级的消息排在前面

        // 添加到消息队列中
       // messageQueue.Enqueue(newMessage);

        if (!isDisplayingMessage)
        {
            DisplayMessages();
        }
    }

    /// <summary>
    /// 监听攻击事件
    /// </summary>
    /// <param name="obj"></param>
    public void OnCharacterAttack(object obj)
    {
        AttackMessage log = new AttackMessage(obj, LogType.Attack);
        messageQueue.Add(log);
        messageQueue.Sort((a, b) => b.priority.CompareTo(a.priority)); // 高优先级的消息排在前面

        if (!isDisplayingMessage)
        {
            DisplayMessages() ;
        }
    }

    /// <summary>
    /// 监听地图更新事件
    /// </summary>
    /// <param name="obj"></param>
    public void OnMapUpdate(string mapContent)
    {
        MapUpdateMessage message = new MapUpdateMessage(mapContent, LogType.System_MapUpdate);
        messageQueue.Add(message);
        messageQueue.Sort((a, b) => b.priority.CompareTo(a.priority)); // 高优先级的消息排在前面

        if (!isDisplayingMessage)
        {
            DisplayMessages();
        }
    }

    public void OnBattleStart(object obj)
    {
        MapUpdateMessage message = new MapUpdateMessage("BattleStart", LogType.System_BattleStart);
        messageQueue.Add(message);
        messageQueue.Sort((a, b) => b.priority.CompareTo(a.priority)); // 高优先级的消息排在前面

        if (!isDisplayingMessage)
        {
            DisplayMessages();
        }
    }

    // 按顺序显示消息
    private void DisplayMessages()
    {
        //isDisplayingMessage = true;

        foreach (var entry in messageQueue)
        {
            if (poolMessage.messagePools.ContainsKey(entry.logType))
            {
                GameObject messageBox = poolMessage.getObjectFromPool(entry.logType);

                messageBox.transform.SetSiblingIndex(gameObject.transform.childCount - 1); // 将其放在最后一个位置

                messageBox.GetComponent<BaseMessageBox>().DisplayMessage(entry);
                messageBox.SetActive(true);
                StartCoroutine(DeactivateMessage(messageBox, entry.logType, 2f));  // 假设特效持续2秒
            }
            else
            {
                Debug.LogWarning("Message type not found: " + entry.logType);
            }
        }
        messageQueue.Clear(); // 处理完后清空队列
        //isDisplayingMessage = false;
    }

    private IEnumerator DeactivateMessage(GameObject messageBox, LogType logType, float delay)
    {
        yield return new WaitForSeconds(delay);
        messageBox.SetActive(false);
        poolMessage.ReturnObjectToPool(logType, messageBox);
    }
}