using Pathfinding;
using UnityEngine;

public class SetPathFindingTarget : MonoBehaviour
{
    public string targetObjName = "Player";

    // Set path finding for astar
    void Start()
    {
        AIDestinationSetter aIDestinationSetter = GetComponent<AIDestinationSetter>();

        aIDestinationSetter.target = GameObject.Find(targetObjName).transform;
    }
}
