using UnityEngine;
using UnityEngine.SceneManagement;

public class BindCamera : MonoBehaviour
{
    public Transform sceneCam;
    
    // Binds the position of the camera to a certain object
    private void FindCamera()
    {
        if (!sceneCam)
        {
            sceneCam = GameObject.Find("Camera").transform;
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // On scene loaded find the camera
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCamera();
    }


    // Update is called once per frame
    void Update()
    {
        if (!transform || !sceneCam) return;
        sceneCam.position = new Vector3(transform.position.x, transform.position.y, sceneCam.position.z);
    }
}
