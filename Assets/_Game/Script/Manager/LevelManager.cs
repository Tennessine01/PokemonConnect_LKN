using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> levels;  
    public int currentLevelIndex = 0;
    public int currentBackGroundIndex = 0;
    private Level currentLevel;
    public List<GameObject> listBackGround = new();
    
    public void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        //LoadLevel(currentLevelIndex);
    }
    public void OnPlay()
    {
        UIManager.Ins.OpenUI<UIGamePlay>();
        GameManager.Ins.ChangeState(GameState.GamePlay);
        LoadLevel(currentLevelIndex);
        LoadBackGround(currentBackGroundIndex);

    }
    public void OnFinish()
    {
        currentLevelIndex++;
        currentBackGroundIndex++;
        GridManager.Ins.OnDespawn();
        GameManager.Ins.ChangeState(GameState.Win);
        UIManager.Ins.CloseUI<UIGamePlay>();
        UIManager.Ins.OpenUI<UINextLevel>();
    }
    public void OnLose()
    {

    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            GridManager.Ins.OnInit(currentLevel);
            return;
        }

        currentLevelIndex = levelIndex;
        currentBackGroundIndex = levelIndex;
        currentLevel = levels[currentLevelIndex];

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
