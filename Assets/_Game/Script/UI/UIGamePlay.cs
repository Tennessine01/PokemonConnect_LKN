using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlay : UICanvas
{
    public void SettingButton()
    {
        UIManager.Ins.OpenUI<Setting>();
    }
}
