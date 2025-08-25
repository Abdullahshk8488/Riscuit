using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void GenerateWalls(HashSet<Vector2Int> floorPos, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPos = FindWallsInDirections(floorPos, Direction2D.cardinalDirectionsList);
        foreach (Vector2Int pos in basicWallPos)
        {
            tilemapVisualizer.PaintSingleBasicWall(pos);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPos, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();

        //Find walls in cardinal directions
        foreach (Vector2Int pos in floorPos)
        {
            foreach (Vector2Int direction in directionsList)
            {
                Vector2Int neighbourPos = pos + direction;
                if (floorPos.Contains(neighbourPos) == false)
                {
                    wallPos.Add(neighbourPos);
                }
            }
        }
        return wallPos;
    }
}
