using System;
using UnityEngine;

public class LoopWarp : MonoBehaviour
{
    LoopManager _loopManager;
    public Transform loopStart;
    public Transform loopEnd;

    public Transform player;
    
    public Transform pushable;

    public bool isPushByWarp = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _loopManager = transform.GetComponentInParent<LoopManager>();
        //Transform pushable = GetChildWithTag(transform.parent, "Pushable");

    }

    private void LateUpdate()
    {
        CheckPosition();
        WarpPush();
        
    }

    void WarpPush()
    {
        if (!isPushByWarp) return;
        
        
        // check if closer to end or start
        bool isPlayerByEnd = (Vector2.Distance(player.position, loopEnd.position) <
                              Vector2.Distance(player.position, loopStart.position));
        bool isPushByEnd = (Vector2.Distance(pushable.position, loopEnd.position) <
                             Vector2.Distance(pushable.position, loopStart.position));
        
        Debug.Log($"push by end: {isPushByEnd} /n player by end: {isPlayerByEnd}");
        
        if (isPlayerByEnd == isPushByEnd) return;
        if (IsOnScreen(pushable,Camera.main))
        {
            isPushByWarp = false;
            return;
        }
        
        Transform target = isPlayerByEnd ? loopEnd : loopStart;
        Transform current = isPushByEnd ? loopEnd : loopStart;
        
        Debug.Log($"Target: {target.name} -- current: {current.name}");
        
        Vector3 pushableDelta = pushable.position - current.position;
        pushable.position = target.position + pushableDelta;

    }

    void CheckPosition()
    {
        Vector2 delta = player.position - loopEnd.position;

        
        if (!(delta.y < 0)) return;
        player.GetComponent<PlayerBehaviour>().WarpTo(loopStart.position + (Vector3)delta, this);
        
        _loopManager.DoPuzzleObject();
        
    }

    public void CheckWarpPush()
    {
        if (pushable == null) return;

        isPushByWarp = IsOnScreen(pushable, Camera.main);

        if (isPushByWarp)
        {
            Vector3 pushableDelta = pushable.position - loopEnd.position;
            pushable.position = loopStart.position + pushableDelta;
        }
    }
    
    Transform GetChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null; // Not found
    }
    
    bool IsOnScreen(Transform obj, Camera cam)
    {
        if (obj == null || cam == null) return false;

        Vector3 viewportPoint = cam.WorldToViewportPoint(obj.position);
        //Debug.Log(viewportPoint);
        return viewportPoint.z > 0 &&
               viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}
