using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RegionDataSO", menuName = "Map/RegionDataSO")]
public class RegionDataSO : ScriptableObject
{
    public string regionName; // 区域名称
    public Sprite regionIcon; // 区域图标
    public List<LevelDataSO> levels; // 区域内的关卡列表
}
