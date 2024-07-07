using UnityEngine;

public class PlayerRelocator : MonoBehaviour
{
    // Relocates the player on start
    void Start()
    {
        GameObject.Find("Player").transform.position = transform.position;
        Destroy(gameObject);
    }
}
