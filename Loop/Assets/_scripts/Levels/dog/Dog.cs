using System;
using UnityEngine;

public class Dog : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DogTakesBone()
    {
        Debug.Log("Dog takes bone");
        animator.SetBool("isStill", true);
        GetComponentInParent<LoopManager>().BreakLoop();
        GetComponentsInChildren<Transform>()[1].gameObject.SetActive(false);
    }
}
