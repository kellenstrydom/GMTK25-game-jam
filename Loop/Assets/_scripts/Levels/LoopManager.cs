using System;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public LoopWarp _loopWarp;
    public GameObject puzzleObject;

    private void Awake()
    {
        if (puzzleObject)
        {
            puzzleObject.SetActive(false);
        }
    }

    public void DoPuzzleObject()
    {
        if (!puzzleObject) return;
        puzzleObject.SetActive(true);
        puzzleObject = null;
    }

    public void BreakLoop()
    {
        _loopWarp.enabled = false;
    }
}
