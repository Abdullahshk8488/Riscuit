using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerationAlgorithmns : MonoBehaviour
{
    //Walk in Many Directions
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        Vector2Int prevPos = startPos;

        for (int i = 0; i < walkLength; i++) 
        {
            Vector2Int newPos = prevPos + Direction2D.GetRandomDirection();
            path.Add(newPos);
            prevPos = newPos;
        }
        return path;
    }

    //Walk in One Direction
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        Vector2Int direction = Direction2D.GetRandomDirection();
        Vector2Int curPos = startPos;
        corridor.Add(curPos);

        for (int i = 0; i < corridorLength; i++)
        {
            curPos += direction;
            corridor.Add(curPos);
        }
        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight) //get area of rooms
    {
        //take a room and split it, then save in a list
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            //Only take rooms that are equal or higher than minimum size
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if(Random.value < 0.5f) //Random.value gives 0 to 1, randomize results between splittling horizontal and vertical
                {
                    if (room.size.y >= minHeight * 2) //Can contain 2 rooms
                    {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }
                    else if(room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minHeight, roomsQueue, room);
                    }
                    else //cannot be split, but can contain a room
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2) //Can contain 2 rooms
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else //cannot be split, but can contain a room
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

//allows getting random direction
public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0,-1), //DOWN
        new Vector2Int(-1,0) //LEFT
    };

    public static Vector2Int GetRandomDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }

    public static Vector2Int GetDirection90From(Vector2Int directionFromCell)
    {
        if (directionFromCell == Vector2Int.up)
        {
            return Vector2Int.right;
        }
        if (directionFromCell == Vector2Int.right)
        {
            return Vector2Int.down;
        }
        if (directionFromCell == Vector2Int.down)
        {
            return Vector2Int.left;
        }
        if (directionFromCell == Vector2Int.left)
        {
            return Vector2Int.up;
        }
        return Vector2Int.zero;
    }
}
