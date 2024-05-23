using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStateMachine : MonoBehaviour
{
    public GameObject pointer;
    public LayerMask terrainLayer;

    private Dictionary<BuildingState, IState> _states;
    private IState _currentState;

    private BuildingState _currentStateName;



    public void Initialize(GameObject obj)
    {
        _states = new Dictionary<BuildingState, IState>
        {
            { BuildingState.Selected, new SelectedState(obj, this, pointer, terrainLayer) },
            { BuildingState.Placed, new PlacedState(obj) }
        };

        ChangeState(BuildingState.Selected);
    }


    public void ChangeState(BuildingState newState)
    {
        _currentState?.Exit();
        _currentState = _states[newState];
        _currentStateName = newState;
        _currentState.Enter();
    }

    private void LateUpdate()
    {
        _currentState?.Update();
    }

    public BuildingState getCurrentStateName()
    {
        return _currentStateName;
    }

    public IState getCurrentState()
    {
        return _currentState;
    }
}
