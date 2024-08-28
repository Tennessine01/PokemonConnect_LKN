using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TileSO", menuName = "ScriptableObjects/TileData", order = 1)]
[System.Serializable]
public class TileSO : ScriptableObject
{
    [SerializeField] public List<TileData> listTile;
        public TileData GetTileData(TileType type)
        {
            foreach (TileData item in listTile)
            {
                if (item.tileType == type)
                {
                    return item;
                }
            }
            return null;
        }
}

[System.Serializable]
public class TileData
{
    public TileType tileType;
    public Sprite icon;
    public bool IsSpecial;
    public bool IsClick;
    //public T GetT<T>() where T : TileData
    //{
        
    //}
}

