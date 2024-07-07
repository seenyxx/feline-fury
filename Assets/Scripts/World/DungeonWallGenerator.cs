using System.Collections.Generic;
using UnityEngine;

public static class DungeonWallGenerator
{
    // Creates all of the walls
    public static bool CreateWalls(HashSet<Vector2Int> floorPositions, DungeonTileMap tilemap, AstarPath pathfinder)
    {
        // Creates walls
        HashSet<Vector2Int> wallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        HashSet<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);

        CreateBasicWalls(tilemap, wallPositions, floorPositions);
        CreateCornerWalls(tilemap, cornerWallPositions, floorPositions);

        return true;
    }

    // Creates all corner walls by looping through all the corner wall positions
    private static void CreateCornerWalls(DungeonTileMap tilemap, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int position in cornerWallPositions)
        {
            string neighboursBinaryType = "";

            foreach (Vector2Int direction in Direction2D.eightDirectionsList)
            {
                Vector2Int neighbourPos = position + direction;
                if (floorPositions.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemap.CreateSingleCornerWall(position, neighboursBinaryType);
        }
    }

    // Creates very basic walls structure
    private static void CreateBasicWalls(DungeonTileMap tilemap, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int position in wallPositions)
        {
            string neighboursBinaryType = "";
            
            foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighbourPos = position + direction;

                if (floorPositions.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemap.CreateSingleBasicWall(position, neighboursBinaryType);
        }
    }

    // Finds walls by testing all cardinal directions
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new();

        foreach (Vector2Int position in floorPositions)
        {
            foreach (Vector2Int direction in directionList)
            {
                Vector2Int neighbourPos = position + direction;

                if (!floorPositions.Contains(neighbourPos))
                {
                    wallPositions.Add(neighbourPos);
                }
            }
        }

        return wallPositions;
    }
}
