using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
   

    void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine))
        {
            if (target.tag == "Player")
            {
                target.transform.GetComponent<BansheePlayer>().speedBostPS.SetActive(false);
                target.GetComponent<HealthController>().photonView.RPC("HealthUpdate", RpcTarget.AllBuffered, 1.0f);
                float enemyHealth = target.GetComponent<HealthController>().health;
                Debug.Log(target.Owner + "enemy health name" + enemyHealth);
                if (enemyHealth <= 0)
                {
                    //Gamemanager.instance.UpdateKillCount();
                    Player GotKilled = target.Owner;
                    //target.GetComponent<HealthController>().photonView.RPC("YouGotKilledBy", GotKilled, KillerName);
                    //target.GetComponent<HealthController>().photonView.RPC("YouKilled", LocalPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }
            }


        }

    }
}
