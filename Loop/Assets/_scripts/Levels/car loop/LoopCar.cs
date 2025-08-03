using UnityEngine;

public class LoopCar : MonoBehaviour
{
    public float speed = 5f;
    public float resetX = 10f;
    public float startX = -10f;
    

    [Header("BoxCast Settings")]
    public Vector2 boxSize = new Vector2(1f, 1f);
    public Vector2 direction = Vector2.right;
    public float castDistance = 1f;
    public Vector2 castOffset = Vector2.zero;
    public LayerMask detectionLayers;

    [Header("Debug")]
    public bool showGizmos = true;
    
    
    private bool isStopped = false;
    private bool isCrashed = false;
    

    void Update()
    {
        if (isCrashed) return;

        // Raycast forward to detect objects
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionDistance);
        Vector2 origin = (Vector2)transform.position + castOffset;

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            boxSize,
            0f,
            direction.normalized,
            castDistance,
            detectionLayers
        );
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isStopped = true;
            }
            else if (hit.collider.CompareTag("Pushable"))
            {
                isCrashed = true;
                Debug.Log("Crashed into a box!");
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
                BreakLoop();
                return;
            }
        }
        else
        {
            isStopped = false;
        }

        // Move if not stopped
        if (!isStopped)
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));

            // Reset to left side
            if (transform.position.x > resetX)
            {
                Vector3 newPosition = transform.position;
                newPosition.x = startX;
                transform.position = newPosition;
            }
        }
    }

    void BreakLoop()
    {
        GetComponentInParent<LoopManager>().BreakLoop();
    }
    
    void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.cyan;

        Vector2 origin = (Vector2)transform.position + castOffset;
        Vector2 end = origin + direction.normalized * castDistance;

        // Draw start box
        Gizmos.DrawWireCube(origin, boxSize);

        // Draw end box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(end, boxSize);

        // Draw connecting lines
        Vector2 half = boxSize * 0.5f;
        Vector2[] corners =
        {
            origin + new Vector2(-half.x, -half.y),
            origin + new Vector2(-half.x,  half.y),
            origin + new Vector2( half.x,  half.y),
            origin + new Vector2( half.x, -half.y)
        };

        foreach (var corner in corners)
        {
            Gizmos.DrawLine(corner, corner + direction.normalized * castDistance);
        }
    }

}
