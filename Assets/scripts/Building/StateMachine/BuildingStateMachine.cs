using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingStateMachine : MonoBehaviour
{
    public GameObject pointer;
    public LayerMask terrainLayer;

    [Range(1f, 100f)]
    public float constructionTime = 1f;

    public TextMeshProUGUI completionText;
    public Renderer renderer;
    public GameObject constructionScaffold;


    private Dictionary<BuildingState, IState> _states;
    private IState _currentState;

    private BuildingState _currentStateName;
    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void Initialize(GameObject obj)
    {
        _states = new Dictionary<BuildingState, IState>
        {
            { BuildingState.Selected, new SelectedState(obj, renderer, this, pointer, terrainLayer, constructionScaffold) },
            { BuildingState.UnderConstruction, new ConstructionState(this, constructionTime, _animator, completionText, constructionScaffold) },
            { BuildingState.Built, new ReadyState(obj) }
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
