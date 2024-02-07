using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Damageable))]
public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 1.5f;
    public List<Transform> waypoints;

    Transform nextWaypoint;
    int waypointNum = 0;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    public float waypointReachedDistance = 0.1f;
    public Collider2D deathCollider;

    public ItemPickup[] itemDrops;

    public bool CanMove
    { 
        get 
        { 
            return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    private void FixedUpdate()
    {

        if(damageable.IsAlive) {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            // Facing the right
            if (rb.velocity.x < 0)
            {
                //Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            // Facing the left
            if (rb.velocity.x > 0)
            {
                //Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnDeath()
    {
        rb.gravityScale = 2;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
        ItemDrop();
        damageable.damageableDeath.RemoveListener(OnDeath);
        Debug.Log("Death here");

    }

    public void Flight()
    {
        // Fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);



        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        // See if we need to switch waypoints
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;
            if(waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void ItemDrop()
    {
        foreach (ItemPickup item in itemDrops)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
    }
}
