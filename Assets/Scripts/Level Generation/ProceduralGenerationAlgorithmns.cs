using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
                        SplitHorizontally(minWidth, minHeight, roomsQueue, roomsQueue);
                    }
                    else if(room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, roomsQueue);
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
                        SplitVertically(minWidth, minHeight, roomsQueue, roomsQueue);
                    }
                    else if (room.size.y >= minHeight * 2) //Can contain 2 rooms
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, roomsQueue);
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

    private static void SplitVertically(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue1, Queue<BoundsInt> roomsQueue2)
    {
        throw new System.NotImplementedException();
    }

    private static void SplitHorizontally(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue1, Queue<BoundsInt> roomsQueue2)
    {
        throw new System.NotImplementedException();
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
