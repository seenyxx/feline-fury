using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationSelector : MonoBehaviour
{
    public List<RuntimeAnimatorController> animators;

    // Selects a random animation for the loading screen on the dungeon game scene
    void Start()
    {
        Animator animator = GetComponent<Animator>();

        RuntimeAnimatorController selectedAnimator = animators[Random.Range(0, animators.Count)];

        animator.runtimeAnimatorController = selectedAnimator;
    }
}
