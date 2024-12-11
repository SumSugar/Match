using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BaseMessage
{
    // 消息的类型
    public LogType logType;

    // 消息发送者
    public object sender;

    // 消息的具体内容
    public string content;

    // 消息生成时间戳
    public System.DateTime timestamp;

    // 消息优先级
    public int priority;

    // 是否是紧急消息
    public bool isUrgent;

    // 构造函数
    //public BaseMessage(MessageType type, object sender, string content, int priority = 0, bool isUrgent = false)
    //{
    //    this.messageType = type;
    //    this.sender = sender;
    //    this.content = content;
    //    this.priority = priority;
    //    this.isUrgent = isUrgent;
    //    this.timestamp = System.DateTime.Now;
    //}

}

public class AttackMessage : BaseMessage
{
    CharacterBase character;
    CharacterBase target;
    public AttackMessage(object sender , LogType logType)
    {
        this.sender = sender;
        this.logType = logType;
        character = sender as CharacterBase;
        target = character.currentTarget;
    }
}

public class MapUpdateMessage : BaseMessage
{
    public MapUpdateMessage(string content, LogType logType)
    {
        this.content = content;
        this.logType = logType;
    }
}

public class BattleMessage : BaseMessage
{
    public BattleMessage(string content, LogType logType)
    {
        this.content = content;
        this.logType = logType;
    }
}

public class BaseMessageBox : MonoBehaviour
{
    // 消息的类型
    public BaseMessage entry;

    // 显示消息
    public virtual void DisplayMessage(BaseMessage entry)
    {
        // 默认消息显示，子类可以重写
        
    }

    // 记录消息到日志
    public virtual void LogMessage()
    {
        // 你可以在这里实现更复杂的日志记录
        Debug.Log($"[{entry.timestamp}] {entry.logType}: {entry.content}");
    }
}