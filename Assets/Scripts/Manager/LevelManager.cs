using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool battleEnd = true;
    public List<CharacterBase> aliveEnemyList;
    public List<CharacterBase> aliveAllyList;

    [Header(header: "事件广播")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;
    public StringEventSO battleStartEvent;

    [ContextMenu("Game Start")]
    public void BattleStart()
    {
        battleEnd = false;
        battleStartEvent.RaiseEvent("Battle Start!", this);
    }



    //public void OnRoomLoadedEvent(object obj)
    //{
    //    Room room = obj as Room;
    //    switch (room.roomData.roomType)
    //    {
    //        case RoomType.MinorEnemy:
    //        case RoomType.ELiteEnemy:
    //        case RoomType.Boss:
    //            //playerObj.SetActive(true);
    //            GameStart();
    //            break;
    //        case RoomType.Shop:
    //        case RoomType.Treasure:
    //            //playerObj.SetActive(false);
    //            break;
    //        case RoomType.RestRoom:
    //            //playerObj.SetActive(true);
    //            //playerObj.GetComponent<PlayerAnimation>().SetSleepAnimation();
    //            break;
    //    }
    //}

    public void OnCharacterDeadEvent(object obj)
    {
        CharacterBase charater = obj as CharacterBase;
        if (charater.tag == "Ally" )
        {
            //发出失败通知
            aliveAllyList.Remove(charater);
            if (aliveAllyList.Count == 0)
            {
                StartCoroutine(EventDelayAction(gameOverEvent));
            }
        }

        if (charater.tag == "Enemy")
        {
            aliveEnemyList.Remove(charater);

            if (aliveEnemyList.Count == 0)
            {
                //发出胜利通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
    }
    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null, this);
    }

    public void OnAfterRoomLoadedEvent(object obj)
    {
        //var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //var allies = GameObject.FindGameObjectsWithTag("Ally");

        //foreach (var enemy in enemies)
        //{
        //    aliveEnemyList.Add(enemy.GetComponent<CharacterBase>());
        //}
        //foreach (var ally in allies)
        //{
        //    aliveAllyList.Add(ally.GetComponent<CharacterBase>());
        //}

        //BattleStart();
    }
    //public void OnFindTargetEvent(object obj)
    //{
    //    var currentCharacter = obj as CharacterBase;
    //    //Debug.Log("currentCharacter: " + currentCharacter.name);
    //    if (currentCharacter.tag == "Ally" && aliveEnemyList.Count != 0)
    //    {
    //        currentCharacter.currentTarget = aliveEnemyList[Random.Range(0, aliveEnemyList.Count)];
    //    }


    //    if (currentCharacter.tag == "Enemy" && aliveAllyList.Count != 0)
    //    {
    //        currentCharacter.currentTarget = aliveAllyList[Random.Range(0, aliveAllyList.Count)];
    //    }
    //}

    public void StopTurnBaseSystem(object obj)
    {
        battleEnd = true;
        //playerObj.SetActive(false);
    }

    public void newGame()
    {
       //playerObj.GetComponent<Player>().NewGame();
    }
}
