using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : UICanvas
{
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Setting);
    }
    public void ContinueButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        Close(0);
    }
    public void BackToMenuButton()
    {
        GridManager.Ins.OnDespawn();
        LevelManager.Ins.OnInit();
        Close(0);
    }
}
