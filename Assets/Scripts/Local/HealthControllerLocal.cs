using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;
using GUPS.AntiCheat.Protected;


public class HealthControllerLocal : MonoBehaviour
{
    public Image fillImage;
    public  ProtectedFloat health = 1;
    public float playerHeath;

    public Rigidbody2D rb;
    public CapsuleCollider2D collider;
    public GameObject playerCanvas;

    public Animator PlayerAnimator;



    void Start()
    {

    }
    public void CheckHealth()
    {
        if (health <= 0)
            CompleteDeath();
    }

    public void EnableInputs()
    {
    }
    public void DisableInputs()
    {
    }

    

    public void death()
    {
        DisableInputs();
        rb.gravityScale = 0;
        rb.isKinematic = true;
        collider.enabled = false;
        rb.simulated = false;
        playerCanvas.SetActive(false);

        //PlayerAnimator.SetBool("Dead", true);

        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
       
    }
    //Custom added scripts 
    public void DelayedDeathActions(double executionTime)
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
        }
            CompleteDeath();
    }

    public void CompleteDeath()
    {
        // Your additional death actions after waiting for 3 seconds
        //this.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        
        PlayerAnimator.SetBool("Dead", true);
        PlayerAnimator.SetBool("Idle", false);
    }
    //waiting scripts

    public void Revive()
    {
        health = 1;
        fillImage.fillAmount = 1f;
        rb.gravityScale = 1;
        rb.isKinematic = false;
        rb.simulated = true;




        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        collider.enabled = true;

        playerCanvas.SetActive(true);
    }

    public void ShieldHealth()
    {
        health = 1;
        fillImage.fillAmount = 1f;
        CheckHealth();
    }

    public void HealthUpdate(float damage)
    {
        print(damage);
        if(Gamemanager.instance.enemyDied)
        {
            fillImage.fillAmount -= damage;
            health -= damage;
        }
            CheckHealth();        
    }

    public void YouGotKilledBy(string name)
    {
        Gamemanager.instance.UpdateYouGotKilledFeedText(name);
    }

    public void YouKilled(string name)
    {
        Gamemanager.instance.UpdateYouKilledFeedText(name);
    }
}
