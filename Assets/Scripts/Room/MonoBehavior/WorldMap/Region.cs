using UnityEngine;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [Header("Region Data")]
    public RegionDataSO regionData; // 手动拖入的区域数据

    [Header("UI Elements")]
    public Image regionIcon; // 区域图标
    public Text regionName;  // 区域名称

    [Header(header:"事件广播")]
    public ObjectEventSO SelectRegionEvent; // 选择区域事件

    /// <summary>
    /// 初始化区域
    /// </summary>
    /// <param name="onClick">点击回调</param>
    public void Initialize()
    {
        if (regionData == null)
        {
            Debug.LogError($"{name} does not have RegionDataSO assigned!");
            return;
        }

        // 设置 UI 显示
        if (regionIcon != null) regionIcon.sprite = regionData.regionIcon;
        if (regionName != null) regionName.text = regionData.regionName;
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public void OnClick()
    {
        SelectRegionEvent.RaiseEvent(this,this);
    }
}



