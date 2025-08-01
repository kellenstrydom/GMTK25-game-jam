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
        Vector2 delta = player.position - loopEnd.position;
        //Debug.Log(delta);
        if (delta.y > 0)
        {
            player.GetComponent<PlayerBehaviour>().WarpTo(loopStart.position + (Vector3)delta);
        }
    }
}
