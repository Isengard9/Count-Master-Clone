using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager of game
/// Controlling level system
/// Controlling game states
/// Loading next or restart level
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGameStarted = false;//Controlling is game started 
    public static bool isGameEnded = false;//Controlling is game ended

    

    [SerializeField]
    private int LevelId = 0;//Keeps level id no
    public int _LevelId { get { return LevelId; } set { LevelId = value; } }

    [SerializeField]
    List<GameObject> levels = new List<GameObject>();//Keeps level prefabs

    private void Awake()
    {
        if (instance == null)
            instance = this;

      
    }
    private void Start()
    {
        LoadGame();
    }

    #region Game Functions
  

    public void StartGame()
    {
        isGameStarted = true;
        isGameEnded = false;
    }


    public void EndGame()
    {
        isGameEnded = true;
        UIManager.EndPanelShow();
    }

    #endregion

    #region Level Functions

    private void LoadGame()
    {
        if (levels.Count <= 0)
            return;

        _LevelId = PlayerPrefs.GetInt("LevelId", 0);
        Instantiate(levels[LevelId % levels.Count]);
        UIManager.SetLevelIdText(_LevelId);
    }

    public void NextLevel()
    {
        _LevelId++;
        PlayerPrefs.SetInt("LevelId", LevelId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
