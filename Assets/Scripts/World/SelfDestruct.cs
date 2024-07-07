using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destroyDelay = 1f;

    void Start()
    {
        // Self destructs after a certain amount of time
        StartCoroutine(Delete());
    }

    // Delayed self destruct
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
