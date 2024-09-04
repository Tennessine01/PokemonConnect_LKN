using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public int Rows = 12;
    public int Cols = 8;
    public Tile tilePrefab;
    public float tileSpacing = 0.62f;
    public List<Tile> listTiles = new();
    public int numberOfTiles = 0;
    public Tile[,] grid;
    private Tile selectedStartTile = null;
    private Tile selectedEndTile = null;
    public LineRenderer lineRenderer;
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public TileSO tileSO; 
    private Level currentlevel;

    int startRow = 1;  
    int endRow => Rows - 1;  
    int startCol = 1; 
    int endCol => Cols - 1;    
    public Coroutine removeTileCoroutine;
    public Coroutine hideLineAfterDelayCoroutine;
    public bool IsCanPlay => GameManager.Ins.IsState(GameState.GamePlay);
    //public bool IsCanClick;
    public void OnInit(Level level)
    {
        //IsCanClick = true;
        numberOfTiles = 0;
        currentlevel = level;
        InitializeGrid();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    public void InitializeGrid()
    {
        grid = new Tile[Rows, Cols];

        float startX = -(Cols / 2f) * tileSpacing + tileSpacing / 2;
        float startY = -(Rows / 2f) * tileSpacing + tileSpacing / 2;

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Vector3 position = new Vector3(startX + j * tileSpacing, startY + i * tileSpacing, 0);
                Tile tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObject.transform.parent = transform;

                tileObject.Type = TileType.Empty;
                tileObject.Position = new Vector2Int(i, j);
                grid[i, j] = tileObject;
            }
        }
        AssignTileData();
    }
    public void AssignTileData()
    {
        List<SpecialTile> specialTiles = currentlevel.specialTiles;
        List<TileData> tileDataList = tileSO.listTile;
        List<TileData> pairedTiles = new List<TileData>();

        int totalTiles = currentlevel.fillablePositions.Count;
        int totalPairs = totalTiles / 2;

        // Get a number of tile types based on the current level
        int numberOfTileTypes = Mathf.Min(currentlevel.numberOfTileTypes, tileDataList.Count);

        // Randomly select tile types
        List<TileData> selectedTileTypes = tileDataList
            .Take(tileDataList.Count - 2)
            .OrderBy(x => Random.value)
            .Take(numberOfTileTypes)
            .ToList();

        // Pair tiles
        for (int i = 0; i < totalPairs; i++)
        {
            TileData randomTileData = selectedTileTypes[Random.Range(0, selectedTileTypes.Count)];
            pairedTiles.Add(randomTileData);
            pairedTiles.Add(randomTileData);
        }

        pairedTiles = ShuffleTileData(pairedTiles);

        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
        foreach (SpecialTile specialTile in specialTiles)
        {
            TileData specialTileData = tileSO.GetTileData(specialTile.tileType);
            grid[specialTile.position.x, specialTile.position.y].
                SetTileData(specialTileData.tileType, specialTileData.icon, specialTileData.IsSpecial, specialTileData.IsClick);
            occupiedPositions.Add(specialTile.position);
        }

        int index = 0;
        foreach (Vector2Int position in currentlevel.fillablePositions)
        {
            if (!occupiedPositions.Contains(position))
            {
                grid[position.x, position.y].SetTileData(pairedTiles[index].tileType, pairedTiles[index].icon, pairedTiles[index].IsSpecial, pairedTiles[index].IsClick);
                numberOfTiles++;
                index++;
            }
        }
    }
    //public void AssignTileData(List<SpecialTile> specialTilesList)
    //{
    //    List<TileData> tileDataList = tileSO.listTile;
    //    List<TileData> pairedTiles = new List<TileData>();

    //    int totalTiles = (Rows - 2) * (Cols - 2) - specialTilesList.Count;
    //    int totalPairs = totalTiles / 2;

    //    //lay so luong type tu level 
    //    int numberOfTileTypes = Mathf.Min(currentlevel.numberOfTileTypes, tileDataList.Count);

    //    // random vi tri cho so loai tile vua layA
    //    List<TileData> selectedTileTypes = tileDataList
    //        .Take(tileDataList.Count - 2)
    //        .OrderBy(x => Random.value)
    //        .Take(numberOfTileTypes)
    //        .ToList();
    //    // Pair tiles
    //    for (int i = 0; i < totalPairs; i++)
    //    {
    //        TileData randomTileData = selectedTileTypes[Random.Range(0, selectedTileTypes.Count)];
    //        pairedTiles.Add(randomTileData);
    //        pairedTiles.Add(randomTileData);
    //    }

    //    pairedTiles = ShuffleTileData(pairedTiles);

    //    HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
    //    foreach (SpecialTile specialTile in specialTilesList)
    //    {
    //        TileData specialTileData = tileSO.GetTileData(specialTile.tileType);
    //        grid[specialTile.position.x, specialTile.position.y].
    //            SetTileData(specialTileData.tileType, specialTileData.icon, specialTileData.IsSpecial, specialTileData.IsClick);
    //        occupiedPositions.Add(specialTile.position);
    //    }

    //    int index = 0;
    //    for (int i = 1; i < Rows - 1; i++)
    //    {
    //        for (int j = 1; j < Cols - 1; j++)
    //        {
    //            Vector2Int position = new Vector2Int(i, j);
    //            if (!occupiedPositions.Contains(position))
    //            {
    //                grid[i, j].SetTileData(pairedTiles[index].tileType, pairedTiles[index].icon, pairedTiles[index].IsSpecial, pairedTiles[index].IsClick);
    //                numberOfTiles++;
    //                index++;
    //            }
    //        }
    //    }
    //}


    //------------------Shuffle tile data------------------------
    public List<TileData> ShuffleTileData(List<TileData> listTile)
    {
        List<TileData> shuffledList = new List<TileData>(listTile);
        int n = shuffledList.Count;
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(i, n);
            TileData temp = shuffledList[i];
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }
        return shuffledList;
    }
    //----------------Shuffle Tile in grid----------------------------
    public void ShuffleTileInGridUntilMatch()
    {
        List<TileData> listTileData;
        do
        {
            listTileData = new List<TileData>();
            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startCol; j < endCol; j++)
                {
                    if (grid[i, j].Type != TileType.Empty && !grid[i, j].IsSpecial)
                    {
                        listTileData.Add(new TileData
                        {
                            tileType = grid[i, j].Type,
                            icon = grid[i, j].icon.sprite,
                            IsSpecial = grid[i, j].IsSpecial,
                            IsClick = grid[i, j].IsClick
                        });
                    }
                }
            }
            listTileData = ShuffleTileData(listTileData);
            //sau khi co 1 list data thi truyen data vao grid
            int index = 0;
            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startCol; j < endCol; j++)
                {
                    if (grid[i, j].Type != TileType.Empty && !grid[i, j].IsSpecial)
                    {
                        grid[i, j].SetTileData(listTileData[index].tileType, listTileData[index].icon, listTileData[index].IsSpecial, listTileData[index].IsClick);
                        index++;
                    }
                }
            }
        } while (!CheckIfAnyTilesCanConnect() || numberOfTiles == 0); 
    }
    //------------when selected tile--------------------------------------------------------
    public void SelectTile(Tile tile)
    {
        if (IsCanPlay)
        {
            if (selectedStartTile == null)
            {
                selectedStartTile = tile;
                selectedStartTile.SetBorder(true);
                //Debug.Log($"Selected Start Tile: {tile.Position}");
            }
            else if (selectedEndTile == null && tile != selectedStartTile)
            {
                selectedEndTile = tile;
                selectedEndTile.SetBorder(true);
                //Debug.Log($"Selected End Tile: {tile.Position}");

                if (CanConnect(selectedStartTile, selectedEndTile, out List<Vector2Int> path))
                {
                    //Debug.Log("Tiles can connect!");
                    DrawConnection(path);
                    numberOfTiles -= 2;
                    //IsCanClick = false;
                    selectedStartTile.SetCanClick(false);
                    selectedEndTile.SetCanClick(false);
                    removeTileCoroutine = StartCoroutine(RemoveTilesWithDelay(selectedStartTile, selectedEndTile, 0.4f));
                    //RemoveTiles(selectedStartTile, selectedEndTile);
                }

                //else{ Debug.Log("Tiles cannot connect.");}
                selectedStartTile.SetBorder(false);
                selectedEndTile.SetBorder(false);
                selectedStartTile = null;
                selectedEndTile = null;
                //OnTileConnected();

            }
        }
    }
