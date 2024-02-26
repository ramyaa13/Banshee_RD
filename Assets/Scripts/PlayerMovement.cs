//working

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     private float horizontal;
//     public float speed = 8f;
//     public float jumpingPower = 10f;
//     private bool isfacingRight = false;

//     [SerializeField] private Rigidbody2D rb;
//     [SerializeField] private Transform groundCheck;
//     [SerializeField] private LayerMask groundLayer;




//     // Update is called once per frame
//     void Update()
//     {
//         horizontal = Input.GetAxisRaw("Horizontal");

//         if(Input.GetButtonDown("Jump") && IsGrounded())
//         {
//             rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
//         }

//         if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
//         }

//         Flip();
//     }

//     void FixedUpdate()
//     {
//         rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

//     }

//     private bool IsGrounded()
//     {
//         return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//     }

//     private void Flip()
//     {
//         if (isfacingRight && horizontal < 0f || !isfacingRight && horizontal > 0f)
//         {
//             isfacingRight = !isfacingRight;
//             Vector3 localScale = transform.localScale;
//             localScale.x *= -1f;
//             transform.localScale = localScale; 
//         }
//     }
// }





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 10f;
    private bool isfacingRight = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator; // Reference to the Animator component

    private const string RUN_TRIGGER = "isMoving";

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Debug.Log("Jump key pressed; IsGrounded: " + IsGrounded());
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
        UpdateAnimation(); // Call the function to update the animation state
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Debug.Log("IsGrounded: " + grounded);
        return grounded;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }


    private void Flip()
    {
        if (isfacingRight && horizontal < 0f || !isfacingRight && horizontal > 0f)
        {
            isfacingRight = !isfacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void UpdateAnimation()
    {

        bool isRunning = Mathf.Abs(horizontal) > 0.1f;
        animator.SetBool(RUN_TRIGGER, isRunning);
    }
}
