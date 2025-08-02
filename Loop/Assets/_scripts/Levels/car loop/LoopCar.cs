using UnityEngine;

public class LoopCar : MonoBehaviour
{
    public float speed = 5f;
    public float resetX = 10f;
    public float startX = -10f;
    public float detectionDistance = 1.0f;
    public LayerMask detectionLayers;

    private bool isStopped = false;
    private bool isCrashed = false;

    void Update()
    {
        if (isCrashed) return;

        // Raycast forward to detect objects
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionDistance);

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
}
