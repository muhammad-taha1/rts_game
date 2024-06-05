using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private float triggerCollisionCount = 0;
    private float builderTriggerCount = 0;

    public bool isInContact = false;
    public bool isBuilderInContact = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "pointer")
        {
            //print("triggered with: " + other.gameObject.name);
            triggerCollisionCount++;
        }

        if (other.gameObject.layer == 6)
        {
            print("builder enter trigger: " + other.gameObject.name);

            if (other.GetComponent<BuildingController>() != null)
            {
                builderTriggerCount++;
            }
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (triggerCollisionCount > 0  && other.gameObject.name != "pointer")
        {
            triggerCollisionCount--;
        }

        if (other.gameObject.layer == 6)
        {
            print("builder exit trigger: " + other.gameObject.name);
            if (other.GetComponent<BuildingController>() != null)
            {
                builderTriggerCount--;
            }
        }
    }

    private void Update()
    {
        BuildingState stateName = gameObject.GetComponent<BuildingStateMachine>().getCurrentStateName();

        if (stateName.Equals(BuildingState.Selected))
        {
            if (triggerCollisionCount > 0)
            {
                isInContact = true;
            }
            if (triggerCollisionCount <= 0)
            {
                isInContact = false;
            }

            SelectedState state = (SelectedState)gameObject.GetComponent<BuildingStateMachine>().getCurrentState();

            if (state != null)
            {
                state.updateIsInContact(isInContact);
            }
        }
        else if (stateName.Equals(BuildingState.UnderConstruction))
        {
            if (builderTriggerCount > 0)
            {
                isBuilderInContact = true;
            }
            if (builderTriggerCount <= 0)
            {
                isBuilderInContact = false;
            }

            ConstructionState state = (ConstructionState)gameObject.GetComponent<BuildingStateMachine>().getCurrentState();

            if (state != null)
            {
                state.updateIsInContact(isBuilderInContact);
            }
        }
        else
        {
            isInContact = false;
            triggerCollisionCount = 0;
            builderTriggerCount = 0;
            isBuilderInContact = false;
        }
    }
}
