using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthFill;
    public Vector3 offset;

    public void SetHealth(float health, float maxHealth)
    {
        healthFill.fillAmount = health / maxHealth;
    }

    private void Start()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void LateUpdate()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // Keep the health bar horizontal
        transform.rotation = Quaternion.LookRotation(-direction);

        // Make the health bar face the camera
        //transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
    }
}
