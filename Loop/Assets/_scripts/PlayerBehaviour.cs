using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public enum Direction
    {
        none,
        up,
        down,
        left,
        right,
    }

    public float moveSpeed;
    public Transform cameraTransform;
    public float camOffset;

    public float xBoundaries;
    public Direction direction;
    void OnEnable()
    {
        InputManager.InputActions.Player.Interact.performed += Interact;
    }

    private void Update()
    {
        Move();
        AdjustCamera();
    }

    void Move()
    {
        Vector2 movement = InputManager.MovementInputValue();
        if (Mathf.Approximately(Mathf.Abs(transform.position.x), xBoundaries))
        {
            if (transform.position.x > 0 && movement.x > 0)
            {
                movement = Vector3.Normalize(new Vector2(0, movement.y));
            }
            else if (transform.position.x < 0 && movement.x < 0)
            {
                movement = Vector3.Normalize(new Vector2(0, movement.y));
            }
        }

        ChangeDirection(GetDirection(movement));
        
        transform.Translate(movement * (moveSpeed * Time.deltaTime));
        if (Mathf.Abs(transform.position.x) > xBoundaries)
        {
            if (transform.position.x > 0)
                transform.position = new Vector2(xBoundaries, transform.position.y);
            else
            {
                transform.position = new Vector2(-xBoundaries, transform.position.y);
            }
        }
    }
    Direction GetDirection(Vector2 input)
    {
        if (input == Vector2.zero)
            return Direction.none;

        if (Mathf.Abs(input.y) >= Mathf.Abs(input.x))
        {
            return input.y > 0 ? Direction.up : Direction.down;
        }
        else
        {
            return input.x > 0 ? Direction.right : Direction.left;
        }
    }

    void ChangeDirection(Direction newDir)
    {
        if (direction != newDir)
        {
            direction = newDir;
            Debug.Log("Change direction");
        }
    }

    void AdjustCamera()
    {
        // more box movement
        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) > camOffset)
        {
            if (cameraTransform.position.y > transform.position.y)
            {
                cameraTransform.position =
                    new Vector3(cameraTransform.position.x, transform.position.y + camOffset, -10);
            }            
            else
            {
                cameraTransform.position = new Vector3(cameraTransform.position.x, transform.position.y - camOffset, cameraTransform.position.z);
            }

        }
    }

    public void WarpTo(Vector2 newPos)

    {
        Vector3 camDelta = cameraTransform.position - transform.position;

        transform.position = newPos;
        cameraTransform.position = transform.position +  camDelta;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interact");
    }
    
    // Returns the last known movement direction
    public Direction GetCurrentDirection()
    {
        return direction;
    }

// Used by the pusher to slow down player when pushing
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

// Resets to normal move speed
    public void ResetMoveSpeed(float defaultSpeed)
    {
        moveSpeed = defaultSpeed;
    }

}
