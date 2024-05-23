using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class SelectedState : IState
{
    private readonly GameObject _object;
    private readonly BuildingStateMachine _stateMachine;
    private readonly Material _originalMaterial;
    private readonly Material _translucentMaterial;
    private readonly Material _invalidBuildingMaterial;
    private readonly GameObject _pointer;
    private bool _isInContact = false;

    public float stepAngle = 15f;
    private LayerMask _terrainLayer;
    public float maxSlopeAngle = 20f;

    private bool isPlacementValid = true;

    public SelectedState(GameObject obj, BuildingStateMachine stateMachine, GameObject pointer, LayerMask terrainLayer)
    {
        _object = obj;
        _stateMachine = stateMachine;
        Renderer renderer = _object.GetComponent<Renderer>();
        _originalMaterial = new Material(renderer.material);


        _translucentMaterial = new Material(renderer.material);

        _translucentMaterial.SetFloat("_Mode", 3); // 3 corresponds to the Transparent mode in Unity's standard shader
        _translucentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _translucentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _translucentMaterial.SetInt("_ZWrite", 0);
        _translucentMaterial.DisableKeyword("_ALPHATEST_ON");
        _translucentMaterial.EnableKeyword("_ALPHABLEND_ON");
        _translucentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _translucentMaterial.renderQueue = 3000;
        // Set the alpha value of the material's color
        Color color = _translucentMaterial.color;
        color.a = 0.3f;
        _translucentMaterial.color = color;


        _invalidBuildingMaterial = new Material(renderer.material);

        _invalidBuildingMaterial.SetFloat("_Mode", 3); // 3 corresponds to the Transparent mode in Unity's standard shader
        _invalidBuildingMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _invalidBuildingMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _invalidBuildingMaterial.SetInt("_ZWrite", 0);
        _invalidBuildingMaterial.DisableKeyword("_ALPHATEST_ON");
        _invalidBuildingMaterial.EnableKeyword("_ALPHABLEND_ON");
        _invalidBuildingMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _invalidBuildingMaterial.renderQueue = 3000;
        Color invalidBuildingColor = Color.red;
        invalidBuildingColor.a = 0.3f;
        _invalidBuildingMaterial.color = invalidBuildingColor;


        _pointer = pointer;

        _terrainLayer = terrainLayer;
    }

    public void Enter()
    {
        SetTransparency(_translucentMaterial);
    }

    public void Exit()
    {
        SetTransparency(_originalMaterial);
    }

    public void Update()
    {
        followPointer();
        rotateObject();

        if (!isPlacementValid)
        {
            SetTransparency(_invalidBuildingMaterial);
        }

        else
        {
            SetTransparency(_translucentMaterial);
        }

        if (isPlacementValid && Input.GetButtonDown(KeyMappings.LeftClick))
        {
            _stateMachine.ChangeState(BuildingState.Placed);
        }
        else if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown(KeyMappings.RightClick))
        {
            Object.Destroy(_object);
        }
    }

    private void rotateObject()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");


        if (mouseScroll != 0)
        {
            float rotationStep = mouseScroll > 0 ? stepAngle : -stepAngle;
            _object.gameObject.transform.Rotate(Vector3.up, rotationStep);

        }
    }



    private void followPointer()
    {
        var position = _pointer.gameObject.transform.position;

        var currentPosition = _object.gameObject.transform.position;

        float highestY = 0;

        Ray rayDown = new Ray(currentPosition, Vector3.down);
        RaycastHit hitDown;


        //Ray rayUp = new Ray(currentPosition - Vector3.up, Vector3.up);
        //RaycastHit hitUp;


        if (Physics.Raycast(rayDown, out hitDown, 200f, _terrainLayer))
        {
            //Debug.Log("hitDown.point.y " +hitDown.point.y);
            Debug.DrawRay(rayDown.origin, 10f * rayDown.direction, Color.blue);
            highestY = hitDown.point.y;
        }
        //if (Physics.Raycast(rayUp, out hitUp, 200f))
        //{
        //    Debug.Log("hitUp.point.y " +hitUp.point.y);
        //    Debug.DrawRay(rayUp.origin, rayUp.direction  * 10f, Color.green);
        //    if (hitUp.point.y > highestY)
        //    {
        //        highestY = hitUp.point.y;
        //    }
        //    hitTerrain = true;
        //}

   
        var yPosition = highestY + 1;

        //Debug.Log(yPosition);
        _object.gameObject.transform.position = new Vector3(position.x, yPosition, position.z);



        isPlacementValid = canBePlaced();
    }

    private bool canBePlaced()
    {
        bool anglePlacementSatisfied = true;
        Ray ray = Camera.main.ScreenPointToRay(_object.gameObject.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _terrainLayer))
        {
            Vector3 surfaceNormal = hit.normal;
            float slopeAngle = Vector3.Angle(surfaceNormal, Vector3.up);

            //Debug.Log("slopeAngle: " + slopeAngle);

            if (slopeAngle <= maxSlopeAngle)
            {
                // Place the object at the hit point
                //Debug.Log("Object placed successfully.");
                anglePlacementSatisfied = true;
            }
            else
            {
                anglePlacementSatisfied = false;
            }

        }
       

        _object.gameObject.GetComponent<BoxCollider>();


        Debug.Log("is in contact: " + _isInContact);
        Debug.Log("anglePlacementSatisfiedt: " + anglePlacementSatisfied);

        // building can be placed if angle req is met object not in contact with anything else on sides
        return anglePlacementSatisfied && !_isInContact;

    }

    private void SetTransparency(Material material)
    {
        Renderer renderer = _object.GetComponent<Renderer>();
        renderer.material = material;
        //_object.GetComponent<Renderer>().material.color = color;
    }

    public void updateIsInContact(bool isInContact)
    {
        _isInContact = isInContact;
    }
}
