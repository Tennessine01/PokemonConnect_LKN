using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTest : Tile
{
    public Mode tileMode;
    public List<GameObject> tileIcon;
    public override void OnMouseDown()
    {
        LevelEditor.Ins.OnTileSelected(Position, this);
    }
    public void ActivateTile(int index)
    {
        for (int i = 0; i < tileIcon.Count; i++)
        {
            if (i == index)
            {
                tileIcon[i].SetActive(true);
            }
            else
            {
                tileIcon[i].SetActive(false);
            }
        }
    }
    public void SetFillable()
    {
        tileMode = Mode.Fillable;
        ActivateTile(0);
    }

    // dung de chuyen type tu tiletype trong SO sang mode type cua test
    public void SetSpecial(TileType type)
    {
        switch (type)
        {
            case TileType.Rock:
                tileMode = Mode.Rock;
                ActivateTile(1);
                break;
            case TileType.Water: 
                tileMode = Mode.Water;
                ActivateTile(2);
                break;      
        }
    }
    public void ResetData()
    {
        Debug.Log("resetttt");
        foreach (GameObject obj in tileIcon)
        {
            obj.SetActive(false);
        }
        tileMode = Mode.None;
        Type = TileType.Empty;
    }
}