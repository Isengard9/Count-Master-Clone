using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterPhysics : MonoBehaviour
{
    /// <summary>
    /// Tiny player agent
    /// </summary>
    [SerializeField] private NavMeshAgent agent;
   

    /// <summary>
    /// Tiny player going destination
    /// </summary>
    private Vector3 Destination;
    public Vector3 _Destination { get { return Destination; } set { Destination = value; } }


    /// <summary>
    /// Setting is enemy come it is fight time
    /// </summary>
    public bool isEnemyCome = false;

    private void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if(agent.isOnNavMesh)//Moving character little bit random point
            agent.Move(this.transform.forward +( (Vector3.forward + Vector3.right)* Random.Range(-.1f,.1f)));
    }


    private void Update()
    {
        //If the character move too much far come back to main point
        if (Vector3.Distance(this.transform.position, this.transform.parent.position) > 4 && !isEnemyCome && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(this.transform.parent.position);
        }
        //Else not move
        else if(Vector3.Distance(this.transform.position, this.transform.parent.position) <= 4 && !isEnemyCome && agent.isOnNavMesh)
        {
            agent.isStopped = true; 
        }
        
    }

    /// <summary>
    /// Go to enemy and fight with them
    /// </summary>
    public void MoveToEnemy()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(Destination);
        }
    }

    /// <summary>
    /// Enemies died stop moving
    /// </summary>
    public void StopToMove()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Is triggered on Enemy point fight time
        if (other.transform.tag.Equals("EnemyPoint"))
        {

            PlayerManager.instance.MovePlayers(other.gameObject);
            other.GetComponent<Collider>().enabled = false;
            other.GetComponent<EnemyGroupManager>().MoveToPlayer();
        }
        //If the triggered enemy fight that enemy
        if (other.transform.tag.Equals("Enemy"))
        {
            other.transform.GetComponentInParent<EnemyGroupManager>().DestroyEnemy(other.gameObject, this.gameObject); ;
        }
    }

}