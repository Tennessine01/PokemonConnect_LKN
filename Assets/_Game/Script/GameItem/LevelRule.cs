using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelRule : ScriptableObject
{
    public abstract void ApplyRule(Tile[,] grid, int rows, int cols);
}

[CreateAssetMenu(menuName = "LevelRule/ShiftRight")]
public class ShiftRightRule : LevelRule
{
    public override void ApplyRule(Tile[,] grid, int rows, int cols)
    {
        for (int i = 1; i < rows-1; i++)
        {
            for (int j = cols -2; j > 0; j--)
            {
                if (grid[i, j].Type == TileType.Empty && grid[i, j - 1].Type != TileType.Empty)
                {
                    if (!grid[i, j - 1].IsSpecial)
                    {
                        grid[i, j].CopyTileData(grid[i, j - 1]);

                        grid[i, j - 1].ClearTileData();
                    }
                }
            }
        }
    }
}

[CreateAssetMenu(menuName = "LevelRule/ShiftLeft")]
public class ShiftLeftRule : LevelRule
{
    public override void ApplyRule(Tile[,] grid, int rows, int cols)
    {
        for (int i = 1; i < rows - 1; i++)
        {
            for (int j = 1; j < cols - 1; j++)
            {
                if (grid[i, j].Type == TileType.Empty && grid[i, j + 1].Type != TileType.Empty)
                {
                    if (!grid[i, j + 1].IsSpecial)
                    {
                        grid[i, j].CopyTileData(grid[i, j + 1]);

                        grid[i, j + 1].ClearTileData();
                    }
                }
            }
        }
    }
}
[CreateAssetMenu(menuName = "LevelRule/ShiftUp")]
public class ShiftUpRule : LevelRule
{
    public override void ApplyRule(Tile[,] grid, int rows, int cols)
    {
        for (int j = 2; j < cols - 2; j++)
        {
            for (int i = 2; i < rows - 2; i++)
            {
                if (grid[i, j].Type != TileType.Empty && grid[i - 1, j].Type == TileType.Empty)
                {
                    if (!grid[i - 1, j].IsSpecial)
                    {
                        grid[i - 1, j].CopyTileData(grid[i, j]);

                        grid[i, j].ClearTileData();
                    }
                }
            }
        }
    }
}
[CreateAssetMenu(menuName = "LevelRule/ShiftDown")]
public class ShiftDownRule : LevelRule
{
    public override void ApplyRule(Tile[,] grid, int rows, int cols)
    {
        for (int j = 1; j < cols; j++)
        {
            for (int i = rows - 2; i > 0; i--)
            {
                if (grid[i, j].Type != TileType.Empty && grid[i + 1, j].Type == TileType.Empty)
                {
                    if (!grid[i + 1, j].IsSpecial)
                    {
                        grid[i + 1, j].CopyTileData(grid[i, j]);

                        grid[i, j].ClearTileData();
                    }
                }
            }
        }
    }
}

[CreateAssetMenu(menuName = "LevelRule/ShiftAlternatingHorizontal")]
public class ShiftAlternatingHorizontalRule : LevelRule
{
    public override void ApplyRule(Tile[,] grid, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = cols - 3; j >= 0; j--)
                {
                    if (grid[i, j].Type != TileType.Empty && grid[i, j + 1].Type == TileType.Empty)
                    {
                        if (!grid[i, j + 1].IsSpecial)
                        {
                            grid[i, j + 1].CopyTileData(grid[i, j]);

                            grid[i, j].ClearTileData();
                        }
                    }
                }
            }
            else
            {
                for (int j = 2; j < cols; j++)
                {
                    if (grid[i, j].Type != TileType.Empty && grid[i, j - 1].Type == TileType.Empty)
                    {
                        if (!grid[i, j - 1].IsSpecial)
                        {
                            grid[i, j - 1].CopyTileData(grid[i, j]);

                            grid[i, j].ClearTileData();
                        }
                    }
                }
            }
        }
    }
}

[CreateAssetMenu(menuName = "LevelRule/ShiftAlternatingVertical")]
public class ShiftAlternatingVerticalRule : LevelRule
{
    public override void ApplyRule(Tile[,] grid, int rows, int cols)
    {
        for (int j = 0; j < cols; j++)
        {
            if (j % 2 == 0)
            {
                for (int i = rows - 2; i >= 0; i--)
                {
                    if (grid[i, j].Type != TileType.Empty && grid[i + 1, j].Type == TileType.Empty)
                    {
                        if (!grid[i + 1, j].IsSpecial)
                        {
                            grid[i + 1, j].CopyTileData(grid[i, j]);

                            grid[i, j].ClearTileData();
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i < rows; i++)
                {
                    if (grid[i, j].Type != TileType.Empty && grid[i - 1, j].Type == TileType.Empty)
                    {
                        if (!grid[i - 1, j].IsSpecial)
                        {
                            grid[i - 1, j].CopyTileData(grid[i, j]);

                            grid[i, j].ClearTileData();
                        }
                    }
                }
            }
        }
    }
}

