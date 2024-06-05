using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavMeshUpdater : MonoBehaviour
{
    private UnityAction buildingPlacedEventListener;
    private NavMeshSurface navMesh;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshSurface>();
        buildingPlacedEventListener = new UnityAction(updateNavMesh);

    }

    private void updateNavMesh()
    {
        navMesh.BuildNavMesh();
    }

    void Start()
    {
        EventManager.StartListening<BuildingPlacedEvent>(buildingPlacedEventListener);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<BuildingPlacedEvent>(buildingPlacedEventListener);
    }
}
