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

    Rigidbody2D rb;
        
    [Header("connections")]
    public BoxPusher _boxPusher;
    public Interact _interact;
    
    public Animator animator;

    private void Awake()
    {
        _boxPusher = GetComponent<BoxPusher>();
        _interact = GetComponent<Interact>();
        cameraTransform = Camera.main.transform;

        animator = GetComponent<Animator>();

    }

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
        
        _boxPusher.CheckBox(movement);

        ChangeDirection(GetDirection(movement));
        
        // Vector2 newPos = rb.position + movement * (moveSpeed * Time.deltaTime);;
        // rb.MovePosition(newPos);
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
        if (direction == newDir) return;
        direction = newDir;

        // Reset all parameters first
        animator.SetBool("isFront", false);
        animator.SetBool("isBack", false);
        animator.SetBool("isSide", false);

        // Set the correct one
        switch (newDir)
        {
            case Direction.up:
                animator.SetBool("isBack", true);    // walking up → back
                break;
            case Direction.down:
                animator.SetBool("isFront", true);   // walking down → front
                break;
            case Direction.left:
            case Direction.right:
                animator.SetBool("isSide", true);    // left/right → side
                break;
        }
        
        if (direction == Direction.right)
            transform.localScale = new Vector3(-1, 1, 1);
        else 
            transform.localScale = new Vector3(1, 1, 1);
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

    public void WarpTo(Vector2 newPos, LoopWarp loopWarp)

    {
        //Debug.Log("Warping to: " + newPos);

        Debug.Log(cameraTransform.name);
        Vector3 camDelta = cameraTransform.position - transform.position;
        
        Vector3 boxDelta = Vector3.zero;
        
        Transform box = _boxPusher.CheckBoxWarp(ref boxDelta);

        if (box == null)
        {
            loopWarp.CheckWarpPush();
        }
        else
        {
            loopWarp.isPushByWarp = false;
        }
        

        transform.position = newPos;

        cameraTransform.position = transform.position +  camDelta;


        if (box != null)
        {
            box.position = boxDelta + transform.position;
        }
        
        // do for things leading
        
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interact");
        _interact.InteractWithObject();
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
