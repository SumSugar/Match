using UnityEngine;
using DG.Tweening;  // 引入 DOTween
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private void Awake()
    {
        // 确保这个类是单例
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnCharacterGetDamage(int damage)
    {
        StartSlowMotion(0.3f, 1.0f);
    }

    // 触发时缓的协程
    public void StartSlowMotion(float timeScale, float duration)
    {
        StartCoroutine(ApplySlowMotion(timeScale, duration));
    }

    // 协程来管理时缓的开始和结束
    private IEnumerator ApplySlowMotion(float timeScale, float duration)
    {
        // 降低 Time.timeScale 实现时缓效果
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;  // 物理引擎同步

        // 在时缓期间，可以触发其他特效
        yield return new WaitForSecondsRealtime(duration);  // 等待指定的实际时间

        // 恢复正常时间
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}