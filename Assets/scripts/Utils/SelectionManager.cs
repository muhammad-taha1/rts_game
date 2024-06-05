using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    private UnityAction<int> selectEventListener;

    private int currentlySelectedUnit = 0;

    private void Awake()
    {
        //selectEventListener = new UnityAction<int>(selectionHandler);
    }

    void Start()
    {
        //EventManager.StartListening<SelectEvent, int>(selectEventListener);
    }

    private void OnDestroy()
    {
        //EventManager.StopListening<SelectEvent, int>(selectEventListener);
    }


    //private void selectionHandler(int id)
    //{
    //    // if a unit is already selected, deselect the old one and update it to new one
    //    if (currentlySelectedUnit != 0  && id != currentlySelectedUnit) {
    //        EventManager.TriggerEvent<DeSelectEvent, int>(currentlySelectedUnit);
    //        currentlySelectedUnit = id;
    //    }
    //}
}
