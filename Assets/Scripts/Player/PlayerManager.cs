using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
/// <summary>
/// Controlling player manage works
/// </summary>
public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    #region Game Variables

    /// <summary>
    /// Runtime tiny player count
    /// </summary>
    [SerializeField]
    private int Count = 0;
    public int _Count {
        get { return Count; }
        set { Count = value;
            CountText.text = Count.ToString();
            }
    }

    #endregion
    #region Actions

    /// <summary>
    /// Adding tiny players action
    /// </summary>
    public static event Action <int> onPlayerAdded;
    public static void PlayerAdd(int value) { onPlayerAdded?.Invoke(value); }

    /// <summary>
    /// Removing tiny player action
    /// </summary>
    public static event Action<int> onPlayerRemove;
    public static void PlayerRemove(int value) { onPlayerRemove?.Invoke(value); }

    #endregion
    #region Player Variables

    [SerializeField] GameObject TinyPlayerParent;//tiny players are set as children of this object
    [SerializeField] GameObject TinyPlayerPrefab;//Prefab of tiny player
    [SerializeField] List<GameObject> TinyPlayerPool = new List<GameObject>();//Pool of tiny player objec


    public Vector3 CenterPoint = Vector3.zero;//Tiny objects centerPoint

    public bool isEnemyTime = false;//Is tiny players in war enemy objects
    #endregion

    #region UI Variables
    public TMP_Text CountText;//Tiny player count text field
    #endregion

    #region Player Manager Initialization

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        _Count = 1;
        CreateOrAddPool(400);
        onPlayerAdded += AddTinyPlayers;
        onPlayerRemove += RemoveTinyPlayers;
        
    }

    /// <summary>
    /// Creating tiny players pool
    /// </summary>
    /// <param name="value"></param>
    void CreateOrAddPool(int value)
    {
        for (int i = 0; i < value; i++)
        {
            var tiny = Instantiate(TinyPlayerPrefab, TinyPlayerParent.transform);
            tiny.transform.localPosition = Vector3.up * UnityEngine.Random.Range(0.0f,1.0f);
            TinyPlayerPool.Add(tiny);
            tiny.SetActive(false);
        }
      
    }

    private void Update()
    {
        CalculateCenterPoint();//Calculating center points all the time
    }

    private void OnDestroy()
    {
        onPlayerAdded -= AddTinyPlayers;
        onPlayerRemove -= RemoveTinyPlayers;
    }
    #endregion




    #region Counting Functions

    /// <summary>
    /// Adding tiny players using value 
    /// </summary>
    /// <param name="value"></param>
    public void AddTinyPlayers(int value)
    {

        for (int i = 0; i < TinyPlayerPool.Count; i++)
        {
            if (value <= 0)
            {
                break;
            }
            else
            {
                if (TinyPlayerPool[i].gameObject.activeInHierarchy == false)
                {
                    TinyPlayerPool[i].gameObject.SetActive(true);
                    _Count++;
                    value--;
                }
            }
        }
    }


    /// <summary>
    /// Removing tiny players using value
    /// Controlling is all players is dead?
    /// if it's true calling game end
    /// </summary>
    /// <param name="value"></param>
    public void RemoveTinyPlayers(int value)
    {
        for (int i = 0; i < TinyPlayerPool.Count; i++)
        {
            if (value <= 0)
            {
                break;
            }
            else
            {
                if (TinyPlayerPool[i].gameObject.activeInHierarchy == true)
                {
                    TinyPlayerPool[i].gameObject.SetActive(false);
                    _Count--;
                    value--;
                }
            }
        }
        if (Count <= 0)
        {
            GameManager.instance.EndGame();
            
            foreach (var o in TinyPlayerPool)
                o.SetActive(false);
        }
    }


    /// <summary>
    /// Removing Selected Player for set that player is dead
    /// </summary>
    /// <param name="player"></param>
    public void RemoveSelectedPlayer(GameObject player)
    {
        TinyPlayerPool[TinyPlayerPool.IndexOf(player)].SetActive(false);
        _Count--;
        if (Count <= 0)
        {
            GameManager.instance.EndGame();

            foreach (var o in TinyPlayerPool)
                o.SetActive(false);
        }

    }
    #endregion


    #region Tiny Player Movement

    /// <summary>
    /// Move players to enemy Center points
    /// Set isEnemyTime true because player can't swerve the players that time
    /// </summary>
    /// <param name="EnemyObject"></param>
    public void MovePlayers(GameObject EnemyObject)
    {
        foreach(GameObject o in TinyPlayerPool)
        {
            o.GetComponent<CharacterPhysics>().isEnemyCome = true;
            o.GetComponent<CharacterPhysics>()._Destination = EnemyObject.transform.position;
            o.GetComponent<CharacterPhysics>().MoveToEnemy();
        }
        isEnemyTime = true;
    }


    /// <summary>
    /// Moving players normal(Swerve time)
    /// </summary>
    public void MoveNormal()
    {
        foreach (GameObject o in TinyPlayerPool)
        {
            o.GetComponent<CharacterPhysics>().isEnemyCome = false;
            o.GetComponent<CharacterPhysics>().StopToMove();
        }
        isEnemyTime = false;
    }


    /// <summary>
    /// Calculating Center points
    /// it's using for hey enemies we are here come over there and get fight with us
    /// </summary>
    public void CalculateCenterPoint()
    {
        Vector3 temp = Vector3.zero;
        int total = 0;
        for (int i = 0; i < TinyPlayerPool.Count; i++)
        {
            if (TinyPlayerPool[i].activeInHierarchy)
            {
                temp += TinyPlayerPool[i].transform.position;
                total++;
            }
        }
        temp /= total;
        CenterPoint = temp;
    }

    #endregion
}
