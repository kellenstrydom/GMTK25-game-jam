using System;
using UnityEngine;

public class EndHouse : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered end house");
    }
}
