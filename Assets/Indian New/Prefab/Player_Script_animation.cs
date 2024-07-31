using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Script_animation : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float runSpeed = 25f;
    public float jumpForce = 10f;
    public float bulletForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private LayerMask groundLayer;
    private Animator animator;
    private bool isFacingRight = false;
    private bool isShooting = false;
    private bool IsJumping = false;

    public GameObject bulletPrefab;
    public Transform firePoint;
    //private bool IsGrounded;

    public Transform GroundCheck;
    //public GameObject bulletPrefab;

    public WeaponControllerLocal gunEquipController;
    public SwordLocal swordLocal;

    private readonly string P_Player = "Player";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");  // Replace "Ground" with the name of your ground layer
        animator = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(GroundCheck.position, 0.02f);
    }

    void Update()
    {
        // Shoot();
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.02f, groundLayer);
        //if (!isGrounded)
        //{
        //    animator.SetBool("IsJumping", false);
        //    print("Stop jump");
        //}

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Adjust speed based on running
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector2 moveDirection = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(moveDirection.x * currentSpeed, rb.velocity.y);

        //var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        //transform.position += move * 10 * Time.deltaTime;


        // Set walking or running animation parameter
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));

        // Set the correct animation based on the speed
        if (isRunning)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsWalking", false);
            Shoot();
        }
        else
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
            Shoot();
        }

        if (horizontalInput > 0 && !isFacingRight || horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        //if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        //{
        //    //Jump = true;
        //    //JumpHeld = true;
        //    rb.AddForce(Vector2.up * jumpForce);
        //    //isGrounded = false;
        //    //animator.SetBool("IsJumping", true);

        //}

        Jump();
        GunEquipControl();
    }


    //Gun Equip
    private void GunEquipControl()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gunEquipController.EquipWeapon();
            animator.SetFloat(P_Player, gunEquipController.animationID);
        }
    }

    private void LateUpdate()
    {
        
    }
    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        IsGrounded = true;
    //        //playerMovementController.JumpFinised();
    //    }
    //}

    //void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        IsGrounded = false;
    //    }
    //}


    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Shoot()
    {
        // Set the shooting animation parameter only when transitioning from not shooting to shooting
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Set the shooting animation trigger
            //animator.SetBool("Shoot", true);
            if (gunEquipController.IsGunEquiped() == false && gunEquipController.IsSwordEquiped() == true)
                swordLocal.EnableTrigger(true);

            animator.SetTrigger("Shoot1");

            //Shoot(transform.localScale.x > 0 ? false : true);

            gunEquipController.Shoot(transform.localScale.x > 0 ? false : true);
        }

        else if (Input.GetKeyUp(KeyCode.F))
        {
            // Reset shooting animation trigger (if needed)
            
            //animator.SetBool("Shoot", false);

        }
    }

    public void Shoot(bool isFacingRight)
    {
        //fireRate = 10f;
       
        GameObject bullet = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<BulletFireLocal>().UpdateDamage(10);


        if (isFacingRight == true)
        {
            bullet.GetComponent<BulletFireLocal>().ChangeDirection();
        }
        //

        //if (isFacingRight == true)
        //{
        //    bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        //}
        bullet.transform.parent = null;
                //Debug.Log("firieng and rate: " + fireRate + "and :" + nextFireTime);

    }

    void Jump()
    {
        // if (IsJumping)
        // {

        // Player jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            IsJumping = true;
            animator.SetBool("IsJumping", IsJumping);
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.AddForce(Vector2.up * jumpForce);
            //isGrounded = false;
        }
        else
        {
            // Reset jumping animation parameter when not jumping
            IsJumping = false;
            animator.SetBool("IsJumping", IsJumping);

        }
        // }

    }
}


