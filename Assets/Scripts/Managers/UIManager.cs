using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
/// <summary>
/// Controlling UI works
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instace;

    [SerializeField]
    TMP_Text LevelText;//Level text field 

    [SerializeField]
    GameObject EndPanel;//End panel object
    [SerializeField]
    GameObject NextLevelB, RestartLevelB;
    [SerializeField]
    TMP_Text EndTitle;


    #region Actions
    public static event Action showEndPanel;
    public static void EndPanelShow() { showEndPanel?.Invoke(); }

    public static event Action<int> levelIdText;
    public static void SetLevelIdText(int val) { levelIdText?.Invoke(val); }
    #endregion
    private void Awake()
    {
        if (instace == null)
            instace = this;
    }

    private void Start()
    {
        showEndPanel += ShowEndPanel;
        levelIdText += SetLevelId;
    }

    private void OnDestroy()
    {
        showEndPanel -= ShowEndPanel;
        levelIdText -= SetLevelId;
    }
    /// <summary>
    /// Start game triggered with button
    /// </summary>
    public void StartGame()
    {
        GameManager.instance.StartGame();
    }


    /// <summary>
    /// Set level text to UI
    /// </summary>
    /// <param name="levelId"></param>
    public void SetLevelId(int levelId)
    {
        LevelText.text = (levelId+1).ToString();
    }

    /// <summary>
    /// Showing end panel if count is lower than 1 it is fail state
    /// else it is win state
    /// </summary>
    public void ShowEndPanel()
    {
        EndPanel.SetActive(true);
        if (PlayerManager.instance._Count > 0)
        {
            //Game end perfectly
            NextLevelB.SetActive(true);
            EndTitle.text = "congratulations".ToUpper();
        }
        else
        {
            //Game end failed
            RestartLevelB.SetActive(true);
            EndTitle.text = "Failed".ToUpper();
        }
    }

    public void NextLevel()
    {
        GameManager.instance.NextLevel();
    }

    public void RestartLevel()
    {
        GameManager.instance.RestartLevel();
    }
}
