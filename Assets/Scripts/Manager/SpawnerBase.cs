using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    public Transform playerSpawnPoint; // 生成点
    public Transform enemySpawnPoint;

    public Vector3 playerSpawnOffset = new Vector3(-1, 0, 0); // 玩家生成的偏移
    public Vector3 enemySpawnOffset = new Vector3(1, 0, 0);   // 敌人生成的偏移

    public void OnUnitSpawn(object obj)
    {
        LevelDataSO levelData = obj as LevelDataSO;
        Debug.Log(levelData.name);
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
            Vector3 spawnPosition = spawnerPoint.position + spawnOffset * i;
            // 生成角色
            GameObject instance = Instantiate(characterDatas[i].characterPrefab, transform);

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

            BattleManager.Instance.AddUnits(character);
            BreakerDeckManager.Instance.AddDeck(character.breakerDeck);

            Debug.Log($"Spawned: {characterDatas[i].characterName} - Index: {i}");
        }
    }

    private void SpawnCharacter(Transform spawnerPoint, Vector3 spawnOffset)
    {

    }
}