using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Player/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public List<CharacterDataSO> ownedCharacters; // 玩家拥有的角色
    public int playerLevel; // 玩家等级
    public int gold; // 玩家金币

    // 添加角色
    public void AddCharacter(CharacterDataSO character)
    {
        if (!ownedCharacters.Contains(character))
        {
            ownedCharacters.Add(character);
        }
    }

    // 移除角色
    public void RemoveCharacter(CharacterDataSO character)
    {
        if (ownedCharacters.Contains(character))
        {
            ownedCharacters.Remove(character);
        }
    }
}