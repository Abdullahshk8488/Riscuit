using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

//Creates boxy rooms (like dungeon crawlers)

public class RoomFirstDungeonGenerator : SRWDungeonGenerator
{
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField][Range(0, 10)] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false; //checking if want to do messy rooms or box ones

    //If you want areas where there are rooms, take this
    private HashSet<Vector2Int> _roomArea = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> RoomArea { get { return _roomArea; } }

    protected override void RunProceduralGeneration()
    {
        RoomManager roomManager = RoomManager.Instance;
        if (roomManager != null)
        {
            roomManager.ResetRooms();
        }
        GenerateRooms();

        if (roomManager != null)
        {
            roomManager.SetFirstRoom();
            Debug.Log($"First roon: {roomManager.CurrentRoom}");
        }
    }

    private void GenerateRooms()
    {
        //Create Rooms
        var roomList = ProceduralGenerationAlgorithmns.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        //get hashset of floor positions of rooms
        if (randomWalkRooms)
        {
            floorPos = CreateRandomRooms(roomList);
        }
        else
        {
            floorPos = CreateSimpleRooms(roomList);
        }

        //Take all areas with rooms and put them in a hashset before floor gets joined together
        _roomArea = floorPos;

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        //Create Corridors
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floorPos.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.GenerateWalls(floorPos, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRandomRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> room = new HashSet<Vector2Int>();
        // Iterate through the room list
        // At the end of a single iteration, pass the room and the list of the floor tiles to the room manager
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParams, roomCenter);
            foreach (var pos in roomFloor)
            {
                if (pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset) &&
                    pos.y >= (roomBounds.yMin + offset) && pos.y <= (roomBounds.yMax - offset))
                {
                    room.Add(pos);
                    floor.Add(pos);
                }
            }

            RoomManager roomManager = RoomManager.Instance;
            if (roomManager != null)
            {
                roomManager.CreateRoom(room, GetCenterPoint(room.ToList()));
            }

            room.Clear();
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var curRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(curRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(curRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(curRoomCenter, closest);
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int curRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = curRoomCenter; //starting point
        corridor.Add(pos);

        while (pos.y != destination.y)
        {
            if (destination.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if (destination.y < pos.y)
            {
                pos += Vector2Int.down;
            }
            corridor.Add(pos);
        }

        while (pos.x != destination.x)
        {
            if (destination.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if (destination.x < pos.x)
            {
                pos += Vector2Int.left;
            }
            corridor.Add(pos);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int curRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var pos in roomCenters)
        {
            float curDistance = Vector2.Distance(pos, curRoomCenter);
            if (curDistance < distance)
            {
                distance = curDistance;
                closest = pos;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    roomFloor.Add(pos);
                }
            }
        }
        return roomFloor; //room floor posiitions
    }

    private Vector2 GetCenterPoint(List<Vector2Int> roomNodes)
    {
        Vector2 centerPos = new Vector2();

        Vector2 minPos = roomNodes[0];
        Vector2 maxPos = roomNodes[0];

        for (int i = 0; i < roomNodes.Count; i++)
        {
            minPos.x = Mathf.Min(minPos.x, roomNodes[i].x);
            minPos.y = Mathf.Min(minPos.y, roomNodes[i].y);

            maxPos.x = Mathf.Max(maxPos.x, roomNodes[i].x);
            maxPos.y = Mathf.Max(maxPos.y, roomNodes[i].y);
        }

        centerPos = (minPos + maxPos) * 0.5f;
        return centerPos;
    }
}
