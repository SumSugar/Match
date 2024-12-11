using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapPanel : MonoBehaviour
{
    public GameObject levelContianer; // 关卡容器
    public Transform levelParent; // 关卡按钮的父物体
    public GameObject levelButtonPrefab; // 关卡按钮的预制体
    public ObjectEventSO regionEvent; // 区域事件
    private List<RegionDataSO> regionList = new List<RegionDataSO>();
    private Region activeRegion;
    /// <summary>
    /// 显示关卡列表
    /// </summary>
    /// <param name="regionData">选中的区域数据</param>

    public void Start()
    {
        Initialize();
    }
    public void DisplayLevels(object obj)
    {
        Region region = obj as Region;
        // 清空关卡列表
        foreach (Transform child in levelParent)
        {
            Destroy(child.gameObject);
        }

        // 动态生成关卡按钮
        foreach (var level in region.regionData.levels)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, levelParent);
            Level levelScript = levelButton.GetComponent<Level>();

            if (levelScript != null)
            {
                levelScript.Initialize(level);
            }
        }

        // 显示关卡面板
        levelContianer.SetActive(true);
    }

    public void Initialize()
    {
        Addressables.LoadAssetsAsync<RegionDataSO>("Region", null).Completed += OnAllAssetsLoaded;
    }

    void OnAllAssetsLoaded(AsyncOperationHandle<IList<RegionDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<RegionDataSO> regionDatas = handle.Result;
            foreach (var regionData in regionDatas)
            {
                regionList.Add(regionData);
                Debug.Log($"Loaded Region: {regionData.regionName}");
            }
            Debug.Log("All Regions loaded successfully!");
            
        }
        else
        {
            Debug.LogError("Failed to load assets!");
        }
    }

}
