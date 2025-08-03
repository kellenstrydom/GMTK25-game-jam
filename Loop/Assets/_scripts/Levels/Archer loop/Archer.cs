using System;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private bool isLoop;
    [SerializeField] private bool isWaiting;
    public GameObject arrow;
    public Transform spawnPoint;
    public float arrowSpeed = 10f;
    public float loopTime;

    public float delay;

    private void Awake()
    {
        isLoop = true;
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
        GameObject obj = Instantiate(arrow, spawnPoint.position, Quaternion.identity);
        obj.GetComponent<Arrow>().InisialiseArrow(arrowSpeed,this);
        isWaiting = true;
    }

    public void ArrowMiss()
    {
        isWaiting = false;
    }

    public void ArrowHit()
    {
        BreakLoop();
    }

    void BreakLoop()
    {
        isLoop = false;
        GetComponentInParent<LoopManager>().BreakLoop();
    }
}
