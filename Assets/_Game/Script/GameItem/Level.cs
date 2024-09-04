using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level/Create New Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public List<SpecialTile> specialTiles;
    [SerializeField] LevelRule levelRule;
    public float time;
    public int numberOfTileTypes;
    public List<Vector2Int> fillablePositions; // New: positions to fill data

    public void OnPlay()
    {
        //dem thoi gian
    }
    public void ApplyRule(Tile[,] grid)
    {
        if (levelRule != null)
        {
            levelRule.ApplyRule(grid, grid.GetLength(0), grid.GetLength(1));
        }
    }
}

[System.Serializable]
public class SpecialTile
{
    public Vector2Int position; 
    public TileType tileType;
    //public Sprite icon;
}

//public class SpecialTileManager : MonoBehaviour
//{
//    public List<SpecialTile> specialTilesList = new List<SpecialTile>();

//    public List<SpecialTile> GetValidSpecialTiles(int rows, int cols)
//    {
//        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
//        List<SpecialTile> validSpecialTiles = new List<SpecialTile>();

//        foreach (var specialTile in specialTilesList)
//        {
//            if (!IsWithinBounds(specialTile.position, rows, cols))
//            {
//                Debug.LogError($"Vị trí {specialTile.position} nằm ngoài lưới.");
//                continue;
//            }

//            if (occupiedPositions.Contains(specialTile.position))
//            {
//                Debug.LogError($"Vị trí {specialTile.position} đã có `Tile` đặc biệt khác.");
//                continue;
//            }

//            occupiedPositions.Add(specialTile.position);
//            validSpecialTiles.Add(specialTile);
//        }

//        if (validSpecialTiles.Count % 2 != 0)
//        {
//            Debug.LogError("Số lượng `Tile` đặc biệt phải là số chẵn.");
//            return null;
//        }

//        return validSpecialTiles;
//    }

//    private bool IsWithinBounds(Vector2Int position, int rows, int cols)
//    {
//        return position.x >= 0 && position.x < rows && position.y >= 0 && position.y < cols;
//    }
//}