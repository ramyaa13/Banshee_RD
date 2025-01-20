using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GUPS.AntiCheat.Protected;


public class BulletFire : MonoBehaviour
{
    public bool MovingDirection = false;
    [SerializeField] ProtectedFloat MoveSpeed = 30f;//30
    public float DestroyTime = 2f;//2
    private PhotonView photonView;

    [SerializeField] ProtectedFloat BulleteDamage;//1
    private int direction;

    public string KillerName;
    public Vector2 leftScale;

    [HideInInspector]
    public GameObject LocalPlayerObj;


    public void UpdateDamage(float damage)
    {
        BulleteDamage = damage;
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (LocalPlayerObj != null && LocalPlayerObj.GetComponent<PhotonView>().IsMine == true)
        {
            KillerName = LocalPlayerObj.GetComponent<BansheePlayer>().PlayerName;
        }
        else
        {
            LocalPlayerObj = null;
        }
        //MoveSpeed = 100f;
        StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        if(!MovingDirection)
        {
            transform.Translate(Vector2.right *  MoveSpeed * Time.deltaTime);
            direction = 1;
        }
        else
        {
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
            direction = -1;
        }

       
    }
    public int GetDirection()
    {
        return direction;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(DestroyTime);
        photonView.RPC("DestroyB", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ChangeDirection()
    {
        MovingDirection = true;
    }

    [PunRPC]
    void ChangeBulletFacing()
    {
        this.transform.localScale = leftScale;
    }

    [PunRPC]
    void DestroyB()
    {
        if (photonView.IsMine)
        {
            Gamemanager.instance.PlayBlastEffect(transform.position);
            StopCoroutine("DestroyBullet");
        }
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.IsMine) && LocalPlayerObj !=  null)
        {
            if (target.tag == "Player")
            {
                float bd = BulleteDamage;
                target.GetComponent<HealthController>().photonView.RPC("HealthUpdate", RpcTarget.AllBuffered, bd/*BulleteDamage*/);
                float enemyHealth = target.GetComponent<HealthController>().health;
                Debug.Log(target.Owner + "enemy health name" + enemyHealth);
                Debug.Log(LocalPlayerObj.GetComponent<PhotonView>().Owner + "my player health name");
                if (enemyHealth <= 0)
                {
                    Gamemanager.instance.enemyDied = false;
                    Gamemanager.instance.UpdateKillCount();
                    Player GotKilled = target.Owner;
                    target.GetComponent<HealthController>().photonView.RPC("YouGotKilledBy", GotKilled, KillerName);
                    target.GetComponent<HealthController>().photonView.RPC("YouKilled", LocalPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }
            }
            else if(target.tag == "Ground")
            {
                Debug.Log("Destroy bullet");
            }

            this.GetComponent<PhotonView>().RPC("DestroyB", RpcTarget.AllBuffered);

        }

        if (collision.tag == "Ground")
        {
            Debug.Log("Destroy bullet");
            this.GetComponent<PhotonView>().RPC("DestroyB", RpcTarget.AllBuffered);
        }
    }
}
