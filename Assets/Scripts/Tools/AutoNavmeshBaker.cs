using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoNavmeshBaker : MonoBehaviour
{

    private void Awake()
    {
        var navmeshSurface = gameObject.AddComponent<NavMeshSurface>();
        navmeshSurface.BuildNavMesh();
    }
}
