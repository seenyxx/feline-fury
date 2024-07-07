using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyRotation : MonoBehaviour
{
    public Transform target;

    public float rotationSpeed = 500f;

    // Start is called before the first frame update
    private void Start()
    {
        // If target has not already be set use the player as the target
        if (!target)
            target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);
        var targetPos = Camera.main.WorldToScreenPoint(target.position);

        targetPos.x -= objectPos.x;
        targetPos.y -= objectPos.y;

        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

        var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // Rotate the turret to the target rotation at a pace set by the variable rotation speed
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
