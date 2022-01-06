using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Enemy group manager script using for controlling enemy group
/// Go for fight
/// Creating enemy pool
/// </summary>
public class EnemyGroupManager : MonoBehaviour
{
    #region Game Variables

    /// <summary>
    /// Count of tiny enemies in group
    /// </summary>
    [SerializeField]
    private int Count = 0;
    [SerializeField]
    public int _Count
    {
        get { return Count; }
        set
        {
            Count = value;
            CountText.text = Count.ToString();
        }
    }

    #endregion

    #region Player Variables

    [SerializeField] GameObject TinyEnemyPrefab;//Prefab of tiny enemy
    [SerializeField] List<GameObject> TinyEnemyPool = new List<GameObject>();//Pool of tiny enemies
    public Vector3 CenterPoint = Vector3.zero;//Center point of enemy group
    #endregion

    #region UI Variables
    public TMP_Text CountText;//Enemy group count text field
    #endregion

  
    private void Start()
    {
      
        CreateOrAddPool(_Count);
    }

    /// <summary>
    /// Creating enemy group using value
    /// </summary>
    /// <param name="value"></param>
    void CreateOrAddPool(int value)
    {
        for (int i = 0; i < value; i++)
        {
            var tiny = Instantiate(TinyEnemyPrefab, this.transform);
            tiny.transform.localPosition = Vector3.up * Random.Range(0.0f, 1.0f);
            TinyEnemyPool.Add(tiny);
            
        }
        _Count = Count;
    }

    private void Update()
    {
        CalculateCenterPoint();
    }

    /// <summary>
    /// Destroying enemy when player triggered tiny enemy and destroying triggered tiny player
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="Player"></param>
    public void DestroyEnemy(GameObject enemy, GameObject Player)
    {
        if (TinyEnemyPool.Contains(enemy) && enemy.activeInHierarchy)
        {
            TinyEnemyPool.Remove(enemy);
            enemy.SetActive(false);
            _Count = TinyEnemyPool.Count;

            PlayerManager.instance.RemoveSelectedPlayer(Player);
        }

        if (_Count <= 0)
        {
            PlayerManager.instance.MoveNormal();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Move enemies to fight players
    /// </summary>
    public void MoveToPlayer()
    {
        foreach(var o in TinyEnemyPool)
        {
            o.GetComponent<EnemyPhysics>()._Destination = PlayerManager.instance.gameObject.transform.position;
            o.GetComponent<EnemyPhysics>().isPlayerCome = true;
            o.GetComponent<EnemyPhysics>().MoveToEnemy();
        }
    }

    /// <summary>
    /// Calculating group center points
    /// </summary>
    public void CalculateCenterPoint()
    {
        Vector3 temp = Vector3.zero;
        int total = 0;
        for (int i = 0; i < TinyEnemyPool.Count; i++)
        {
            if (TinyEnemyPool[i].activeInHierarchy)
            {
                temp += TinyEnemyPool[i].transform.position;
                total++;
            }
        }
        temp /= total;
        CenterPoint = temp;
    }
}
