using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    public float rotationSpeed = 6000f;
    public bool rotate = true;

    // Computest the Quaternion for rotation towards mouse
    private Quaternion GetTargetRotation()
    {
        var mousePos = Input.mousePosition;
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle + 270));
    }

    // Rotates the weapon holder towards the mouse position at a certain speed
    void Update()
    {
        if (!rotate) return;

        transform.rotation =
            Quaternion.RotateTowards
            (transform.rotation, GetTargetRotation(), rotationSpeed * Time.deltaTime);
    }


    // Instantly rotates the weapon holder towards the mouse position
    public void InstantRotate()
    {
        transform.rotation = GetTargetRotation();
    }
}
