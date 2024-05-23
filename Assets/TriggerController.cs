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

    public bool isInContact = false;

    private void OnTriggerEnter(Collider other)
    {
        triggerCollisionCount++;
    }


    private void OnTriggerExit(Collider other)
    {
        if (triggerCollisionCount > 0)
        {
            triggerCollisionCount--;
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
        else
        {
            isInContact = false;
            triggerCollisionCount = 0;
        }
    }
}
