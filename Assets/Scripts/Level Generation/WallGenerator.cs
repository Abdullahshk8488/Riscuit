using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WallGenerator
{
    public static void GenerateWalls(HashSet<Vector2Int> floorPos, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPos = FindWallsInDirections(floorPos, Direction2D.cardinalDirectionList);
        HashSet<Vector2Int> cornerWallPos = FindWallsInDirections(floorPos, Direction2D.diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPos, floorPos);
        GenerateCornerWalls(tilemapVisualizer, cornerWallPos, floorPos);
    }

    private static void GenerateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPos, HashSet<Vector2Int> floorPos)
    {
        foreach (var position in cornerWallPos)
        {
            string neighboursBinaryValue = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPos.Contains(neighbourPosition))
                {
                    neighboursBinaryValue += "1";
                }
                else
                {
                    neighboursBinaryValue += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryValue);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPos, HashSet<Vector2Int> floorPos)
    {
        foreach (Vector2Int pos in basicWallPos)
        {
            string neighboursBinaryValue = "";
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neighbourPosition = pos + direction;
                if (floorPos.Contains(neighbourPosition))
                {
                    neighboursBinaryValue += "1";
                }
                else
                {
                    neighboursBinaryValue += "0";
                }
            }

            tilemapVisualizer.PaintSingleBasicWall(pos, neighboursBinaryValue);
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
