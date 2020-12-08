using UnityEngine;
using System.Collections.Generic;

public class DefenseBaseBoard : MonoBehaviour
{
    int wallCount = 0, laserCount = 0, mortarCount = 0, spawnCount = 0, destCount = 0;

    [SerializeField]
    Transform ground = default;

    [SerializeField]
    GameTile tilePrefab = default;

    [SerializeField]
    BoardConfig config = default;

    Vector2Int size;

    GameTile[] tiles;

    List<GameTile> spawnPoints = new List<GameTile>();

    List<GameTileContent> updatingContent = new List<GameTileContent>();

    Queue<GameTile> searchFrontier = new Queue<GameTile>();

    GameTileContentFactory contentFactory;

    public void Initialize(
        Vector2Int size, GameTileContentFactory contentFactory
    )
    {
        this.size = size;
        this.contentFactory = contentFactory;
        ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2(
            (size.x - 1) * 0.5f, (size.y - 1) * 0.5f
        );
        tiles = new GameTile[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = tiles[i] = Instantiate(tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(
                    x - offset.x, 0f, y - offset.y
                );

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, tiles[i - 1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, tiles[i - size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
            }
        }
        Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PrintTiles();
        }
    }

    void PrintTiles()
    {
        int[] boardLogic = GenerateBoardLogic();
        foreach(int i in boardLogic)
        {
            Debug.Log(i);
        }
    }

    public void Clear()
    {
        foreach (GameTile tile in tiles)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
        }
        spawnPoints.Clear();
        updatingContent.Clear();
        ToggleDestination(tiles[tiles.Length / 2]);
        ToggleSpawnPoint(tiles[0]);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < updatingContent.Count; i++)
        {
            updatingContent[i].GameUpdate();
        }
    }

    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
            if (!FindPaths())
            {
                tile.Content =
                    contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }

    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = contentFactory.Get(GameTileContentType.Empty);
            wallCount--;
            FindPaths();
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            if (wallCount < config.maxWallCount)
            {
                tile.Content = contentFactory.Get(GameTileContentType.Wall);
                wallCount++;
                if (!FindPaths())
                {
                    tile.Content = contentFactory.Get(GameTileContentType.Empty);
                    wallCount--;
                    FindPaths();
                }
            }
            else
            {
                Debug.Log("Walls Have been maxed out");
            }
        }
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            if (spawnPoints.Count > 1)
            {
                spawnPoints.Remove(tile);
                spawnCount--;
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            if (spawnCount < config.maxSpawnPoints)
            {
                spawnCount++;
                tile.Content = contentFactory.Get(GameTileContentType.SpawnPoint);
                spawnPoints.Add(tile);
            }
            else
            {
                Debug.Log("Spawn Points Maxed Out");
            }
        }
    }

    public void ToggleTower(GameTile tile, TowerType towerType)
    {
        if (tile.Content.Type == GameTileContentType.Tower)
        {
            updatingContent.Remove(tile.Content);
            if (((Tower)tile.Content).TowerType == towerType)
            {
                if(towerType == TowerType.Laser)
                {
                    laserCount--;
                }
                else
                {
                    mortarCount--;
                }
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            if (towerType == TowerType.Laser)
            {
                if (laserCount < config.maxLaserShooters)
                {
                    tile.Content = contentFactory.Get(towerType);
                    laserCount++;
                    updatingContent.Add(tile.Content);
                }
                else
                {
                    Debug.Log("Laser Shooters Maxed Out");
                }
            }
            else
            {
                if (mortarCount < config.maxMortarShooters)
                {
                    tile.Content = contentFactory.Get(towerType);
                    mortarCount++;
                    updatingContent.Add(tile.Content);
                }
                else
                {
                    Debug.Log("Mortar Shooters Maxed Out");
                }
            }
            if (FindPaths())
            {
                updatingContent.Add(tile.Content);
            }
            else
            {
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = contentFactory.Get(towerType);
            updatingContent.Add(tile.Content);
        }
    }

    public GameTile GetSpawnPoint(int index)
    {
        return spawnPoints[index];
    }

    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1))
        {
            int x = (int)(hit.point.x + size.x * 0.5f);
            int y = (int)(hit.point.z + size.y * 0.5f);
            if (x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                return tiles[x + y * size.x];
            }
        }
        return null;
    }

    bool FindPaths()
    {
        foreach (GameTile tile in tiles)
        {
            if (tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }
        if (searchFrontier.Count == 0)
        {
            return false;
        }

        while (searchFrontier.Count > 0)
        {
            GameTile tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    searchFrontier.Enqueue(tile.GrowPathWest());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (GameTile tile in tiles)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }

        return true;
    }

    public int[] GenerateBoardLogic()
    {
        int[] boardLogic = new int[tiles.Length];
        int i= 0;
        int temp = 0;
        foreach (GameTile t in tiles)
        {
            temp = (int)(t.Content.Type);
            if (temp == 4)
            {
                temp += (int)((Tower)t.Content).TowerType;
            }
            boardLogic[i] = temp;
            i++;
        }
        return boardLogic;
    }
}
