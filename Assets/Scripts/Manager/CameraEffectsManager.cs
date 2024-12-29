using UnityEngine;
using DG.Tweening; // 引入 DOTween 命名空间

public class CameraEffectsManager : Singleton<CameraEffectsManager>
{
    public Camera mainCamera;
    private float originalZoom;
    private Vector3 originalRotation;
    private Vector3 originalPosition;  // 摄像机的初始位置

    void Start()
    {
        // 获取初始的摄像机参数
        originalZoom = mainCamera.fieldOfView;
        originalRotation = mainCamera.transform.eulerAngles;
        originalPosition = mainCamera.transform.position;
    }

    public void OnCharacterHit(object obj)
    {
        CharacterBase character = obj as CharacterBase;

        TriggerSlowMotion(0.5f, 1.0f);
        TriggerShake(0.1f, 1.0f);
        //TriggerZoomToCharacter(character.transform, 5f, 1.0f);
    }

    public void OnCharacterDeath(object obj)
    {
        CharacterBase character = obj as CharacterBase;

        TriggerSlowMotion(0.1f, 1.0f);
        TriggerZoomToCharacter(character.vfxTransform.transform , 10f, 1.0f);
        TriggerShake(1.0f, 1.0f);

    }

    // 拉近到角色的效果，结合位置和缩放
    public void TriggerZoomToCharacter(Transform characterTransform, float zoomLevel, float zoomDuration)
    {
        // 终止之前的缩放动画，避免冲突
        DOTween.Kill("ZoomMove");

        // 恢复摄像机到初始位置
        //mainCamera.transform.position = originalPosition;

        // 拉近到角色位置
        Vector3 zoomPosition = new Vector3(characterTransform.position.x, characterTransform.position.y, originalPosition.z);

        Sequence cameraSequence = DOTween.Sequence();
        cameraSequence.Append(mainCamera.transform.DOMove(zoomPosition, zoomDuration)
            .SetId("ZoomMove")
            .SetUpdate(true));
        cameraSequence.Join(mainCamera.transform.DOLookAt(characterTransform.position, zoomDuration)
            .SetId("ZoomLookAt")
            .SetUpdate(true));
        cameraSequence.Join(mainCamera.DOFieldOfView(zoomLevel, zoomDuration)
            .SetId("ZoomScale")
            .SetUpdate(true));

        cameraSequence.OnComplete(() =>
        {
            // 缩放回默认的初始值
            mainCamera.DOFieldOfView(originalZoom, zoomDuration).SetId("ZoomMove").SetUpdate(true);
            mainCamera.transform.DOMove(originalPosition, zoomDuration).SetId("ZoomMove").SetUpdate(true); // 回到初始位置
            mainCamera.transform.DORotate(originalRotation, zoomDuration).SetId("ZoomMove").SetUpdate(true); // 回到初始位置
        });
    }

    // 触发镜头震动效果
    public void TriggerShake(float intensity, float duration)
    {
        // 终止之前的震动动画，避免冲突
        //DOTween.Kill("Shake");

        // 震动效果不受时缓影响
        mainCamera.transform.DOShakePosition(duration, intensity).SetId("Shake");
    }

    // 触发 Zoom Punch 效果（镜头快速拉近再回弹）
    public void TriggerZoomPunch(float punchAmount, float duration)
    {
        DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, originalZoom - punchAmount, duration / 2)
            .OnComplete(() => {
                DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, originalZoom, duration / 2);
            });
    }


    // 触发慢动作效果
    public void TriggerSlowMotion(float timeScale, float duration)
    {
        // 确保之前的时缓动画被终止，防止多个时缓叠加
        DOTween.Kill("SlowMotionTween");

        // 降低 Time.timeScale 实现时缓效果
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, 0.1f)
            .SetId("SlowMotionTween")  // 设置唯一的ID，便于控制时缓动画
            .OnComplete(() =>
            {
                // 持续一段时间后，恢复正常时间流速
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.1f).SetId("SlowMotionTween");
            });

        // 使用额外的延迟来确保时缓的总持续时间
        DOVirtual.DelayedCall(duration, () =>
        {
            // 在时缓结束后，确保 `Time.timeScale` 恢复正常
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.1f).SetId("SlowMotionTween");
        }).SetId("SlowMotionTween");
    }

    // 触发侧向震动
    public void TriggerLateralShake(float intensity, float duration)
    {
        mainCamera.transform.DOMoveX(mainCamera.transform.position.x + intensity, duration / 2).SetLoops(2, LoopType.Yoyo);
    }
}