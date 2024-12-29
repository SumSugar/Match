using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public CinemachineCamera overviewVCam;
    public CinemachineCamera characterFocusVCam;
    //public CinemachineCamera skillVCam;
    public CinemachineCamera impactVCam;
    //public CinemachineCamera eventVCam;

    private CinemachineBrain brain;
    private CinemachineCamera currentActiveCamera;


    private void Start()
    {
        SwitchToOverview(null);
    }

    private void OnEnable()
    {
        // Subscribing to events
        // BattleManager.OnTurnStart += SwitchToOverview;
        // BattleManager.OnCharacterActionStart += SwitchToCharacterFocus;
        // BattleManager.OnImpact += SwitchToImpact;
        // BattleManager.OnSpecialEvent += SwitchToEvent;
    }

    private void OnDisable()
    {
        // Unsubscribing from events
        // BattleManager.OnTurnStart -= SwitchToOverview;
        // BattleManager.OnCharacterActionStart -= SwitchToCharacterFocus;
        // BattleManager.OnImpact -= SwitchToImpact;
        // BattleManager.OnSpecialEvent -= SwitchToEvent;
    }

    public void SwitchToOverview(object obj)
    {
        SetActiveCamera(overviewVCam);
    }

    public void SwitchToCharacterFocus(object obj)
    {
        CharacterBase character = obj as CharacterBase;
        Transform actor = character.vfxTransform;

        characterFocusVCam.Follow = actor;

        var positionComposer = characterFocusVCam.GetComponent<CinemachinePositionComposer>();

        // 获取目标与摄像机之间的x距离
        float distance = Mathf.Abs(actor.position.x - overviewVCam.transform.position.x);
        float direction = Mathf.Sign(overviewVCam.transform.position.x - actor.position.x );

        // 使用 InverseLerp 计算插值比例 t
        float t = Mathf.InverseLerp(0, 10f, distance);
        Debug.Log("t: " + t);
        // 使用 Lerp 平滑地计算新的 X 偏移量
        float newXOffset = Mathf.Lerp(0, 8f, t);

        // 如果偏移量小于 0.1，则设置为 0
        if (Mathf.Abs(newXOffset) < 1f)
        {
            newXOffset = 0f;
        }

        newXOffset *= direction;
        positionComposer.TargetOffset.x = newXOffset;

        SetActiveCamera(characterFocusVCam);

        //SwitchToOverviewAfterDelay(1.0f);
    }

    public void SwitchToImpact(object obj)
    {
        CharacterBase character = obj as CharacterBase;
        Transform target = character.vfxTransform;

        impactVCam.Follow = target; // 受击镜头一般不跟随
        //impactVCam.LookAt = null;

        SetActiveCamera(impactVCam);

        // Trigger camera shake
        TriggerCameraShake(impactVCam, 0.3f, 5f, 2f);

        // Add flash effect
        //StartCoroutine(TriggerFlash(Color.white, 0.1f));

        SwitchToOverviewAfterDelay(0.65f);
    }

    private void SwitchToOverviewAfterDelay(float delay)
    {
        StartCoroutine(SwitchBackToOverview(delay));
    }

    private IEnumerator SwitchBackToOverview(float delay)
    {
        yield return new WaitForSeconds(delay);
        SwitchToOverview(null);
    }

    //public void SwitchToEvent()
    //{
    //    ActivateCamera(eventVCam);

    //    // Trigger dolly animation if applicable
    //    TriggerDollyAnimation(eventVCam, 3f);
    //}


    private void SetActiveCamera(CinemachineCamera vCam)
    {
        if (currentActiveCamera != null)
        {
            currentActiveCamera.Priority = 0; // 关闭当前激活的镜头
        }

        currentActiveCamera = vCam;
        currentActiveCamera.Priority = 10; // 激活新镜头
    }


    private void TriggerCameraShake(CinemachineCamera vCam, float duration, float amplitude, float frequency)
    {
        var noise = vCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.AmplitudeGain = amplitude;
            noise.FrequencyGain = frequency;
            StartCoroutine(ResetCameraShake(noise, duration));
        }
    }

    private IEnumerator ResetCameraShake(CinemachineBasicMultiChannelPerlin noise, float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }

    private IEnumerator TriggerFlash(Color color, float duration)
    {
        var flashUI = GameObject.Find("FlashUI")?.GetComponent<UnityEngine.UI.Image>();
        if (flashUI != null)
        {
            flashUI.color = color;
            flashUI.enabled = true;

            yield return new WaitForSeconds(duration);

            flashUI.enabled = false;
        }
    }

    //private void TriggerDollyAnimation(CinemachineCamera vCam, float duration)
    //{
    //    var dollyCart = vCam.GetComponentInChildren<CinemachineDollyCart>();
    //    if (dollyCart != null)
    //    {
    //        StartCoroutine(AnimateDolly(dollyCart, duration));
    //    }
    //}

    //private IEnumerator AnimateDolly(CinemachineDollyCart dollyCart, float duration)
    //{
    //    float startPosition = dollyCart.m_Position;
    //    float time = 0f;

    //    while (time < duration)
    //    {
    //        dollyCart.m_Position = Mathf.Lerp(startPosition, 1f, time / duration);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    dollyCart.m_Position = 0; // Reset to the start
    //}
}

