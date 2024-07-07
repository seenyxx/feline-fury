using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Serialisable enemy spawner key value storage for the prefab and chance
[Serializable]
public class EnemySpawners
{
    public GameObject prefab;
    public int probabilityWeighting;
}

public class DungeonGenerator : MonoBehaviour
{
    public AstarPath pathfinder;
    public Transform player;
    public EnemyManager enemyManager;
    public Vector3Int startPosition;
    public DungeonTileMap dungeonTileMap;

    [Header("Room properties")]
    public int minRoomWidth = 10, minRoomHeight = 10;
    [Header("Dungeon properties")]
    public int dungeonWidth = 100, dungeonHeight = 100;

    [Range(0,20)]
    public int offset = 3;

    [Header("Dungeon props")]
    public GameObject crateProp;
    public GameObject torchProp;
    public GameObject barrelProp;
    public GameObject playerPortal;
    public GameObject bossPortal;
    public GameObject fridge;
    public GameObject armsDealer;

    [Header("Enemies")]
    private List<int> evaluatedEnemyList = new();
    public EnemySpawners[] enemySpawners;

    [Header("Minimap")]

    public GameObject spawnMarker;
    public GameObject endMarker;

    [Header("Loading screen")]
    public CanvasGroup loadScreen;
    public float canvasFadeDuration = 0.5f;
    public GamePause pauseFunction;
    private ParticleSystem playerSpawnParticles;
    private Transform playerWeaponHolder;

    private static System.Random rng = new();

    private bool dungeonLoaded = false;
    private bool propsLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        // Evaluate the chances and counts for enemy spawners
        PopulateEnemySpawners();

        // Find the particles and scripts to activate when the game loads
        playerSpawnParticles = player.transform.Find("SpawnParticles").GetComponent<ParticleSystem>();
        playerWeaponHolder = player.transform.Find("WeaponHolder");

