using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float Damage;
    //public bool Attack;
    private PhotonView photonView;
    public string KillerName;
    public GameObject LocalPlayerObj;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            KillerName = LocalPlayerObj.GetComponent<BansheePlayer>().PlayerName;
        }
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
                float enemyHealth = target.GetComponent<HealthController>().health;
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, Damage);

                if (enemyHealth < 0.1)
                {
                    Gamemanager.instance.UpdateKillCount();
                    Player GotKilled = target.Owner;
                    target.GetComponent<HealthController>().photonView.RPC("YouGotKilledBy", GotKilled, KillerName);
                    target.GetComponent<HealthController>().photonView.RPC("YouKilled", LocalPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }

            }
        }
    }
}
