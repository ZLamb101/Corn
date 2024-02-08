using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEye : Enemy
{
    public List<Transform> waypoints;

    Transform nextWaypoint;
    int waypointNum = 0;

    public float waypointReachedDistance = 0.1f;
    public Collider2D deathCollider;

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
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

    public override void OnDeath()
    {
        base.OnDeath();
        rb.gravityScale = 2;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }

    public void Flight()
    {
        // Fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);



        rb.velocity = directionToWaypoint * moveSpeed;
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
}
