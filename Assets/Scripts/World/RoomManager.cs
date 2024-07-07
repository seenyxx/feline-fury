using UnityEngine;
using System.Collections.Generic;

public static class RoomManager
{
    // Finds adjacent tiles
    public static List<Vector2Int> GetAdjacentTiles(Vector2Int pos)
    {
        List<Vector2Int> adjacentTiles = new();
        foreach (Vector2Int dir in Direction2D.cardinalDirectionsList)
        {
            adjacentTiles.Add(pos + dir);
        }

        return adjacentTiles;
    }

    // Removes tiles within an area from a larger list of tiles
    public static HashSet<Vector2Int> SubtractBoundsFromTiles(HashSet<Vector2Int> tiles, BoundsInt bounds)
    {
        HashSet<Vector2Int> newTiles = new();


        foreach (Vector2Int tile in tiles)
        {
            if (!IsWithinBounds(tile, bounds))
            {
                newTiles.Add(tile);
            }
        }

        return newTiles;
    }

    // Checks whether a certain point is within an area
    public static bool IsWithinBounds(Vector2Int point, BoundsInt bounds)
    {
        return point.x >= bounds.min.x && point.x <= bounds.max.x && point.y >= bounds.min.y && point.y <= bounds.max.y;
    }

    // Pathfinding algorithm to find a far away room
    public static Vector2Int FindFurthestRoomCentre(HashSet<Vector2Int> corridorTiles, Vector2Int currentPos, List<Vector2Int> roomCentres)
    {
        HashSet<Vector2Int> allowedTiles = new(corridorTiles);

        allowedTiles.UnionWith(roomCentres);

        int maxDistance = int.MinValue;
        Vector2Int furthestRoomCentre = Vector2Int.zero;
        
        foreach (var roomCentre in roomCentres)
        {

            int dist = RoomPathfinding.FindShortestDistance(currentPos, roomCentre, allowedTiles);
            Debug.Log("Distance from: " + currentPos + "to: " + roomCentre + "is " + dist);

            if (dist > maxDistance)
            {
                maxDistance = dist;
                furthestRoomCentre = roomCentre;
            }
        }

        return furthestRoomCentre;
    }

}