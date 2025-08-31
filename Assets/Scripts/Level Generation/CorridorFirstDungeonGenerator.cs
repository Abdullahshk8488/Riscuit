using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct CorridorStartEnd
{
    public Vector2Int start, end;
}

//Creates jagged rooms (like Minecraft tunnels)

public class CorridorFirstDungeonGenerator : SRWDungeonGenerator
{
    [SerializeField] private int corridorLength = 14, corridorWidth = 2, corridorCount = 5;
    [SerializeField][Range(0.1f, 1f)] private float roomPercent = 0.8f; //percentage of rooms created from all potential room positions

    //If you want areas where there are rooms, take this
    private HashSet<Vector2Int> _roomArea = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> RoomArea { get { return _roomArea; } }
    private List<CorridorStartEnd> _corridorsStartEnd = new();

    protected override void RunProceduralGeneration()
    {
        RoomManager roomManager = RoomManager.Instance;
        if (roomManager != null)
        {
            roomManager.ResetRooms();
        }
        CorridorFirstGeneration();

        if (roomManager != null)
        {
            roomManager.SetTriggerForRooms();
            roomManager.SetFirstRoom();
        }
    }

    private void CorridorFirstGeneration()
    {
        //Create Corridors
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>();

        //Make and get all corridor tiles
        List<List<Vector2Int>> corridors = GenerateCorridors(floorPos, potentialRoomPos);

        //Create Rooms
        //Make rooms along corridors
        HashSet<Vector2Int> roomPos = CreateRooms(potentialRoomPos);


        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPos);
        CreateRoomsAtDeadEnd(deadEnds, roomPos); //roomPos to make sure the 'dead end' isn't just inside a room

        //Take all areas with rooms and put them in a hashset before floor gets joined together
        _roomArea = roomPos;

        //Merge corridors and rooms
        floorPos.UnionWith(roomPos);

        //Increade corridor width
        RoomManager roomManager = RoomManager.Instance;
        for (int i = 0; i < corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorBrush(corridors[i]);
            floorPos.UnionWith(corridors[i]);
        }

        if (roomManager != null)
        {
            roomManager.PlaceTriggers(_corridorsStartEnd);
        }

        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.GenerateWalls(floorPos, tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorBrush(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < corridorWidth; x++)
            {
                for (int y = -1; y < corridorWidth; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y)); //offset
                }
            }
        }
        return newCorridor;
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (Vector2Int pos in deadEnds)
        {
            if (roomFloors.Contains(pos) == false)
            {
                HashSet<Vector2Int> room = RunRandomWalk(randomWalkParams, pos);
                RoomManager roomManager = RoomManager.Instance;
                if (roomManager != null)
                {
                    roomManager.CreateRoom(room, GetCenterPoint(room.ToList()));
                }
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (Vector2Int pos in floorPos)
        {
            //if neighbour only found in one direction, it is a dead end
            int neighboursCount = 0;
            foreach (Vector2Int direction in Direction2D.cardinalDirectionList)
            {
                if (floorPos.Contains(pos + direction))
                {
                    neighboursCount++;
                }
            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(pos);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        HashSet<Vector2Int> roomPos = new HashSet<Vector2Int>();
        int roomAmount = Mathf.RoundToInt(potentialRoomPos.Count * roomPercent);

        //Randomly sort our hashset
        List<Vector2Int> roomsToCreate = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomAmount).ToList(); //GUID Creates a unique ID for each potetntial room positions

        foreach (Vector2Int roomPosition in roomsToCreate)
        {
            HashSet<Vector2Int> roomFloor = RunRandomWalk(randomWalkParams, roomPosition);
            RoomManager roomManager = RoomManager.Instance;
            if (roomManager != null)
            {
                roomManager.CreateRoom(roomFloor, GetCenterPoint(roomFloor.ToList()));
            }
            roomPos.UnionWith(roomFloor);
        }
        return roomPos;
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

    private List<List<Vector2Int>> GenerateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomPos)
    {
        Vector2Int curPos = startPos;
        potentialRoomPos.Add(curPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgorithmns.RandomWalkCorridor(curPos, corridorLength, out Vector2Int endPos);
            CorridorStartEnd corridorStartEnd = new CorridorStartEnd();
            corridorStartEnd.start = curPos;
            corridorStartEnd.end = endPos;
            _corridorsStartEnd.Add(corridorStartEnd);
            
            corridors.Add(corridor);
            curPos = corridor[corridor.Count - 1]; //Set to last position of corridor
            potentialRoomPos.Add(curPos); //add start and each end of corridor
            floorPos.UnionWith(corridor);
        }
        return corridors;
    }
}
