using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public ContactFilter2D groundCastFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.1f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    Rigidbody2D rb;

    [SerializeField]
    private bool _isGrounded;
    public bool IsGrounded { get
        {
            return _isGrounded;
        } 
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.IsGrounded, value);
        } 
    }

     [SerializeField]
    private bool _isOnWall;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsOnWall { get
        {
            return _isOnWall;
        } private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.IsOnWall, value);
        } 
    }

     [SerializeField]
    private bool _isOnCeiling;
   
    public bool IsOnCeiling { get
        {
            return _isOnCeiling;
        } private set
        {
             _isOnCeiling = value;
             animator.SetBool(AnimationStrings.IsOnCeiling, value);     
        } 
    }

    private void Awake()
   {
        animator = GetComponent<Animator>();
        touchingCol = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
   }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        IsGrounded = touchingCol.Cast(Vector2.down, groundCastFilter, groundHits, groundDistance) > 0;
    }
}
