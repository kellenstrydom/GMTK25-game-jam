using UnityEngine;

public class BoxPusher : MonoBehaviour
{
    public float pushSpeed = 1.5f;
    public float detectionDistance = 0.6f;
    public LayerMask boxLayer;

    private PlayerBehaviour player;
    private float defaultSpeed;

    public void CheckBox(Vector2 moveDir)
    {
        
        RaycastHit2D hit = default;

        // Convert input to orthogonal direction
        Vector2 dir = Vector2.zero;

        if (Mathf.Abs(moveDir.y) > Mathf.Abs(moveDir.x))
        {
            dir = moveDir.y > 0 ? Vector2.up : Vector2.down;
        }
        else if (Mathf.Abs(moveDir.x) > 0)
        {
            dir = moveDir.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return ;
        }

        hit = Physics2D.Raycast(transform.position, dir, detectionDistance, boxLayer);
        //return hit.collider != null && hit.collider.CompareTag("Box");
    }
}