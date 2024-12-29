using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int column;
    public int line;

    private SpriteRenderer spriteRenderer;

    public RoomDataSO roomData;

    public RoomState roomState;
    public List<Vector2Int> linkTo = new(); 

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    private void OnMouseDown()
    {
        //Debug.Log("点击了房间" + roomData.roomType);
        if (roomState == RoomState.Attainable)
            loadRoomEvent.RaiseEvent(this, this);

    }

    /// <summary>
    /// 外部创建房间时调用房间配置
    /// </summary>
    /// <param name="colum"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int colum , int line , RoomDataSO roomData)
    {
        this.column = colum;
        this.line = line;
        this.roomData = roomData;

        spriteRenderer.sprite = roomData.roomIcon;

        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 1f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        }; 
    }
}
