using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [Header("UI Elements")]
    public Image levelIcon; // 关卡图标
    public TextMeshProUGUI levelName;  // 关卡名称

    public LevelDataSO levelData; // 当前关卡数据
    public ObjectEventSO SelectLevelEvent;

    /// <summary>
    /// 初始化关卡数据
    /// </summary>
    /// <param name="data">关卡数据</param>
    /// <param name="onClick">点击回调</param>
    public void Initialize(LevelDataSO data)
    {
        levelData = data;

        // 设置 UI
        if (levelIcon != null) levelIcon.sprite = levelData.levelIcon;
        if (levelName != null) levelName.text = levelData.levelName;

        // 绑定点击事件
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public void OnClick()
    {
        SelectLevelEvent.RaiseEvent(levelData, this);
    }
}

