using System;
using UnityEngine;

public class LoopWarp : MonoBehaviour
{
    public Transform loopStart;
    public Transform loopEnd;

    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        CheckPosition();
    }

    void CheckPosition()
    {
        float delta = player.position.y - loopEnd.position.y;
        //Debug.Log(delta);
        if (delta > 0)
        {
            player.GetComponent<PlayerBehaviour>().WarpTo(loopStart.position + Vector3.up * delta);
        }
    }
}
