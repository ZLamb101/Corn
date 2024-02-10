using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding;
using System.IO;
using UnityEngine.InputSystem.HID;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    //Player Control
    public float walkSpeed = 2f;
    public float airWalkSpeed = 1f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    Rigidbody2D rb;
    Animator animator;

    //AI 
    public float nextWaypointDistance = 0.5f;
    private Transform target;
    Pathfinding.Path path;
    int currentWaypoint = 0;
    Seeker seeker;

    public float jumpImpulse = 5.4f;
    public bool CanMove { get 
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving , value);
        } 
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get { return _isFacingRight; } private set {
            if(_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }



    private void FixedUpdate()
    {
        if (!GameManager.instance.IsIdle)
        {
            if (!damageable.LockVelocity)
            {
               
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
                animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

            }
 
        }
        else
        {
            if (!IsAlive)
            {
                GameManager.instance.IdleChanged = true;
                return;
            }
            if (path == null || damageable.LockVelocity)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                animator.SetTrigger(AnimationStrings.attackTrigger);
                return;
            }

            if (CanMove)
            {
                findClosestEnemy();
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

               
                    rb.velocity = new Vector2(direction.x * walkSpeed, rb.velocity.y);
                    IsMoving = direction != Vector2.zero;
                
                
                //Jump if too high
                if (path.vectorPath[path.vectorPath.Count-1].y > rb.position.y)
                {
                    Jump();
                }
                //If Enemies are below, got through 1 way
                if (direction.y <= -0.85f && touchingDirections.IsGrounded)
                {
                    StartCoroutine(OneWayPlatform.instance.FallThroughCollider());
                }

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                SetFacingDirection(rb.velocity);
            }
            else
            {    
                 rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }   
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsAlive)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            StopMoving();
        }
    }

    public void StopMoving()
    {
        IsMoving = false;
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && CanMove)
            {
                if (!touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        return walkSpeed;
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else if (touchingDirections.IsOnCeiling)
                {
                    // When jumping through platforms
                    return airWalkSpeed;
                }
            }
            return 0;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            // Face the right
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            //Face the left
            IsFacingRight = false;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && GameManager.instance.IsIdle)
        {
            if (target == null)
            {
                findClosestEnemy();
            }
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void findClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in allEnemies)
        {
            float distanceToEnemy = (enemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        target = closestEnemy.transform;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove)
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnSpellSlot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.spellSlot1Trigger);
        }
    }

    public void OnShockwave(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.shockwaveTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
       rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

}
