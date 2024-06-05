using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public List<BuildingPrefabMap> buildingPrefabMapList;


    public GameObject pointer;

    private GameObject selectedBuilding = null;


    public void selectBuilding(BuildingType building)
    {

        if (selectedBuilding == null) {
            BuildingPrefabMap buildingMap = buildingPrefabMapList.Find(map => map.BuildingType == building);
            if (buildingMap == null)
            {
                Debug.LogError("building not found for type: " + building);
            }

            var position = pointer.gameObject.transform.position;
            selectedBuilding = Instantiate(buildingMap.BuildingPrefab, new Vector3(position.x, 0f, position.z), Quaternion.identity);
            selectedBuilding.GetComponent<BuildingStateMachine>().pointer = pointer;
            selectedBuilding.GetComponent<BuildingStateMachine>().Initialize(selectedBuilding);
        }

        else
        {
            selectedBuilding = null;
        }

    }
}
