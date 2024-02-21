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

    private void Start()
    {
        delayOnRespawn = 15;
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
                rb.velocity = new Vector2(moveSpeed * base.walkDirectionVector.x, rb.velocity.y);
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
