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

    public Vector2 boxSize = new Vector2(1f, 1f);
    public Vector2 horizontalOffset = new Vector2(0.5f, 0f);
    public Vector2 verticalOffset = new Vector2(0f, 0.5f);

    [Header("Push Audio")]
    public AudioClip push;
    public AudioSource pushAS;

    private bool isPushing = false; // Tracks pushing state

    void Start()
    {
        pushAS.clip = push;
        pushAS.loop = true; // Loop the sound while pushing
    }

    private void Awake()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    public void CheckBox(Vector2 moveDir)
    {
        box = null;

        if (moveDir == Vector2.zero)
        {
            StopPushingSound();
            return;
        }

        bool boxHit = false;

        // Check horizontal
        if (moveDir.x != 0)
        {
            Vector2 dir = new Vector2(moveDir.x, 0).normalized;
            Vector2 origin = (Vector2)transform.position + new Vector2(horizontalOffset.x * Mathf.Sign(dir.x), horizontalOffset.y);

            RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, dir, detectionDistance, pushLayer);
            if (hit.collider != null)
            {
                MoveBox(hit.collider.transform, dir);
                boxHit = true;
            }
        }

        // Check vertical
        if (!boxHit && moveDir.y != 0)
        {
            Vector2 dir = new Vector2(0, moveDir.y).normalized;
            Vector2 origin = (Vector2)transform.position + new Vector2(verticalOffset.x, verticalOffset.y * Mathf.Sign(dir.y));

            RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, dir, detectionDistance, pushLayer);
            if (hit.collider != null)
            {
                MoveBox(hit.collider.transform, dir);
                boxHit = true;
            }
        }

        // If no box was hit, stop pushing sound
        if (!boxHit)
        {
            StopPushingSound();
        }
    }

    void MoveBox(Transform box, Vector2 orthDir)
    {
        this.box = box;
        box.Translate(orthDir * (player.moveSpeed * Time.deltaTime));

        StartPushingSound();
    }

    void StartPushingSound()
    {
        if (!isPushing)
        {
            isPushing = true;
            pushAS.Play();
        }
    }

    void StopPushingSound()
    {
        if (isPushing)
        {
            isPushing = false;
            pushAS.Stop();
        }
    }

    [CanBeNull]
    public Transform CheckBoxWarp(ref Vector3 boxDelta)
    {
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

        // Draw horizontal BoxCasts
        foreach (int dirX in new int[] { -1, 1 })
        {
            Vector2 direction = new Vector2(dirX, 0);
            Vector2 origin = (Vector2)transform.position + new Vector2(horizontalOffset.x * dirX, horizontalOffset.y);
            DrawBoxCastGizmo(origin, direction, boxSize, detectionDistance);
        }

        // Draw vertical BoxCasts
        foreach (int dirY in new int[] { -1, 1 })
        {
            Vector2 direction = new Vector2(0, dirY);
            Vector2 origin = (Vector2)transform.position + new Vector2(verticalOffset.x, verticalOffset.y * dirY);
            DrawBoxCastGizmo(origin, direction, boxSize, detectionDistance);
        }
    }
#endif

    private void DrawBoxCastGizmo(Vector2 origin, Vector2 direction, Vector2 size, float distance)
    {
        Vector2 halfExtents = size * 0.5f;
        Vector2 castCenter = origin + direction.normalized * distance;

        Gizmos.DrawWireCube(origin, size);
        Gizmos.DrawWireCube(castCenter, size);

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
