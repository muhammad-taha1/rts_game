using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PointerController : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update

    private Plane plane;
    void Start()
    {
        // Plane at y = 0 (xz-plane)
        plane = new Plane(Vector3.up, Vector3.zero);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //var position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //print("pointer loc: "  + position);

        //rb.position = new Vector3(position.x, 0.2f, position.z + position.y);


        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to a ray
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

  

        // Raycast to the plane
        if (plane.Raycast(ray, out float distance))
        {
            // Get the point where the ray intersects the plane
            Vector3 worldPosition = ray.GetPoint(distance);

            // Update the position of the object to follow the mouse
            rb.position = new Vector3(worldPosition.x, 0.2f, worldPosition.z);
        }

    }
}
