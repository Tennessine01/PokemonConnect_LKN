using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINextLevel : UICanvas
{
    public void PlayButton()
    {
        //LevelManager.Ins.OnInit();
        LevelManager.Ins.OnPlay();
        Close(0);
    }
}
