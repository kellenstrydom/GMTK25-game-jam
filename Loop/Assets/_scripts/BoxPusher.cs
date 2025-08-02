using System;
using JetBrains.Annotations;
using UnityEngine;

public class BoxPusher : MonoBehaviour
{
    public float pushSpeed = 1.5f;
    public float detectionDistance = 0.6f;
    public LayerMask pushLayer;

    private PlayerBehaviour player;
    private float defaultSpeed;

    public Transform box;

    private void Awake()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    public void CheckBox(Vector2 moveDir)
    {
        //Debug.Log(moveDir);
        RaycastHit2D hit = default;
        box = null;
        if (moveDir == Vector2.zero) return;
        
        // Convert input to orthogonal direction
        Vector2 horizontal = new Vector2(moveDir.x, 0);
        Vector2 vertical = new Vector2(0, moveDir.y);
        
        hit = Physics2D.Raycast(transform.position, horizontal.normalized, detectionDistance, pushLayer);
        
        if (hit.collider != null && horizontal != Vector2.zero)
        {
            Debug.Log("Hit side: " + hit.collider.name );
            MoveBox(hit.collider.transform, horizontal);
            return;
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, vertical, detectionDistance, pushLayer);
            if (hit.collider != null && vertical != Vector2.zero)
            {
                Debug.Log("Hit up: " + hit.collider.name);
                MoveBox(hit.collider.transform, vertical);
                return;
            }
            
        }
    }

    void MoveBox(Transform box, Vector2 orthDir)
    {
        Debug.Log(box);
        this.box = box;
        //transform.Translate(moveDir * pushSpeed * Time.deltaTime);
        box.Translate(orthDir * (player.moveSpeed * Time.deltaTime));
    }

    [CanBeNull]
    public Transform CheckBoxWarp(ref Vector3 boxDelta)
    {
        Debug.Log($"checking warp: {box}");
        if (box == null)
        {
            boxDelta = Vector3.zero;
            return box;
        }
        
        boxDelta = box.position - transform.position;
        return box;
        
    }
}