        // Generate the dungeon
        StartCoroutine(CreateRooms());
        // Uses A-star to scan a graph for pathfinding
        StartCoroutine(ScanPathfinding());
    }

    // Evaluate the chances and counts for enemy spawners
    private void PopulateEnemySpawners()
    {
        int i = 0;
        foreach (EnemySpawners enemySpawners in enemySpawners)
        {
            for (int j = 0; j < enemySpawners.probabilityWeighting; j++)
            {
                evaluatedEnemyList.Add(i);
            }
            i++;
        }
    }

    // Creates rooms
    private IEnumerator CreateRooms()
    {
        // Start of Dungeon Genreation

        // Conducts Binary Space Partitioning on the area
        List<BoundsInt> roomsList = DungeonGenBSP.BinarySpacePartitioning(new BoundsInt((Vector3Int) startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        // Creates a simple list of floor tiles for each room and combines them into a large list
        HashSet<Vector2Int> floor = CreateSimpleRooms(roomsList);

        // Create a list of room centres
        List<Vector2Int> roomCentres = new();

        foreach (BoundsInt room in roomsList)
        {
            // Populate the room centre list with the list of all the room centres
            roomCentres.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // TODO: implement random generation of enemies and room contents
        // TODO: implement rooms like spawn room, boss room, treasure room etc maybe use Djikstra's algorithm?
        
        // Connect the rooms with paths of tiles
        HashSet<Vector2Int> corridors = ConnectRooms(new(roomCentres));
        
        // Combine the two lists into the floor list
        floor.UnionWith(corridors);

        // Create the floor tiles 
        dungeonTileMap.CreateFloor(floor);

        // Create the walls around the floor tiles
        DungeonWallGenerator.CreateWalls(floor, dungeonTileMap, pathfinder);
        
        // Wait until the end of loading before setting flag
        yield return new WaitForEndOfFrame();
        dungeonLoaded = true;
        // HashSet<Vector2Int> newCorridors = new(corridors);
        // newCorridors.UnionWith(roomCentres);

        // Initiate processing for room types like spawn room, exit room, normal room, etc
        floor = ProcessDungeonRoomTypes(roomCentres, roomsList, floor, corridors);

        // Initiate processing for props on normal rooms
        ProcessDungeonForProps(floor, corridors);
    }

    // Scans the pathfinding of the room with A* after the dungeon has generated completely
    public IEnumerator ScanPathfinding()
    {
        // Makes A-star scan pathfinding
        while (!dungeonLoaded || !propsLoaded)
        {
            yield return null;
        }

        Debug.Log("Astar scanning");

        pathfinder.Scan();

        yield return new WaitForSeconds(5f);

        // loadScreen.enabled = false;
        StartCoroutine(FadeCanvas());

        yield return new WaitForSeconds(1.5f);
        enemyManager.spawnEnabled = true;
        pauseFunction.enabled = true;


        StopCoroutine(ScanPathfinding());
    }

    // Fade the canvas after loading is finished
    private IEnumerator FadeCanvas()
    {
        float startAlpha = 1f;
        float endAlpha = -1f;
        float currentTime = 0f;
        while (currentTime < canvasFadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / canvasFadeDuration);

            loadScreen.alpha = alpha;
            currentTime += Time.deltaTime;

            if (alpha <= 0)
            {
                loadScreen.gameObject.SetActive(false);
                
                yield return new WaitForSeconds(0.5f);
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                Stamina playerStamina = player.GetComponent<Stamina>();
                SpriteRenderer playerSprite = playerMovement.spriteRenderer;


                playerSprite.enabled = true;
                playerMovement.enabled = true;
                playerHealth.enabled = true;
                playerStamina.enabled = true;

                Debug.Log("Game loaded");
                playerSpawnParticles.Play();
                playerWeaponHolder.gameObject.SetActive(true);

                yield break;
            }

            yield return null;
        }
    }
    
    // Gets a random position within the room with an offset of 3
    private Vector2Int RandomPositionWithinBounds(BoundsInt bounds)
    {
        return new Vector2Int(
            UnityEngine.Random.Range(bounds.min.x + 3, bounds.max.x - 3),
            UnityEngine.Random.Range(bounds.min.y + 3, bounds.max.y - 3)
        );
    }

    // Process the dungeon rooms and assign types
    public HashSet<Vector2Int> ProcessDungeonRoomTypes(List<Vector2Int> roomCentres, List<BoundsInt> _roomsList, HashSet<Vector2Int> floor, HashSet<Vector2Int> corridors)
    {
        // Process the dungeon floor tiles for room types
        List<BoundsInt> roomsList = new(_roomsList);

        // Player Spawn Room is selected at random
        BoundsInt playerSpawnRoom = _roomsList[UnityEngine.Random.Range(0, roomsList.Count)];
        Vector2Int playerSpawnRoomCentre = (Vector2Int)Vector3Int.RoundToInt(playerSpawnRoom.center);
        Vector2Int spawnArmsDealer = RandomPositionWithinBounds(playerSpawnRoom);

        // Remove the spawn room from the list of dungeon rooms that will be processed for props
        _roomsList.Remove(playerSpawnRoom);

        // Find the centre of the boss spawn room using a pathfinding algorithm that finds the furthest room
        Vector2Int bossSpawnRoomCentre = RoomManager.FindFurthestRoomCentre(corridors, playerSpawnRoomCentre, roomCentres);
        
        // Creates the new floor after subtracting the spawn room tiles
        HashSet<Vector2Int> newFloor = RoomManager.SubtractBoundsFromTiles(floor, playerSpawnRoom);

        // Remove the boss room from the roomlist
        foreach (BoundsInt room in roomsList)
        {
            if (RoomManager.IsWithinBounds(bossSpawnRoomCentre, room))
            {
                newFloor = RoomManager.SubtractBoundsFromTiles(newFloor, room);
                // Create an arms dealer instance within the boss portal room
                InstantiateDungeonProp(armsDealer, RandomPositionWithinBounds(room), transform.parent);
            }
        }

        // Creates props for the spawn portal and associated mini-map markers
        InstantiateDungeonProp(playerPortal, playerSpawnRoomCentre, transform.parent);
        InstantiateDungeonProp(spawnMarker, playerSpawnRoomCentre, transform.parent);

        // Create an arms dealer in the spawn room
        InstantiateDungeonProp(armsDealer, spawnArmsDealer, transform.parent);

        // Set player position to the spawn room
        player.position = new Vector3(playerSpawnRoomCentre.x, playerSpawnRoomCentre.y, player.position.z);

        // Creates props for the exit portal and associated mini-map markers
        InstantiateDungeonProp(bossPortal, bossSpawnRoomCentre, transform.parent);
        InstantiateDungeonProp(endMarker, bossSpawnRoomCentre, transform.parent);


        // Returns the new processed floor
        return newFloor;
    }

    // Processes the dungeon in order to place objects
    private void ProcessDungeonForProps(HashSet<Vector2Int> floor, HashSet<Vector2Int> corridors)
    {
        // Guarantees a path
        HashSet<Vector2Int> emptyTiles = new(floor);
        HashSet<Vector2Int> edgeTiles = FindEdgeTiles(floor);

        // Remove the edge tiles so no props are generated on the edge of the rooms
        foreach (Vector2Int edge in edgeTiles)
        {
            emptyTiles.Remove(edge);
        }

        // Removes the tiles that are part of the corridors so no props are generated in the corridors
        foreach (Vector2Int corridorTiles in corridors)
        {
            emptyTiles.Remove(corridorTiles);
            edgeTiles.Remove(corridorTiles);
        }

        // Simply assign each prop a parent for organisational purposes
        Transform cratesParent = new GameObject("CrateProps").transform;
        Transform torchesParent = new GameObject("TorchProps").transform;
        Transform enemyParent = new GameObject("Enemies").transform;

        // Loop through each tile and evalute chances and generate props
        foreach (Vector2Int emptyTile in new HashSet<Vector2Int>(emptyTiles))
        {
            if (!emptyTiles.Contains(emptyTile))
            {
                continue;
            }

            int spawnFridge = UnityEngine.Random.Range(0, 300);

            if (spawnFridge == 1)
            {
                InstantiateDungeonProp(fridge, emptyTile, cratesParent);
                emptyTiles.Remove(emptyTile);
                continue;
            }

            int objectType = UnityEngine.Random.Range(0, 100);

            if (objectType >= 1 && objectType <= 4)
            {
                // Create a cluster of props
                List<Vector2Int> filledPositions = InstantiateDungeonPropCluster(crateProp, emptyTile, 3, emptyTiles, cratesParent);
                foreach (Vector2Int filled in filledPositions)
                {
                    emptyTiles.Remove(filled);
                }
            }

            if (objectType == 5 || objectType == 6)
            {
                InstantiateDungeonProp(torchProp, emptyTile, torchesParent);
                emptyTiles.Remove(emptyTile);
            }

            if (objectType >= 7 && objectType <= 13)
            {
                int enemyId = evaluatedEnemyList[UnityEngine.Random.Range(0, evaluatedEnemyList.Count)];

                InstantiateDungeonProp(enemySpawners[enemyId].prefab, emptyTile, enemyParent);
                emptyTiles.Remove(emptyTile);
            } 
        }

        // Set flag
        propsLoaded = true;
    }

    // Creates a dungeon prop cluster
    private List<Vector2Int> InstantiateDungeonPropCluster(GameObject gameObject, Vector2Int position, int max, HashSet<Vector2Int> placeableTiles, Transform parent)
    {
        // Creates a randomised cluster of props
        InstantiateDungeonProp(gameObject, position, parent);

        int propCount = 1;
        List<Vector2Int> propPlacedPositions = new()
        {
            position
        };

        max = UnityEngine.Random.Range(1, max + 1);

        List<Vector2Int> directions = new(Direction2D.cardinalDirectionsList);

        foreach (Vector2Int dir in directions.OrderBy(x => rng.Next()).ToList())
        {
            Vector2Int testPos = position + dir;
            if (placeableTiles.Contains(testPos))
            {
                InstantiateDungeonProp(gameObject, testPos, parent);
                propPlacedPositions.Add(testPos);
                propCount++;
            }

            if (propCount >= max)
            {
                break;
            }
        }

        return propPlacedPositions;
    }


    // Places a singular object
    private void InstantiateDungeonProp(GameObject gameObject, Vector2Int position, Transform parent)
    {
        // Creates a dungeon prop with the necessary x and y value offset to centre a gameobject on a tile
        Instantiate(gameObject, new Vector3(position.x + 0.5f, position.y + 0.5f, -2f), Quaternion.identity, parent);
    }

    // Finds the edges of the room
    private HashSet<Vector2Int> FindEdgeTiles(HashSet<Vector2Int> tiles)
    {
        // Finds the edge  tiles of a room
        HashSet<Vector2Int> edgeTiles = new();

        foreach (Vector2Int tile in tiles)
        {
            foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighbourPos = tile + direction;

                if (!tiles.Contains(neighbourPos))
                {
                    edgeTiles.Add(tile);
                    break;
                }
            }
        }

        return edgeTiles;
    }

    // Connects the rooms with corridors
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCentres)
    {
        // Draws connecting corridor paths between all the room centres
        HashSet<Vector2Int> corridors = new();
        Vector2Int currentRoomCentre = roomCentres[UnityEngine.Random.Range(0, roomCentres.Count)];

        roomCentres.Remove(currentRoomCentre);

        while (roomCentres.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCentre, roomCentres);
            roomCentres.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCentre, closest);
            currentRoomCentre = closest;

            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    // Creates each individual corridors
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCentre, Vector2Int destination)
    {
        // Creates a straight line path
        HashSet<Vector2Int> corridor = new();
        Vector2Int position = currentRoomCentre;

        corridor.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }

            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            
            corridor.Add(position);
        }

        return corridor;
    }

    // Method to find closest point
    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCentre, List<Vector2Int> roomCentres)
    {
        // Finds closest point to
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (Vector2Int position in roomCentres)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCentre);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }

        return closest;
    }

    // Creates a simple barebones room
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        // Creates a simple room with floors
        HashSet<Vector2Int> floor = new();

        foreach(BoundsInt room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int) room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}