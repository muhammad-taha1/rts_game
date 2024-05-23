using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public GameObject buildingPrefab;


    public GameObject pointer;

    private GameObject selectedBuilding = null;


    // Update is called once per frame
    void LateUpdate()
    {

        // select/un-select building
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (selectedBuilding == null) {
                var position = pointer.gameObject.transform.position;
                selectedBuilding = Instantiate(buildingPrefab, new Vector3(position.x, 1, position.z), Quaternion.identity);
                selectedBuilding.GetComponent<BuildingStateMachine>().Initialize(selectedBuilding);
            }

            else
            {
                selectedBuilding = null;
            }

        }
    }
}
