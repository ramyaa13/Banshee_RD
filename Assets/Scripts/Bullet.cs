// using UnityEngine;

// public class Bullet : MonoBehaviour
// {
//     public float speed;
//     public float lifetime;
//     public int damage;
//     public LayerMask collisionLayer;
//     public Transform referenceTransform;
//     private Rigidbody2D rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         // Vector2 direction = (referenceTransform.position - transform.position).normalized;
//         Vector2 direction =referenceTransform.right;
//         rb.velocity = direction * speed;
//         Destroy(gameObject, lifetime);
//     }

//     void Update()
//     {
//         CheckCollision();
//     }



//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Check if the collision is with an object that has a HealthController
//         HealthController healthController = collision.gameObject.GetComponent<HealthController>();

//         // If the collided object has a HealthController, apply damage
//         if (healthController != null)
//         {
//             healthController.TakeDamage(damage);
//         }

//         // Destroy the bullet
//         // Destroy(gameObject);
//     }
//     void CheckCollision()
//     {
//         RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity.normalized, speed * Time.deltaTime, collisionLayer);

//         if (hit.collider != null)
//         {
//             HandleCollision(hit.collider);
//         }
//     }

//     void HandleCollision(Collider2D other)
//     {
//         // Example: Damage enemies and destroy the bullet
//         if (other.CompareTag("Gem"))
//         {
//             Destroy(other.gameObject);

//             //     EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
//             //     if (enemyHealth != null)
//             //     {
//             //         enemyHealth.TakeDamage(damage);
//             //     }
//         }
//         Destroy(gameObject);
//     }
// }



// //working perfectly,
// using UnityEngine;

// public class Bullet : MonoBehaviour
// {
//     public float speed;
//     public float lifetime;
//     public int damage;
//     public LayerMask collisionLayer;
//     public Transform referenceTransform;
//     private Rigidbody2D rb;


//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();

//         // Vector2 direction = (referenceTransform.position - transform.position).normalized;

//         Vector2 direction = referenceTransform.right;
//         transform.up = direction;
//         rb.velocity = direction * speed;
//         Destroy(gameObject, lifetime);
//     }

//     void Update()
//     {
//         CheckCollision();
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         HealthController healthController = collision.gameObject.GetComponent<HealthController>();

//         if (healthController != null)
//         {
//             healthController.TakeDamage(damage);
//         }

//         Destroy(gameObject);
//     }

//     void CheckCollision()
//     {
//         RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity.normalized, speed * Time.deltaTime, collisionLayer);

//         if (hit.collider != null)
//         {
//             HandleCollision(hit.collider);
//         }
//     }

//     void HandleCollision(Collider2D other)
//     {
//         if (other.gameObject.tag == "Gem")
//         {
//             Destroy(other.gameObject);
//         }
//         Destroy(gameObject);
//     }
// }




using System.Collections;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviourPun
{
    public float speed;
    public float lifetime =1f;
    public float damage = 20f;
    public LayerMask collisionLayer;
    // public Transform referenceTransform;
    private Rigidbody2D rb;
    private WeaponController gunController; // Reference to GunEquipController script
    private PhotonView photonView;

   

    void Start()
    {
        photonView = GetComponent<PhotonView>();
       
        if(photonView.IsMine)
        {
            rb = GetComponent<Rigidbody2D>();
            gunController = FindObjectOfType<WeaponController>(); // Find GunEquipController in the scene
            BulletFire();
        }

        
    }

    private void Update()
    {
        
    }

    public void BulletFire()
    {
        if (gunController != null && gunController.isGunEquipped == true)
        {
           Vector2 direction = gunController.GetGunDirection(transform);


            transform.up = direction;
            rb.velocity = direction * speed;
            StartCoroutine(DestroyBullet());
            //Destroy(gameObject, lifetime);
        }
        else
        {
            // Gun is not equipped, destroy the bullet
            Destroy(gameObject);
        }
    }


    public void BulletFires()
    {
        //if (gunController != null && gunController.isGunEquipped == true)
        //{
        //    Vector2 direction = gunController.GetGunDirection(transform);


        //    transform.up = direction;
        //    rb.velocity = direction * speed;
        //    StartCoroutine(DestroyBullet());
        //    //Destroy(gameObject, lifetime);
        //}
        //else
        //{
        //    // Gun is not equipped, destroy the bullet
        //    Destroy(gameObject);
        //}
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(lifetime);
        photonView.RPC("DestroyB", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void DestroyB()
    {
        Destroy(this.gameObject);
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //if(!photonView.IsMine)
        //{
        //    return;
        //}

        PhotonView enemyTarget = collision.gameObject.GetComponent<PhotonView>();
        HealthController healthController = collision.gameObject.GetComponent<HealthController>();
        if (enemyTarget != null && (!enemyTarget.IsMine || enemyTarget.IsRoomView))
        {
            if(enemyTarget.tag == "Player")
            {
                if (healthController != null)
                {
                    //healthController.TakeDamage(damage, enemyTarget);
                }
            }
            //photonView.RPC("DestroyB", RpcTarget.AllBuffered);
        }
    }


}
