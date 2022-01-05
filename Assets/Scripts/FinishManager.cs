using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Finish manager script using for the player can come to finish line
/// </summary>
public class FinishManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player") && !GameManager.isGameEnded)
        {
            GameManager.instance.EndGame();
           
        }
    }
}
