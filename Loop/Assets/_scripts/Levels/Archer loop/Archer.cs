    using System;
using System.Collections;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private bool isLoop;
    [SerializeField] private bool isWaiting;
    public GameObject arrow;
    public Transform spawnPoint;
    public float arrowSpeed = 10f;
    public float loopTime;
    
    public float shootDelay = 1f; // adjustable in inspector
    
    Animator animator;

    private void Awake()
    {
        isLoop = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isLoop) return;
        if (isWaiting) return;
        // shoot
        Shoot();
    }
    
    void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        isWaiting = true;
        animator.SetBool("isMiss", false);
        yield return new WaitForSeconds(shootDelay);

        GameObject obj = Instantiate(arrow, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Arrow>().InisialiseArrow(arrowSpeed, this);
        
        animator.SetBool("isMiss", true);
    }


    public void ArrowMiss()
    {
        isWaiting = false;
    }

    public void ArrowHit()
    {
        BreakLoop();
        animator.SetBool("isHit", true);
        animator.SetBool("isMiss", false);
    }

    void BreakLoop()
    {
        isLoop = false;
        GetComponentInParent<LoopManager>().BreakLoop();
    }
}
