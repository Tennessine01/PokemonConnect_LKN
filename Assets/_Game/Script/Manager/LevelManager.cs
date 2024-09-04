using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> levels;  
    public int currentLevelNumber = 0;
    public int currentBackGroundIndex = 0;
    private Level currentLevel;
    public List<GameObject> listBackGround = new();
    private float maxTime;
    public void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIMainMenu>();
    }
    public void OnPlay()
    {
        UIManager.Ins.CloseAll();
        LoadLevel(currentLevelNumber);
        LoadBackGround(currentBackGroundIndex);
        UIManager.Ins.OpenUI<UIGamePlay>().SetupGamePlay(maxTime);

    }
    public void OnFinish()
    {
        currentLevelNumber++;
        currentBackGroundIndex++;
        GridManager.Ins.OnDespawn();
        GameManager.Ins.ChangeState(GameState.Win);
        UIManager.Ins.CloseUI<UIGamePlay>();
        UIManager.Ins.OpenUI<UINextLevel>();
    }
    public void OnLose()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UILose>();

    }

    public void LoadLevel(int numberLevel)
    {
        if (numberLevel < 0 || numberLevel >= levels.Count)
        {
            GridManager.Ins.OnInit(currentLevel);
            return;
        }

        currentLevelNumber = numberLevel;
        currentBackGroundIndex = numberLevel;
        currentLevel = levels[currentLevelNumber];
        maxTime = currentLevel.time;

        GridManager.Ins.OnInit(currentLevel);
    }
    public void LoadBackGround(int index)
    {
        if (index >= 0 && index < listBackGround.Count)
        {
            foreach (GameObject obj in listBackGround)
            {
                obj.SetActive(false);
            }
            listBackGround[index].SetActive(true);
        }
    }

}
