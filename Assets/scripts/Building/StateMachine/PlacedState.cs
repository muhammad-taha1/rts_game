using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedState : IState
{
    private readonly GameObject _object;

    public PlacedState(GameObject obj)
    {
        _object = obj;
    }

    public void Enter()
    {
        // Reset color to original if needed
    }

    public void Update()
    {
        // Update logic for placed state if needed
    }

    public void Exit()
    {
        // Clean up logic if needed
    }
}
