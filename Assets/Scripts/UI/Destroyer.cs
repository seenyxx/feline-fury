using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Destroyer : MonoBehaviour {

    public List<PreserveInGame> preservedObjects;
    
    // Child of preservation controller to immediately trigger functions on load
    private void Start() {
        Destroy(SceneManager.GetActiveScene().name);
    }

    // Batch destruction
    public void Destroy(string sceneName)
    {
        List<PreserveInGame> removalList = new();
        foreach (PreserveInGame obj in preservedObjects)
        {
            if (obj == null) continue;

            if (obj.deletionScenes.Contains(sceneName))
            {
                if (obj.firstLoad)
                {
                    obj.firstLoad = false;
                    continue;
                }

                Destroy(GameObject.Find(obj.gameObject.name));
                removalList.Add(obj);
            }
        }

        foreach (PreserveInGame obj in removalList)
        {
            GameObject.Find("PreservationController").GetComponent<PreservationController>().preservedObjects.Remove(obj);
        }

        Destroy(gameObject);
    }
}