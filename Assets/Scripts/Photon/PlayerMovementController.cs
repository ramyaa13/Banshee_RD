using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;
using Photon.Realtime;
using GUPS.AntiCheat.Protected;

/// <summary>
/// A component that handles all player movement.
/// </summary>
public class PlayerMovementController : MonoBehaviour, IPunObservable
{

    [Header("Movement")]
    //public float MovementSpeed = 100.0f;
    [SerializeField] ProtectedFloat GroundedBufferTime = 0.15f;//0.25
    [SerializeField] ProtectedFloat walkSpeed = 10f;//9
    [SerializeField] ProtectedFloat runSpeed = 25f;//13
    // public ParticleSystem FootstepParticles;

    [Header("Jumping")]
    public float JumpBufferTime = 0.5f;//2.5
    [SerializeField] ProtectedFloat JumpForce = 400.0f;//1600
    private bool IsJumping = false;
    //public float GravityScale = 100.0f;
    //public float FallGravityMultiplier = 3.0f;
    // public ParticleSystem LandingParticles;

    [Header("Ground Collision")]
    public Transform FeetPosition;
    public LayerMask GroundLayer;

    [Header("Projectile Collision")]
    //  public ParticleSystem BloodParticles;
    public float KnockbackForce = 2000f;//2000
    public float KnockbackTime = 0.25f;//0.25

    private Rigidbody2D rb;
    public Animator PlayerAnimator;

    //public RuntimeAnimatorController IdleAnimatorController;
    //public RuntimeAnimatorController GunHoldAnimatorController;
    //public RuntimeAnimatorController katanaHoldAnimatorController;

    private float horizontalMovement;
    private int direction = 1;
    private bool jump;
    private bool jumpHeld;
    private bool isGrounded;
    //private float jumpTimer;
    //private float groundedTimer;
    private float knockbackTimer = 0f;
    //private bool falling;
    private PhotonView photonView;

    public float ResetTimeAmount = 1f;//0.3
    private bool DisableInputs;
    //private bool isRunning;
    //private bool isDead;
    private bool Idle;
    private float horizontalInput;

    private Vector3 smoothMovement;
    private ProtectedFloat currentSpeed;
    private readonly string P_Player = "Player";


    // private ParticleSystem.EmissionModule footstepEmission;

    /// <summary>
    /// Called by Unity when this GameObject starts.
    /// </summary>
    ///
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }


    private void Start()
    {
       
        //PlayerAnimator.runtimeAnimatorController = IdleAnimatorController;
        // footstepEmission = FootstepParticles.emission;

    }

 
    /// <summary>
    /// Called by Unity every frame.
    /// </summary>
    private void Update()
    {

        if (photonView.IsMine && DisableInputs == true)
        {
            PlayerAnimator.SetFloat("Speed", 0f);
            PlayerAnimator.SetBool("IsRunning", false);
            //PlayerAnimator.SetBool("IsWalking", false);
            PlayerAnimator.SetBool("IsJumping", false);
            PlayerAnimator.SetBool("Shoot", false);
            runSpeed = 0f;
            //isRunning = false;
            
            horizontalInput = 0f;
        }

        if (DisableInputs == false)
        {
            if (photonView.IsMine)
            {
                //runSpeed = 40f;
                //PlayerAnimator.SetBool("Dead", false);


                //bool isRunning = Mathf.Abs(horizontalMovement) > 0.1f;
                //PlayerAnimator.SetBool("isMoving", isRunning);
                //PlayerAnimator.SetFloat("Horizontal", Mathf.Abs(horizontalMovement));

                //isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.02f, groundLayer);
                isGrounded = Physics2D.OverlapCircle(FeetPosition.transform.position, 0.05f, GroundLayer);
                //if(Gamemanager.instance != null)
                //    Gamemanager.instance.ShowMessage(isGrounded ? "Grounded" : "In Air");

                // Player movement
                horizontalInput = Input.GetAxis("Horizontal");

                if (Gamemanager.instance.LocalPlayer.isBoostActive)
                {
                    if (horizontalInput != 0)
                        rb.gameObject.GetComponent<BansheePlayer>().speedBostPS.SetActive(true);
                    else
                        rb.gameObject.GetComponent<BansheePlayer>().speedBostPS.SetActive(false);

                    currentSpeed = runSpeed; // 0 OR 1
                }
                else
                {
                    currentSpeed = walkSpeed; // 0 OR 1
                }

                Vector2 moveDirection = new Vector2(horizontalInput, 0f);
                rb.velocity = new Vector2(moveDirection.x * currentSpeed, rb.velocity.y); // Check FM

             
                PlayerAnimator.SetFloat("Speed", Mathf.Abs(moveDirection.x));
               
                PlayerAnimator.SetBool("IsRunning", true);
                //PlayerAnimator.SetBool("IsWalking", false);
                Shoot();
             
                //idle
                if (horizontalInput == 0)
                    PlayerAnimator.SetBool("IsRunning", false);

                CheckIfGrounded();
            }
        }
    }

    public void ChangeAnimation(int animationID)
    {
        PlayerAnimator.SetFloat(P_Player, animationID);
    }

    public void Shoot()
    {
        // Set the shooting animation parameter only when transitioning from not shooting to shooting
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Set the shooting animation trigger
            //PlayerAnimator.SetBool("Shoot1", true);
            PlayerAnimator.SetTrigger("Shoot1");

        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            // Reset shooting animation trigger (if needed)

            //PlayerAnimator.SetBool("Shoot1", false);
        }

        if(Input.GetMouseButtonDown(0))
            PlayerAnimator.SetTrigger("Shoot1");
        //PlayerAnimator.SetBool("Shoot1", true);
        else if (Input.GetMouseButtonUp(0))
            PlayerAnimator.SetBool("Shoot1", false);

    }

    private void CheckIfGrounded()
    {
        Jump();
    }

    private void HandleMovement()
    {
        // If the time has not surpassed the existing knockback timer, don't allow the player to move horizontally yet.
        if (Time.time < knockbackTimer)
        {
            return;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {

            IsJumping = true;
            PlayerAnimator.SetBool("IsJumping", IsJumping);
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.AddForce(Vector2.up * JumpForce);
            //isGrounded = false;
        }
        else
        {
            // Reset jumping animation parameter when not jumping
            IsJumping = false;
            PlayerAnimator.SetBool("IsJumping", IsJumping);
        }
    }
   
   
    public void SetHorizontalMovement(float value)
    {
        horizontalMovement = value;
    }

  
    public void SetJump(bool value)
    {
        //jump = value;
        Jump();
    }

    public void JumpFinised()
    {
        IsJumping = false;
    }
    public void SetEnableDisableInputs(bool value)
    {
        DisableInputs = value;
    }
  
    public void SetJumpHeld(bool value)
    {
        jumpHeld = value;
    }
   
    public int GetDirection()
    {
        return direction;
    }
 
    public void PlayDeathAnimation()
    {
        //PlayerAnimator.SetTrigger("Died");

        PlayerAnimator.SetBool("Dead", true);
        PlayerAnimator.SetBool("Idle", false);
    }

    public void ResetDeathAnimation()
    {
        //PlayerAnimator.SetBool("Dead", false);
        PlayerAnimator.SetBool("Idle",true);
        PlayerAnimator.SetBool("Dead", false);
        ChangeAnimation(0);
        //PlayerAnimator.SetFloat("Player", 0f);
    }

    // Synchronize data across the network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Writing our own velocity to the network
            stream.SendNext(transform.position);
            stream.SendNext(rb.velocity);
        }
        else if(stream.IsReading)
        {
            // Receiving velocity from the network
            transform.position = (Vector3)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();
        }
    }

    #region Extra code
    /// <summary>
    /// Called by Unity at a fixed tick rate based on the physics settings.
    /// </summary>
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //CheckIfFalling();
            //CheckIfGrounded();
            // HandleMovement();
            //HandleJumping();
            //ModifyPhysics();
        }
    }


    //public void PlayerHitsbyBullet(int direction)
    //{
    //    // Add a knockback force to this player based on the direction the projectile was travelling.
    //    r.velocity = new Vector2(0, r.velocity.y);
    //    r.AddForce(new Vector2(direction * KnockbackForce, 0), ForceMode2D.Impulse);
    //    knockbackTimer = Time.time + KnockbackTime;

    //    // Play the blood particle effect.
    //    // BloodParticles.Play();

    //    // Trigger the hit animation.
    //    //PlayerAnimator.SetTrigger("Hit");
    //}

  

  
  
    #endregion
}

