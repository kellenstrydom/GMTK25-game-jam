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

    [Header("Audio")]
    public AudioClip walkingClip;  // assign in inspector
    public AudioSource walkingAS;  // assign in inspector
    private bool isWalking = false;

    private void Awake()
    {
        _boxPusher = GetComponent<BoxPusher>();
        _interact = GetComponent<Interact>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (walkingAS != null && walkingClip != null)
        {
            walkingAS.clip = walkingClip;
            walkingAS.loop = true; // so it repeats while walking
        }
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

    private void LateUpdate()
    {
        CheckSideWarp();
    }

    void CheckSideWarp()
    {
        Transform box = _boxPusher.box;
        Vector3 boxDelta = Vector2.zero;
        if (box != null)
        {
            boxDelta = box.position - transform.position;
        }
        
        if (Mathf.Abs(transform.position.x) > xBoundaries)
        {
            if (transform.position.x > 0)
                transform.position += Vector3.left * (2 * xBoundaries);
            else
                transform.position -= Vector3.left * (2 * xBoundaries);
        }

        if (box)
        {
            box.position = transform.position + boxDelta;
        }
    }

    void Move()
    {
        Vector2 movement = InputManager.MovementInputValue();

        // --- walking sound control ---
        bool movingNow = movement != Vector2.zero;
        if (movingNow && !isWalking)
        {
            isWalking = true;
            if (walkingAS != null && !walkingAS.isPlaying)
                walkingAS.Play();
        }
        else if (!movingNow && isWalking)
        {
            isWalking = false;
            if (walkingAS != null && walkingAS.isPlaying)
                walkingAS.Stop();
        }
        // ----------------------------

        // if (Mathf.Approximately(Mathf.Abs(transform.position.x), xBoundaries))
        // {
        //     if (transform.position.x > 0 && movement.x > 0)
        //     {
        //         movement = Vector3.Normalize(new Vector2(0, movement.y));
        //     }
        //     else if (transform.position.x < 0 && movement.x < 0)
        //     {
        //         movement = Vector3.Normalize(new Vector2(0, movement.y));
        //     }
        // }

        _boxPusher.CheckBox(movement);
        ChangeDirection(GetDirection(movement));
        transform.Translate(movement * (moveSpeed * Time.deltaTime));

        // if (Mathf.Abs(transform.position.x) > xBoundaries)
        // {
        //     if (transform.position.x > 0)
        //         transform.position = new Vector2(xBoundaries, transform.position.y);
        //     else
        //         transform.position = new Vector2(-xBoundaries, transform.position.y);
        // }
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

    public void ChangeDirection(Direction newDir)
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
                animator.SetBool("isBack", true);
                break;
            case Direction.down:
                animator.SetBool("isFront", true);
                break;
            case Direction.left:
            case Direction.right:
                animator.SetBool("isSide", true);
                break;
        }

        if (direction == Direction.right)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void AdjustCamera()
    {
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
        Vector3 camDelta = cameraTransform.position - transform.position;
        Vector3 boxDelta = Vector3.zero;

        Transform box = _boxPusher.CheckBoxWarp(ref boxDelta);

        if (box == null)
            loopWarp.CheckWarpPush();
        else
            loopWarp.isPushByWarp = false;

        transform.position = newPos;
        cameraTransform.position = transform.position + camDelta;

        if (box != null)
            box.position = boxDelta + transform.position;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        _interact.InteractWithObject();
    }

    public Direction GetCurrentDirection()
    {
        return direction;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetMoveSpeed(float defaultSpeed)
    {
        moveSpeed = defaultSpeed;
    }
}
