using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header(header:"地图配置表")]
    public MapConfigSO mapConfig;

    [Header(header:"地图布局")]
    public MapLayoutSO mapLayout;

    [Header(header: "预制体")]
    public Room roomPrefab;

    public LineRenderer linePrefab;


    private float screenHeight;

    private float screenWidth;

    private float columWidth;

    private Vector3 generatePoint;

    public float border;

    private List<Room> rooms = new();
    private List<LineRenderer> lines = new();

    public List<RoomDataSO> RoomDataList = new();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();

    [Header(header: "事件广播")]
    public StringEventSO MapUpdateEvent;

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight *Camera.main.aspect;

        columWidth = screenWidth / (mapConfig.roomBlueprints.Count);

        foreach (var roomData in RoomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }

     
    }

    //private void Start()
    //{
    //    CreateMap();
    //}

    private void OnEnable()
    {
        if(mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        else
        {
            CreateMap();
        }
        MapUpdateEvent.RaiseEvent("Map Update!" , this);
    }


    public void CreateMap()
    {
        //创建前一列房间列表
        List<Room> previousColumnRooms = new();

        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];

            var amount = UnityEngine.Random.Range(blueprint.min , blueprint.max);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);

            generatePoint = new Vector3(-screenWidth / 2 + border + columWidth * column, startHeight, 0);

            var newPosition = generatePoint;

            //创建当前列房间列表
            List<Room> currentColumnRoom = new();

            var roomGapY = screenHeight / (amount + 1);

            //循环当前列的所有房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                newPosition.y = startHeight - roomGapY * i;
                //判断为最后一列Boss房间
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }
                else if (column != 0)
                {
                    newPosition.x = generatePoint.x + UnityEngine.Random.Range(-border / 2, border / 2);
                }

                newPosition.y = startHeight - roomGapY * (i);
                //生成房间
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity , transform);
                RoomType newType = GetRoomType(mapConfig.roomBlueprints[column].roomType);
                if(column == 0)
                {
                    room.roomState = RoomState.Attainable;

                }
                else
                {
                    room.roomState = RoomState.Locked;
                }

                room.SetupRoom(column , i ,GetRoomData(newType) );

                rooms.Add(room);
                currentColumnRoom.Add(room);
            }

            //判断当前列是否为第一列，如果不是则连接上一列
            if(previousColumnRooms.Count > 0)
            {
                //创建两个列表的房间连线
                CreateConnection(previousColumnRooms, currentColumnRoom);
            }

            previousColumnRooms = currentColumnRoom;
        }

        SaveMap();
    }

    private void CreateConnection(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms= new ();
        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2 , false);
            connectedColumn2Rooms.Add(targetRoom);
        }

        foreach(var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1 , true);
            }
        }
    }
   
    private Room ConnectToRandomRoom(Room room, List<Room> column2 , bool check)
    {
        Room targetRoom;
        targetRoom = column2[UnityEngine.Random.Range(minInclusive:0 , column2.Count)];

        if(check)
        {
            targetRoom.linkTo.Add(new(room.column, room.line));
        }
        else
        {
            room.linkTo.Add(new(targetRoom.column, targetRoom.line));
        }

        //创建房间之间连线
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(index:0 , room.transform.position);
        line.SetPosition(index: 1, targetRoom.transform.position);

        lines.Add(line);

        return targetRoom;
    }

    [ContextMenu(itemName:"ReGenerateRoom")]
    //从新生成地图
    public void ReGenerateRoom()
    {
        foreach(var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach(var line in lines)
        {
            Destroy(line.gameObject);
        }

        rooms.Clear();
        lines.Clear(); 

        CreateMap();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');

        string randomOption = options[UnityEngine.Random.Range(0, options.Length)];

        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), randomOption);

        return roomType;
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();
        //添加所有已经生成的房间
        for(int i = 0; i< rooms.Count; i++) 
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].column,
                line = rooms[i].line,   
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo,
            };

            mapLayout.mapRoomDataList.Add(room);
        }

        mapLayout.linePositionsList = new();
        //添加所有连线
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };

            mapLayout.linePositionsList.Add(line);
        }

    }

    private void LoadMap()
    {
        //读取房间数据生成房间
        for(int i = 0;i< mapLayout.mapRoomDataList.Count;i++) 
        {
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posX, mapLayout.mapRoomDataList[i].posY);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.roomState = mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].column, mapLayout.mapRoomDataList[i].line, mapLayout.mapRoomDataList[i].roomData);
            newRoom.linkTo = mapLayout.mapRoomDataList[i].linkTo;  

            rooms.Add(newRoom);
        }

        //读取连线
        for(int i = 0;i< mapLayout.linePositionsList.Count ; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionsList[i].startPos.ToVector3());
            line.SetPosition(1, mapLayout.linePositionsList[i].endPos.ToVector3());

            lines.Add(line);
        }
    }
}
