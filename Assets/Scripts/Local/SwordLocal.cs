using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUPS.AntiCheat.Protected;


public class SwordLocal : MonoBehaviour
{
    public ProtectedFloat Damage;
    //public bool Attack;
    public string KillerName;
    public GameObject LocalPlayerObj;
    public BoxCollider2D capsuleCollider2D;

    private void Start()
    {
        EnableTrigger(false);
    }

    public void EnableTrigger(bool state)
    {
        capsuleCollider2D.enabled = state;
        if (state)
            Invoke("DisableIt", 0.8f);
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
        var target = collision.gameObject;
        print(collision.transform.name);

            if (target.tag == "Player")
            {
                target.GetComponent<HealthControllerLocal>().HealthUpdate(Damage);
                //target.RPC("HealthUpdate", RpcTarget.AllBuffered, Damage);

                //if (enemyHealth < 0.1)
                //{
                //    //Gamemanager.instance.UpdateKillCount();
                //    //Player GotKilled = target.Owner;
                //    //target.GetComponent<HealthController>().photonView.RPC("YouGotKilledBy", GotKilled, KillerName);
                //    //target.GetComponent<HealthController>().photonView.RPC("YouKilled", LocalPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                //}

            }
    }
}
