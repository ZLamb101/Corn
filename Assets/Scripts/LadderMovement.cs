using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 4f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField]
    private Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if(isLadder && Mathf.Abs(vertical) > 0) 
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if(isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
            IsClimbing = true;
            if(vertical * speed <= 0f)
            {
                IsClimbingDown = true;
            }
            else
            {
                IsClimbingDown = false;
            }

        }
        else
        {
            if(IsClimbing)
            {
                IsClimbing = false;
                IsClimbingDown = false;
                rb.gravityScale = 1.5f;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    [SerializeField]
    private bool _isClimbing = false;

    public bool IsClimbing
    {
        get
        {
            return _isClimbing;
        }
        private set
        {
            _isClimbing = value;
            animator.SetBool(AnimationStrings.isClimbing, value);
        }
    }

    [SerializeField]
    private bool _isClimbingDown = false;

    public bool IsClimbingDown
    {
        get
        {
            return _isClimbingDown;
        }
        private set
        {
            _isClimbingDown = value;
            animator.SetBool(AnimationStrings.isClimbingDown, value);
        }
    }
}
