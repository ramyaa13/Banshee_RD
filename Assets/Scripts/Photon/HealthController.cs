using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;


public class HealthController : MonoBehaviour
{
    public Image fillImage;
    public float health = 1;
    public float playerHeath;

    public Rigidbody2D rb;
    public GameObject deadsprite;
    public GameObject MainCharacter;
    public CapsuleCollider2D collider;
    public GameObject playerCanvas;

    public BansheePlayer playerScript;
    public PlayerMovementController PlayerMovementController;
    public Animator PlayerAnimator;


    public PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        PlayerMovementController = GetComponent<PlayerMovementController>();

    }
    public void CheckHealth()
    {
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
       
        photonView.RPC("DelayedDeathActions", RpcTarget.All, PhotonNetwork.Time + 2f);
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
        deadsprite.gameObject.SetActive(true);
        MainCharacter.gameObject.SetActive(false);

    }
    //waiting scripts

    [PunRPC]
    public void Revive()
    {
        health = 1;
        fillImage.fillAmount = 1f;
        rb.gravityScale = 1;
        rb.isKinematic = false;
        rb.simulated = true;

        PlayerMovementController.ResetDeathAnimation();
        

        MainCharacter.gameObject.SetActive(true);


        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        collider.enabled = true;

        deadsprite.gameObject.SetActive(false);
        playerCanvas.SetActive(true);
    }

    [PunRPC]
    public void ShieldHealth()
    {
        health = 1;
        fillImage.fillAmount = 1f;
        CheckHealth();
    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {

        fillImage.fillAmount -= damage;
        health -= damage;
        CheckHealth();
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
