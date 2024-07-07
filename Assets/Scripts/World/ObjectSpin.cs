using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    public float rotateSpeed = 20f;
    public Vector3 rotation;
    // Update is called once per frame
    void FixedUpdate()
    {
        // Constantly rotates an object at a certain speed
        transform.Rotate(rotateSpeed * Time.fixedDeltaTime * rotation);
    }
}
