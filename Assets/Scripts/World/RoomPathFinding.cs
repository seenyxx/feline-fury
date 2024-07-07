using UnityEngine;
using System.Collections.Generic;

public static class RoomPathfinding
{
    // Implement A star algorithm for path finding
    public static int FindShortestDistance(Vector2Int currentPos, Vector2Int targetPos, HashSet<Vector2Int> allowedTiles)
    {
        HashSet<Vector2Int> closedSet = new();
        HashSet<Vector2Int> openSet = new() { currentPos };

        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        distances[currentPos] = 0;

        while (openSet.Count > 0)
        {
            Vector2Int current = GetClosestNode(openSet, distances);
            if (current == targetPos)
            {
                // Return the distance to targetPos
                return distances[targetPos];
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in RoomManager.GetAdjacentTiles(current))
            {
                if (!allowedTiles.Contains(neighbor) || closedSet.Contains(neighbor))
                    continue;

                int tentativeDistance = distances[current] + 1; // Assuming each move has a distance of 1

                if (!openSet.Contains(neighbor) || tentativeDistance < distances[neighbor])
                {
                    distances[neighbor] = tentativeDistance;
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return -1;
    }

    // Finds closest node in graph
    private static Vector2Int GetClosestNode(HashSet<Vector2Int> openSet, Dictionary<Vector2Int, int> distances)
    {
        int minDistance = int.MaxValue;
        Vector2Int closestNode = Vector2Int.zero;

        foreach (Vector2Int node in openSet)
        {
            if (distances.ContainsKey(node) && distances[node] < minDistance)
            {
                minDistance = distances[node];
                closestNode = node;
            }
        }

        return closestNode;
    }
}