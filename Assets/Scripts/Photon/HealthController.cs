using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;
using GUPS.AntiCheat.Protected;



public class HealthController : MonoBehaviour
{
    public Image fillImage;
    public ProtectedFloat health = 1;//1
    public ProtectedFloat shieldHealth = 1;//1
    public float playerHeath;

    public Rigidbody2D rb;
    //public GameObject deadsprite;
    public GameObject MainCharacter;
    public CapsuleCollider2D collider;
    public GameObject playerCanvas;

    public BansheePlayer playerScript;
    public PlayerMovementController PlayerMovementController;
    public Animator PlayerAnimator;


    public PhotonView photonView;

    private float defaultGravityScale;

    void Start()
    {
        defaultGravityScale = rb.gravityScale;
        photonView = GetComponent<PhotonView>();
        PlayerMovementController = GetComponent<PlayerMovementController>();

    }
    public void CheckHealth_old()
    {
        if(Gamemanager.instance.isSheildActive)//new code
        {
            if(shieldHealth<=0)
            {
                //this.gameObject.GetComponent<BansheePlayer>().ActivateShild(false);
                photonView.RPC("ActivateShild", RpcTarget.AllBuffered, false);//shieldhealthmethod

            }
        }//new code

        if (photonView != null && photonView.IsMine)
        {
            Gamemanager.instance.UpdateHealth(health);
        }
        if (photonView.IsMine && health <= 0)
        {
            Gamemanager.instance.UpdateHealth(0f);
            Gamemanager.instance.UpdateDeathCount();
            // Gamemanager.instance.EnableRespawn();

            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    public void CheckHealth()
    {
        if (playerScript.isSheildActive)//new code
        {
            if (shieldHealth <= 0)
            {
                //this.gameObject.GetComponent<BansheePlayer>().ActivateShild(false);
                photonView.RPC("ActivateShild", RpcTarget.AllBuffered, false);//shieldhealthmethod

            }
        }//new code

        if (photonView != null && photonView.IsMine)
            Gamemanager.instance.UpdateHealth(health);//oldcode

        if (photonView.IsMine && health <= 0)
        {

            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);

            Gamemanager.instance.UpdateHealth(0f);
            Gamemanager.instance.UpdateDeathCount();
            // Gamemanager.instance.EnableRespawn();
            
            //rpc death was here
        }
    }

    public void EnableInputs()
    {
        playerScript.DisableInputs = false;
    }
    public void DisableInputs()
    {
        playerScript.DisableInputs = true;
    }

    

    [PunRPC]
    public void death()
    {
        DisableInputs();
        playerScript.DisableInputs = true;
        rb.gravityScale = 0;
        rb.isKinematic = true;
        collider.enabled = false;
        rb.simulated = false; 
        playerCanvas.SetActive(false);

        PlayerMovementController.PlayDeathAnimation();
        //PlayerAnimator.SetBool("Dead", true);

        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
       
        //photonView.RPC("DelayedDeathActions", RpcTarget.All, PhotonNetwork.Time + 1.5f);
    }
    //Custom added scripts 
    [PunRPC]
    private void DelayedDeathActions(double executionTime)
    {
        if (PhotonNetwork.Time < executionTime)
        {
            // Schedule the delayed actions using Invoke
            float delay = (float)(executionTime - PhotonNetwork.Time);
            Invoke("CompleteDeath", delay);
        }
        else
        {
            // If the scheduled time has passed, execute immediately
            CompleteDeath();
        }
    }

    private void CompleteDeath()
    {
        // Your additional death actions after waiting for 3 seconds
        //deadsprite.gameObject.SetActive(true);
        //MainCharacter.gameObject.SetActive(false);
        //this.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);

    }

    public void RevivePlayer()
    {
        this.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
    }
     

    //waiting scripts

    [PunRPC]
    public void Revive()
    {
        health = 1;
        fillImage.fillAmount = 1f;
        rb.gravityScale = defaultGravityScale;
        rb.isKinematic = false;
        rb.simulated = true;

        PlayerMovementController.ResetDeathAnimation();
        

        MainCharacter.gameObject.SetActive(true);


        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // collider.enabled = true;

        playerCanvas.SetActive(true);
    }

    [PunRPC]
    public void ShieldHealthMethod()//old name = "ShieldHealth"
    {
        /*health = 1;
        fillImage.fillAmount = 1f;
        CheckHealth();*/
        //commented above code for test
        //
    }

[PunRPC]
    public void HealthUpdate(float damage)
    {
        print("Health Update Method");

        if(photonView.IsMine)
        {
            if (!playerScript.isSheildActive)//new code
            {
                //Debug.Log("Received Damage : " + damage);
                fillImage.fillAmount -= damage;
                health -= damage;
                //Debug.Log("Player health in HealthUpdate(damage) : Health = " + health);
                CheckHealth();
            }
            else
            {
                print("Shield health receiving damage");
                shieldHealth -= damage;
                CheckHealth();
            }
        }
        else
        {
            if (!playerScript.isSheildActive)//new code
            {
                //Debug.Log("Received Damage : " + damage);
                fillImage.fillAmount -= damage;
                health -= damage;
                //Debug.Log("Player health in HealthUpdate(damage) : Health = " + health);
                CheckHealth();
            }
            else
            {
                print("Shield health receiving damage");
                shieldHealth -= damage;
                CheckHealth();
            }
        }
        #region old code uncomment if needed
        /*if(!Gamemanager.instance.isSheildActive)//new code
        {
            //Debug.Log("Received Damage : " + damage);
            fillImage.fillAmount -= damage;
            health -= damage;
            //Debug.Log("Player health in HealthUpdate(damage) : Health = " + health);
            CheckHealth();
        }
        else
        {
            print("Shield health receiving damage");
            shieldHealth -= damage;
            CheckHealth();
        }*/
        #endregion
    }

    [PunRPC]
    public void YouGotKilledBy(string name)
    {
        Gamemanager.instance.UpdateYouGotKilledFeedText(name);
    }

    [PunRPC]
    public void YouKilled(string name)
    {
        Gamemanager.instance.UpdateYouKilledFeedText(name);
    }
}
