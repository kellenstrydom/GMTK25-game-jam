using System;
using UnityEngine;

public class Dog : MonoBehaviour
{
    Animator animator;

    public AudioClip bark;
    public AudioSource dogAS;

    private void Start()
    {
        dogAS.clip = bark; 
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DogTakesBone()
    {
        Debug.Log("Dog takes bone");
        animator.SetBool("isStill", true);
        dogAS.Play(); 
        GetComponentInParent<LoopManager>().BreakLoop();
        GetComponentsInChildren<Transform>()[1].gameObject.SetActive(false);
    }
}
