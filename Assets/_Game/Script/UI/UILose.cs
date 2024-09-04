using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UICanvas
{
    public Text score;

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Lose);
    }

    public void MainMenuButton()
    {
        GridManager.Ins.OnDespawn();
        LevelManager.Ins.OnInit();
        Close(0);
    }
}
