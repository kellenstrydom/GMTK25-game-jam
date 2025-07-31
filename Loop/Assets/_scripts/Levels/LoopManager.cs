using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public LoopWarp _loopWarp;
    
    public void BreakLoop()
    {
        _loopWarp.enabled = false;
    }
}
