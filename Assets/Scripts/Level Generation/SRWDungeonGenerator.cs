using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SRWDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SRW_SO randomWalkParams;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPos = RunRandomWalk(randomWalkParams, startPos);
        tilemapVisualizer.ClearTiles();
        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.GenerateWalls(floorPos, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SRW_SO parameters, Vector2Int pos)
    {
        var curPos = pos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgorithmns.SimpleRandomWalk(curPos, parameters.walkLength);
            floorPos.UnionWith(path); //Add, but no duplicates
            if (parameters.startRandomlyEachIteration)
            {
                curPos = floorPos.ElementAt(Random.Range(0, floorPos.Count)); //Select random position from all floor positions
            }
        }
        return floorPos;
    }
}
