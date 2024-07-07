using UnityEngine;

public class ScanPF : MonoBehaviour {
    // Scans pathfinding on awake
    private void Awake() {
        GetComponent<AstarPath>().Scan();
    }
}