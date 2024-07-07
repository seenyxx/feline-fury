using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonTileMap : MonoBehaviour
{
    public Tilemap floorTilemap, wallTilemap, minimapViewTileMap;
    public TileBase minimapTile;

    public List<TileBase> floorTiles, wallTops, wallBottoms, wallSideLefts, wallSideRights,
        wallInnerCornerDownLefts, wallInnerCornerDownRights,
        wallDiagonalCornerDownLefts, wallDiagonalCornerDownRights,
        wallDiagonalCornerUpLefts, wallDiagonalCornerUpRights,
        wallFulls, wallFullBottoms;

    // Creates the floor
    public void CreateFloor(IEnumerable<Vector2Int> floorPositions)
    {
        // Creates floor tiles for all the floor positions
        CreateFloorTiles(floorPositions, floorTilemap, floorTiles);
    }

    // Creates the basic walls
    internal void CreateSingleBasicWall(Vector2Int position, string binaryType)
    {
        // Creates basic wall while testing the positions of empty spaces and wallls around

        // Serialises the surrounding features into a binary format
        int typeAsInt = Convert.ToInt32(binaryType, 2);

        TileBase tile = null;

        if (WallTypes.wallTop.Contains(typeAsInt))
        {
            tile = RandomTile(wallTops);
        }
        else if (WallTypes.wallBottm.Contains(typeAsInt))
        {
            tile = RandomTile(wallBottoms);
        }
        else if (WallTypes.wallSideLeft.Contains(typeAsInt))
        {
            tile = RandomTile(wallSideLefts);
        }
        else if (WallTypes.wallSideRight.Contains(typeAsInt))
        {
            tile = RandomTile(wallSideRights);
        }

        if (tile)
        {
            CreateSingleTile(wallTilemap, tile, position);
            CreateSingleTile(minimapViewTileMap, minimapTile, position);
        }
    }

    // Creates the corner walls
    internal void CreateSingleCornerWall(Vector2Int position, string binaryType)
    {
        // Serialises the surrounding features into a binary format
        int typeAsInt = Convert.ToInt32(binaryType, 2);

        // Creates corner wall

        TileBase tile = null;
        // Debug.Log(typeAsInt);
        if (WallTypes.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = RandomTile(wallInnerCornerDownLefts);
        }
        else if (WallTypes.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = RandomTile(wallInnerCornerDownRights);
        }
        else if (WallTypes.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = RandomTile(wallDiagonalCornerDownLefts);
        }
        else if (WallTypes.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = RandomTile(wallDiagonalCornerDownRights);
        }
        else if (WallTypes.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = RandomTile(wallDiagonalCornerUpLefts);
        }
        else if (WallTypes.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = RandomTile(wallDiagonalCornerUpRights);
        }
        else if (WallTypes.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = RandomTile(wallFulls);
        }
        else if (WallTypes.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = RandomTile(wallFullBottoms);
        }

        if (tile)
        {
            CreateSingleTile(wallTilemap, tile, position);
            CreateSingleTile(minimapViewTileMap, minimapTile, position);
        }
    }

    // Creates floor tiles
    private void CreateFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TileBase> tile)
    {
        // Creates floor tile
        foreach(Vector2Int position in positions)
        {
            CreateSingleTile(tilemap, RandomTile(tile), position);
        }
    }

    // Create a singular tile at a position
    private void CreateSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int) position);
        tilemap.SetTile(tilePosition, tile);
    }

    // Select a random tile
    private TileBase RandomTile(List<TileBase> tiles) {
        // Outputs a random tile from a list
        int random = UnityEngine.Random.Range(0, tiles.Count);
        return tiles[random];
    }
}