/// <summary>
/// Checks to see if the player is falling and sets the falling variable.
/// </summary>
/* private void CheckIfFalling()
 {
     // Cache the current state of the falling variable.
     var wasFalling = falling == true;

     // Set the new falling state based on whether the player's y velocity is below 0.
     falling = rb.velocity.y < 0;

     // If the player is not falling, trigger the Land animation event.
     // We don't check if they were already falling here due to a bug with the animation controller that sometimes locks the player in a falling animation
     // if they hold left/right when landing on a platform.
     if (!falling)
     {
         // PlayerAnimator.SetTrigger("Land");

     }

     // If the player wasn't falling but is now falling, trigger the Fall animation event.
     if (!wasFalling && falling)
     {
         // PlayerAnimator.SetTrigger("Fall");

     }
 }*/


/// <summary>
/// Modifies the player's RigidBody2D `gravityScale` based on their current state.
/// </summary>
/* private void ModifyPhysics()
 {
     // If the player is grounded, set their gravityScale to 0.
     if (isGrounded)
     {
         rb.gravityScale = 0;
     }
     else
     {
         // Otherwise, reset the gravity scale.
         rb.gravityScale = GravityScale;

         // If the player is falling, apply the fall gravity multiplier.
         if (rb.velocity.y < 0)
         {
             rb.gravityScale = GravityScale * FallGravityMultiplier;
         }
         // If the player is jumping and holding the jump key, apply half the default gravity scale to allow for a higher jump.
         else if (rb.velocity.y > 0 && jumpHeld)
         {
             rb.gravityScale = GravityScale / 2;
         }
     }
 }*/



/// <summary>
/// Sets the player's y velocity based on whether the player has initated a jump.
/// </summary>
//private void HandleJumping_()
//{
// If the `jumpTimer` is greater than the current time and the groundedTimer is greater than 0 then the player has requested to jump and they're still allowed to
//// so activate the jump.
//if (jumpTimer > Time.time && groundedTimer > 0)
//{
//    // Set the player's new velocity based on their existing x velocity and the JumpForce.
//    rb.velocity = new Vector2(rb.velocity.x, JumpForce);

//    ResetTimeAmount -= Time.deltaTime;

//    if (ResetTimeAmount <= 0)
//    {
//        // Reset the jumpTimer and groundedTimer.
//        jumpTimer = 0;
//        groundedTimer = 0;
//        ResetTimeAmount = 0.2f;

//    }


//    IsJumping = true;
//    // Play the jump animation.
//    //PlayerAnimator.SetTrigger("Jump");


//}
//else
//{
//    IsJumping = false;

//}
//}
