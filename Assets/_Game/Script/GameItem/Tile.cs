using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public TileType Type;
    public Vector2Int Position;
    public SpriteRenderer icon;
    public SpriteRenderer border;
    public bool IsSpecial;
    public bool IsClick;
    public bool IsCanClick;
    public void SetTileData(TileType type, Sprite image, bool isSpecial, bool isClick )
    {
        Type = type;
        icon.sprite = image;
        IsClick = isClick;
        IsSpecial = isSpecial;
        border.gameObject.SetActive(false);
        IsCanClick = true;
    }
    public void CopyTileData(Tile otherTile)
    {
        SetTileData(otherTile.Type, otherTile.icon.sprite, otherTile.IsSpecial, otherTile.IsClick);
    }
    public void ClearTileData()
    {
        Type = TileType.Empty;  
        icon.sprite = null;
        IsSpecial = false;
        IsClick = false;
        IsCanClick = true;
    }
    public virtual void OnMouseDown()
    {
        if (Type != TileType.Empty && IsClick == true && GameManager.Ins.IsState(GameState.GamePlay) && IsCanClick)
        {
            GridManager.Ins.SelectTile(this);
        }
    }
    public void SetBorder(bool check)
    {
        if(Type != TileType.Empty)
        {
            border.gameObject.SetActive(check);
        }
    }
    public void SetCanClick(bool check)
    {
        IsCanClick = check;
    }

}
