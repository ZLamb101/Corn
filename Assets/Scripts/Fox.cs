using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TouchingDirections))]
public class Fox : Enemy
{
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    TouchingDirections touchingDirections;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set { 
             if( _walkDirection != value)
            {
                //Direction Flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;

                } else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }   
            
            _walkDirection = value; }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget;  } private set
        {
            _hasTarget = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        touchingDirections = GetComponent<TouchingDirections>();
        enemyRef = Resources.Load("FoxEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }

        if(!base.damageable.LockVelocity) {
            if (CanMove && touchingDirections.IsGrounded)
            {
                rb.velocity = new Vector2(moveSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }  
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if (WalkDirection == WalkableDirection.Left) 
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left.");
        }
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
