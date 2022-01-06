using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Calculating the navmesh surface
/// </summary>
public class RoadNavMesh : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update
    void Awake()
    {
        surface = this.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

   
}
