using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : Singleton<LevelEditor>
{
    public Dropdown levelDropdown;
    public Button fillableButton;
    public Button specialTileButton;
    public Button saveButton;
    public List<Level> levels;  // Assign in Inspector
    public TileSO tileSO;

    private Level selectedLevel;
    private List<Vector2Int> fillableSelectedPosition = new List<Vector2Int>();
    private List<SpecialTile> specialTilesList = new List<SpecialTile>();

    public TileTest[,] grid;
    public TileTest tilePrefab;
    public int rows;
    public int cols;
    public float tileSpacing;
    public Vector2Int currentPosition;
    public Mode currentMode = Mode.None;
    private TileTest currentTileTest;
    private SpecialTile currentSpecialTile;

    public void Start()
    {
        rows = GridManager.Ins.Rows;
        cols = GridManager.Ins.Cols;
        tileSpacing = GridManager.Ins.tileSpacing;
        selectedLevel = levels[0];
        PopulateLevelDropdown();
        InitializeGrid();
        LoadLevelData();

        //fillableButton.onClick.AddListener(SetFillablePositions);
        //specialTileButton.onClick.AddListener(SetSpecialTiles);
        //saveButton.onClick.AddListener(SaveData);
        //InitializeGrid();

    }
    public void LoadLevelData()
    {
        //fillableSelectedPosition.Clear();
        //specialTilesList.Clear();
        fillableSelectedPosition = new List<Vector2Int>(selectedLevel.fillablePositions);
        specialTilesList = new List<SpecialTile>(selectedLevel.specialTiles);

        foreach (Vector2Int pos in fillableSelectedPosition)
        {
            grid[pos.x, pos.y].SetFillable();
        }

        foreach (SpecialTile tile in specialTilesList)
        {
            grid[tile.position.x, tile.position.y].SetSpecial(tile.tileType);

        }
    }
    //private void Update()
    //{
    //    Debug.Log(currentMode + "==============");
    //    Debug.Log(currentTileTest.tileMode + "===========");
    //}
    public void PopulateLevelDropdown()
    {
        levelDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var level in levels)
        {
            options.Add(level.levelName);
        }
        levelDropdown.AddOptions(options);
        levelDropdown.onValueChanged.AddListener(delegate { SelectLevel(levelDropdown.value); });
    }

    public void SelectLevel(int index)
    {
        selectedLevel = levels[index];
        DestroyGrid();
        InitializeGrid();
        LoadLevelData();

    }
    public void InitializeGrid()
    {
        grid = new TileTest[rows, cols];

        float startX = -(cols / 2f) * tileSpacing + tileSpacing / 2;
        float startY = -(rows / 2f) * tileSpacing + tileSpacing / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 position = new Vector3(startX + j * tileSpacing, startY + i * tileSpacing, 0);
                TileTest tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObject.transform.parent = transform;

                //tileObject.Type = TileType.Empty;
                tileObject.Position = new Vector2Int(i, j);
                //tileObject.ResetData();
                grid[i, j] = tileObject;
            }
        }
    }
    public void DestroyGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Destroy(grid[i, j].gameObject);
            }
        }
    }
    public void SetFillablePositions()
    {
        SetMode(Mode.Fillable);
    }

    public void SetRockTiles()
    {
        SetMode(Mode.Rock);
    }
    public void SetWaterTiles()
    {
        SetMode(Mode.Water);
    }
    private void SetMode(Mode mode)
    {
        currentMode = mode;
    }
    public void ClearTileData()
    {
        // Clear the lists
        fillableSelectedPosition.Clear();
        specialTilesList.Clear();

        // Reset all tiles in the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j].ResetData(); // Assuming ResetData() resets the tile to its default state
            }
        }
    }

    public void OnTileSelected(Vector2Int position, TileTest tile)
    {
        currentTileTest = tile;
        currentPosition = position;
        SpecialTile specialTile = new SpecialTile();

        switch (currentMode)
        {
            case Mode.Fillable:
                if (!fillableSelectedPosition.Contains(currentPosition))
                {
                    fillableSelectedPosition.Add(currentPosition);
                    currentTileTest.SetFillable();
                    //Debug.Log(currentPosition + "=====.");
                }
                break;

            case Mode.Rock:
                specialTile.position = position;
                specialTile.tileType = TileType.Rock;
                if (!specialTilesList.Exists(tile => tile.position == position && tile.tileType == TileType.Rock))
                {
                    specialTilesList.Add(specialTile);
                }
                currentTileTest.SetSpecial(TileType.Rock);
                break;
            case Mode.Water:
                specialTile.position = position;
                specialTile.tileType = TileType.Water;
                if (!specialTilesList.Exists(tile => tile.position == position && tile.tileType == TileType.Water))
                {
                    specialTilesList.Add(specialTile);
                }
                currentTileTest.SetSpecial(TileType.Water);
                break;
        }
    }


    public void SaveData()
    {
        if (fillableSelectedPosition.Count % 2 != 0)
        {
            Debug.LogError("number is not even");
            return;
        }
        if (fillableSelectedPosition.Count < 2)
        {
            Debug.LogError("Not enough tile");
            return;
        }
        if (selectedLevel != null)
        {
            Debug.Log(fillableSelectedPosition.Count);
            selectedLevel.fillablePositions = fillableSelectedPosition;
            selectedLevel.specialTiles = specialTilesList;
            // Save changes to the ScriptableObject
            UnityEditor.EditorUtility.SetDirty(selectedLevel);
        }
    }
}