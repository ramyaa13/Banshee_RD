using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float Damage;
    //public bool Attack;
    //private PhotonView photonView;
    //public string KillerName;
    //public GameObject LocalPlayerObj;
    public BoxCollider2D capsuleCollider2D;

    private void Start()
    {
        //photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine)
        //{
        //    KillerName = LocalPlayerObj.GetComponent<BansheePlayer>().PlayerName;
        //}

        EnableTrigger(false);
    }

    public void EnableTrigger(bool state)
    {
        capsuleCollider2D.enabled = state;
        if (state)
            Invoke("DisableIt", 0.8f);

        Debug.Log("enabled");
    }

    private void DisableIt()
    {
        capsuleCollider2D.enabled = false;
    }
    public void UpdateDamage(float damage)
    {
        Damage = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine))
        {
            if (target.tag == "Player")
            {
                print("wertwertwert_____triggered__3");
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, Damage);
                float enemyHealth = target.GetComponent<HealthController>().health;

                Debug.Log("ENEMY HEALTH = " + enemyHealth);

                if (enemyHealth <= 0)
                {
                    Debug.Log("wertwertwert_____triggered_____dead");
                    Gamemanager.instance.enemyDied = false;

                    Gamemanager.instance.UpdateKillCount();
                    Player GotKilled = target.Owner;
                    target.GetComponent<HealthController>().photonView.RPC("YouGotKilledBy", GotKilled, "KillerName");
                    target.GetComponent<HealthController>().photonView.RPC("YouKilled", Gamemanager.instance.LocalPlayer.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }

            }
        }
    }
}
