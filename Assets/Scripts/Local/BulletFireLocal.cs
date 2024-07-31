using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFireLocal : MonoBehaviour
{
    public bool MovingDirection = false;
    public float MoveSpeed = 100f;
    public float DestroyTime = 2f;

    public float BulleteDamage;
    private int direction;

    public string KillerName;
    public GameObject blast;

    [HideInInspector]
    public GameObject LocalPlayerObj;


    public void UpdateDamage(float damage)
    {
        BulleteDamage = damage;
    }
    void Start()
    {

        
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

    public void ChangeDirection()
    {
        MovingDirection = true;
    }

    public int GetDirection()
    {
        return direction;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Col " + collision.transform.name);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        

        Debug.Log("Col " + collision.transform.name);

     

        if (collision.tag == "Ground")
        {
            Debug.Log("Destroy bullet");
            Instantiate(blast, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            //this.GetComponent<PhotonView>().RPC("DestroyB", RpcTarget.AllBuffered);
        }
    }
}
