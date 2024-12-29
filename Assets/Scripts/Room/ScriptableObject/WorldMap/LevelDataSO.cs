using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Map/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    public string levelName; // 关卡名称
    public Sprite levelIcon; // 关卡图标
    public AssetReference sceneToLoad; // 对应的场景
    public string description; // 关卡描述
    public List<CharacterDataSO> enemyCharacters; // 敌人数据列表
}


