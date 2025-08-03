using UnityEngine;

public class Dog : MonoBehaviour
{
    Animator animator;
    public void DogTakesBone()
    {
        Debug.Log("Dog takes bone");
        animator.SetBool("isStill", true);
        GetComponentInParent<LoopManager>().BreakLoop();
    }
}
