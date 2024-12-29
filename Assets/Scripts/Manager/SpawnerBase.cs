using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    public Transform playerSpawnPoint; // 生成点
    public Transform enemySpawnPoint;

    public Vector3 playerSpawnOffset = new Vector3(-1, 0, 3); // 玩家生成的偏移
    public Vector3 enemySpawnOffset = new Vector3(1, 0, 3);   // 敌人生成的偏移
    public ObjectEventSO UnitSpawnCompeletedEvent;
    public void SpawnCharacters(object obj)
    {
        LevelDataSO levelData = obj as LevelDataSO;
        var enemys =  levelData.enemyCharacters;
        var allys = GameManager.Instance.GetOwnerCharacterData();


        InitSpawnCharacters(allys, "Ally", playerSpawnPoint, playerSpawnOffset);
        InitSpawnCharacters(enemys, "Enemy", enemySpawnPoint, enemySpawnOffset);

    }

    void InitSpawnCharacters(List<CharacterDataSO> characterDatas, string tag, Transform spawnerPoint, Vector3 spawnOffset)
    {
        for (int i = 0; i < characterDatas.Count; i++)
        {

            // 每个角色在生成时稍微错开位置
            float zOffset = (i % 2 == 0) ? 0 : -3f;

            Vector3 spawnPosition = spawnerPoint.position + spawnOffset * i + new Vector3(0, 0, zOffset);

            // 生成角色
            GameObject instance = Instantiate(characterDatas[i].prefab, transform);

            instance.transform.position = spawnPosition;
            instance.transform.rotation = transform.rotation;
            instance.tag = tag;
            if(tag == "Ally")
            {
                Vector3 currentScale = instance.transform.localScale;
                currentScale.x = -currentScale.x;
                instance.transform.localScale = currentScale;
            }
            //Instantiate(characterPrefabs[i], spawnPosition, transform.rotation);

            CharacterBase character = instance.GetComponent<CharacterBase>();
            character.Initialize(characterDatas[i]);

            BattleManager.Instance.AddUnit(character);
            BreakerDeckManager.Instance.AddDeck(character.breakerDeck);

        }
    }
}