using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    public Image timerBar; 
    public float maxTime; 
    private float timeLeft;
    private bool IsCanUpdate => GameManager.Ins.IsState(GameState.GamePlay);

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.GamePlay);

    }
    void Update()
    {
        //Debug.Log(IsCanUpdate);
        if (timeLeft > 0 && IsCanUpdate)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        if(timeLeft <= 0) 
        {
            Debug.Log(timeLeft + "----");
            LevelManager.Ins.OnLose();
        }
    }
    public void SettingButton()
    {
        UIManager.Ins.OpenUI<Setting>();
    }
    public void SetupGamePlay(float time)
    {
        maxTime = time;
        timeLeft = maxTime;
    }
}
