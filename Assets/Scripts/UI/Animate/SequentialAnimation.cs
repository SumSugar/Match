using UnityEngine;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;

public class SequentialAnimation : Singleton<SequentialAnimation>
{
    public float scaleDuration = 0.5f; // 弹出动画持续时间
    public float delayBetweenAnimations = 0.2f; // 动画之间的延迟

    public void PlaySequentialAnimations(List<Transform> icons)
    {
        // 创建动画序列
        Sequence sequence = DOTween.Sequence();

        foreach (Transform icon in icons)
        {
            // 初始缩放设置为 0
            icon.localScale = Vector3.zero;

            // 添加动画：缩放到 1
            sequence.Append(icon.DOScale(1f, scaleDuration).SetEase(Ease.InOutQuad)) // 使用平滑曲线
                    .AppendInterval(delayBetweenAnimations); // 添加延迟
        }
    }
}