//---------------------check connect -----------------------------------------------------------------------
    public bool CanConnect(Tile startTile, Tile endTile, out List<Vector2Int> path)
    {
        path = new List<Vector2Int>();

        if (!TryFindPath(startTile, endTile, out path))
        {
            return false;

            //if (!TryFindPath(endTile, startTile, out path))
            //{
            //    return false;
            //}
            //else { return true; }
        }
        else 
        { 
            return true;
        }
    }
    private bool TryFindPath(Tile startTile, Tile endTile, out List<Vector2Int> path)
    {
        path = new List<Vector2Int>();

        // Kiểm tra tính hợp lệ của hai Tile
        if (startTile.Type != endTile.Type || startTile.Type == TileType.Empty || endTile.Type == TileType.Empty)
        {
            return false;
        }

        // Duyệt BFS để tìm tất cả các ô có thể đi qua
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> bfsQueue = new Queue<Vector2Int>();
        bfsQueue.Enqueue(startTile.Position);
        visited.Add(startTile.Position);

        while (bfsQueue.Count > 0)
        {
            Vector2Int currentPos = bfsQueue.Dequeue();

            foreach (var direction in directions)
            {
                Vector2Int newPos = currentPos + direction;

                if (IsWithinBounds(newPos) && !visited.Contains(newPos))
                {
                    Tile neighborTile = grid[newPos.x, newPos.y];

                    if (neighborTile.Type == TileType.Empty || neighborTile.Type == TileType.Water || neighborTile.Position == endTile.Position)
                    {
                        bfsQueue.Enqueue(newPos);
                        visited.Add(newPos);
                    }
                }
            }
        }

        // Lưu trữ đường đi ngắn nhất tìm được (nếu có)
        List<Vector2Int> bestPath = null;

        // Gọi hàm backtracking để tìm đường với số lần rẽ <= 2
        Backtrack(startTile.Position, endTile.Position, visited, new List<Vector2Int>(), Vector2Int.zero, 0, ref bestPath);

        if (bestPath != null)
        {
            path = bestPath;
            return true;
        }

        return false;
    }

    private void Backtrack(Vector2Int currentPos, Vector2Int endPos, HashSet<Vector2Int> visited, List<Vector2Int> currentPath, Vector2Int lastDirection, int turns, ref List<Vector2Int> bestPath)
    {
        currentPath.Add(currentPos);

        // Nếu đã đến đích và số lần rẽ <= 2
        if (currentPos == endPos && turns <= 2)
        {
            if (bestPath == null || currentPath.Count < bestPath.Count)
            {
                bestPath = new List<Vector2Int>(currentPath);
            }
        }
        else
        {
            // Duyệt qua tất cả các hướng
            foreach (var direction in directions)
            {
                Vector2Int newPos = currentPos + direction;

                // Nếu vị trí mới có thể đi và chưa được duyệt trong đường hiện tại
                if (visited.Contains(newPos) && !currentPath.Contains(newPos))
                {
                    int newTurns = (lastDirection == Vector2Int.zero || lastDirection == direction) ? turns : turns + 1;

                    // Nếu số lần rẽ <= 2, tiếp tục đi
                    if (newTurns <= 2)
                    {
                        Backtrack(newPos, endPos, visited, currentPath, direction, newTurns, ref bestPath);
                    }
                }
            }
        }

        // Backtrack: quay lại điểm rẽ trước đó
        currentPath.RemoveAt(currentPath.Count - 1);
    }
   
    
    public void OnTileConnected()
    {
        //Debug.Log(numberOfTiles + "----------");
        //------- ap rule sau khi connect duoc 2 o--------------
        if (numberOfTiles > 0)
        {
            currentlevel.ApplyRule(grid);
            currentlevel.ApplyRule(grid);
            if (!CheckIfAnyTilesCanConnect() && numberOfTiles>2)
            {
                ShuffleTileInGridUntilMatch();
            }
            //IsCanClick = true;
        }
        else
        {
            LevelManager.Ins.OnFinish();
        }
    }
    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < Rows && position.y >= 0 && position.y < Cols;
    }

    public bool CheckIfAnyTilesCanConnect()
    {
        // Iterate over all tiles and group them by type
        var tileGroups = new Dictionary<TileType, List<Tile>>();

        for (int i = startRow; i < endRow; i++)
        {
            for (int j = startCol; j < endCol; j++)
            {
                var currentTile = grid[i, j];
                if (currentTile.Type != TileType.Empty && currentTile.Type != TileType.Water)
                {
                    if (!tileGroups.ContainsKey(currentTile.Type))
                    {
                        tileGroups[currentTile.Type] = new List<Tile>();
                    }
                    tileGroups[currentTile.Type].Add(currentTile);
                }
            }
        }

        // Check if any two tiles of the same type can connect
        foreach (var group in tileGroups.Values)
        {
            for (int i = 0; i < group.Count; i++)
            {
                for (int j = i + 1; j < group.Count; j++)
                {
                    if (CanConnect(group[i], group[j], out _))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //-----Remove tile-----------------------------------------------------

    public void RemoveTiles(Tile tile1, Tile tile2)
    {
        //Debug.Log(tile1 + " " + tile2 + "================");
        //listTiles.Remove(tile1);
        //listTiles.Remove(tile2);

        //Debug.Log(listTiles.Count + "----------");

        grid[tile1.Position.x, tile1.Position.y] = null;
        grid[tile2.Position.x, tile2.Position.y] = null;

        tile1.Type = TileType.Empty;
        tile1.icon.sprite = null;
        tile1.IsClick = false;
        tile2.Type = TileType.Empty;
        tile2.icon.sprite = null;
        tile2.IsClick = false;

        grid[tile1.Position.x, tile1.Position.y] = tile1;
        grid[tile2.Position.x, tile2.Position.y] = tile2;
    }
    //-------------------------------------------------------------------------------
    //Draw line when connect 2 tiles
    private void DrawConnection(List<Vector2Int> path)
    {
        if (path.Count < 2)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        // Set color for the line
        //lineRenderer.startColor = Color.green;
        //lineRenderer.endColor = Color.green;

        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 tilePosition = grid[path[i].x, path[i].y].transform.position;
            lineRenderer.SetPosition(i, new Vector3(tilePosition.x, tilePosition.y, -1));
        }
        StartCoroutine(HideLineAfterDelay(0.4f));

    }
    private IEnumerator HideLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.positionCount = 0;
    }
    private IEnumerator RemoveTilesWithDelay(Tile tile1, Tile tile2, float delay)
    {
        yield return new WaitForSeconds(delay);
        tile1.SetCanClick(true); 
        tile2.SetCanClick(true);
        RemoveTiles(tile1, tile2);
        OnTileConnected();
    }
    public void OnDespawn()
    {
        if(removeTileCoroutine != null)
        {
            StopCoroutine(removeTileCoroutine);
        }
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Destroy(grid[i, j].gameObject);
            }
        }
        //listTiles.Clear();
        numberOfTiles = 0;
        
    }
}



