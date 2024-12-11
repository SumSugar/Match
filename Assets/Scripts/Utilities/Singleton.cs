using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogWarning($"多个 {typeof(T).Name} 实例存在，销毁重复的实例。");
            Destroy(gameObject); // 销毁重复的实例，确保只有一个实例
            return;
        }
    }
}