using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstructionState : IState
{
    private BuildingStateMachine buildingStateMachine;
    private float constructionTime;
    private Animator animator;
    private TextMeshProUGUI completionText;

    private float elapsedTime = 0f;

    private bool isBeingBuilt = false;

    private GameObject constructionScaffold;

    public ConstructionState(BuildingStateMachine buildingStateMachine, float constructionTime, Animator animator, TextMeshProUGUI completionText, GameObject constructionScaffold)
    {
        this.buildingStateMachine = buildingStateMachine;
        this.constructionTime = constructionTime;
        this.animator = animator;
        this.completionText = completionText;
        this.constructionScaffold = constructionScaffold;
    }

    public void Enter()
    {
        constructionScaffold.gameObject.SetActive(true);
        animator.enabled = true;
        animator.speed = 0f;
        animator.SetBool("building", true);
        animator.SetFloat("speed", 1.0f / constructionTime);

    }

    public void Exit()
    {
        animator.SetBool("building", false);
        constructionScaffold.gameObject.SetActive(false);
        EventManager.TriggerEvent<BuildingConstructedEvent>();
    }

    internal void updateIsInContact(bool isBuilderInContact)
    {
        isBeingBuilt = isBuilderInContact;
    }

    void IState.Update()
    {
        if (isBeingBuilt)
        {
            elapsedTime += Time.deltaTime;
            animator.speed = 1f;
            animator.SetBool("building", true);
        }

        else
        {
            animator.speed = 0f;
            animator.SetBool("building", false);

        }

        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        int percentageCompletion = Mathf.RoundToInt(animator.GetCurrentAnimatorStateInfo(0).normalizedTime/elapsedTime);

        
  
        completionText.text = "Building: " + percentageCompletion + "%";

        if (percentageCompletion >= 100)
        {
            buildingStateMachine.ChangeState(BuildingState.Built);
        }

    }
}
