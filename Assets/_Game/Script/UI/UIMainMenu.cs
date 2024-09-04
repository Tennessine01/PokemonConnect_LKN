using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
    }
    public void PlayButton()
    {
        
        //LevelManager.Ins.OnInit();
        LevelManager.Ins.OnPlay();
        Close(0);
    }
}
