using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controlling tiny enemy
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPhysics : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private Vector3 Destination;//Keeps moving direction
    public Vector3 _Destination { get { return Destination; } set { Destination = value; } }


    public bool isPlayerCome = false;//Controlling is players come 
    private void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.Move(this.transform.forward + ((Vector3.forward + Vector3.right) * Random.Range(-.1f, .1f)));
    }

    private void Update()
    {
        if (isPlayerCome)
        {
            agent.SetDestination(PlayerManager.instance.CenterPoint);
        }
    }

    /// <summary>
    /// Moving enemies to player
    /// </summary>
    public void MoveToEnemy()
    {
        if (agent.isOnNavMesh)
            agent.SetDestination(Destination);
    }

    /// <summary>
    /// Stopping movement
    /// </summary>
    public void StopToMove()
    {
        if (agent.isOnNavMesh)
            agent.isStopped = true;
    }
}
