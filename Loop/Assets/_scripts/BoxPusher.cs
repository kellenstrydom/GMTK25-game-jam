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
    
    
    public Vector2 boxSize = new Vector2(1f, 1f); // Adjust to match your player size
    public Vector2 horizontalOffset = new Vector2(0.5f, 0f); // Offset cast origin for side pushes
    public Vector2 verticalOffset = new Vector2(0f, 0.5f);   // Offset cast origin for up/down pushes

    public AudioClip push;
    public AudioSource pushAS;

    void Start()
    {
        pushAS.clip = push;
    }

    private void Awake()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    public void CheckBox(Vector2 moveDir)
    {
        box = null;
        if (moveDir == Vector2.zero) return;

        // Check horizontal
        if (moveDir.x != 0)
        {
            Vector2 dir = new Vector2(moveDir.x, 0).normalized;
            Vector2 origin = (Vector2)transform.position + new Vector2(horizontalOffset.x * Mathf.Sign(dir.x), horizontalOffset.y);

            RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, dir, detectionDistance, pushLayer);
            if (hit.collider != null)
            {
                Debug.Log("Hit side: " + hit.collider.name);
                MoveBox(hit.collider.transform, dir);
                return;
            }
        }

        // Check vertical
        if (moveDir.y != 0)
        {
            Vector2 dir = new Vector2(0, moveDir.y).normalized;
            Vector2 origin = (Vector2)transform.position + new Vector2(verticalOffset.x, verticalOffset.y * Mathf.Sign(dir.y));

            RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, dir, detectionDistance, pushLayer);
            if (hit.collider != null)
            {
                Debug.Log("Hit vertical: " + hit.collider.name);
                MoveBox(hit.collider.transform, dir);

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
        pushAS.Play(); 
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
    
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.cyan;

        // Draw horizontal BoxCasts (left & right)
        foreach (int dirX in new int[] { -1, 1 })
        {
            Vector2 direction = new Vector2(dirX, 0);
            Vector2 origin = (Vector2)transform.position + new Vector2(horizontalOffset.x * dirX, horizontalOffset.y);
            Vector2 endPoint = origin + direction * detectionDistance;

            DrawBoxCastGizmo(origin, direction, boxSize, detectionDistance);
        }

        // Draw vertical BoxCasts (up & down)
        foreach (int dirY in new int[] { -1, 1 })
        {
            Vector2 direction = new Vector2(0, dirY);
            Vector2 origin = (Vector2)transform.position + new Vector2(verticalOffset.x, verticalOffset.y * dirY);
            Vector2 endPoint = origin + direction * detectionDistance;

            DrawBoxCastGizmo(origin, direction, boxSize, detectionDistance);
        }
    }
#endif
    
    private void DrawBoxCastGizmo(Vector2 origin, Vector2 direction, Vector2 size, float distance)
    {
        Quaternion rotation = Quaternion.identity;
        Vector2 halfExtents = size * 0.5f;

        // Calculate the center of the box after the cast
        Vector2 castCenter = origin + direction.normalized * distance;

        // Draw the original box
        Gizmos.DrawWireCube(origin, size);

        // Draw the casted box
        Gizmos.DrawWireCube(castCenter, size);

        // Draw connecting lines between the corners
        Vector2[] corners = new Vector2[4]
        {
            origin + new Vector2(-halfExtents.x, -halfExtents.y),
            origin + new Vector2(-halfExtents.x,  halfExtents.y),
            origin + new Vector2( halfExtents.x,  halfExtents.y),
            origin + new Vector2( halfExtents.x, -halfExtents.y)
        };

        Vector2 offset = direction.normalized * distance;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i] + offset);
        }
    }


}