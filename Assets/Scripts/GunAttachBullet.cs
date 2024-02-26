// using UnityEngine;

// public class GunAttachBullet : MonoBehaviour
// {
//     public Transform bulletSpawnPoint;
//     public GameObject bulletPrefab;
//     public float fireRate;

//     private float nextFireTime;

//     void Start()
//     {
//         nextFireTime = 0f;
//     }

//     void Update()
//     {

//         if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
//         {
//             GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
//             bullet.transform.parent = null;
//             nextFireTime = Time.time + 1f / fireRate;
//         }
//     }
// }



using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GunAttachBullet : MonoBehaviour, IPunOwnershipCallbacks
{
    public GameObject gunPrefab; // Reference to the gun prefab
    public float fireRate;
    public GameObject bulletPrefab;

    private Transform bulletSpawnPoint;
    private float nextFireTime;
    private GunEquipController gunEquipController;
    public bool isGunTaken;

    private PhotonView photonView;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        isGunTaken = false;
        nextFireTime = 0f;
        gunEquipController = FindObjectOfType<GunEquipController>();

        // Find the GunBarrel GameObject in the gunPrefab
        Transform gunBarrel = gunPrefab.transform.Find("GunBarrel");

        if (gunBarrel != null)
        {
            // Assign the GunBarrel's transform to the bulletSpawnPoint
            bulletSpawnPoint = gunBarrel;
        }
        else
        {
            Debug.LogError("GunBarrel not found in the gunPrefab!");
        }

    }

    void Update()
    {
        if(isGunTaken)
        {
            
            rb.simulated = false;
        }
        else
        {
            rb.simulated = true;
        }
    }
    public void GunIsHeld()
    {
        photonView.RPC("GunHeld", RpcTarget.AllBuffered);

    }

    public void GunIsNotHeld()
    {
        photonView.RPC("GunNotHeld", RpcTarget.AllBuffered);
    }

    public void GunShoot()
    {
        photonView.RPC("GunShootRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void GunShootRPC()
    {
        if (Time.time >= nextFireTime && isGunTaken)
        {

            if (bulletSpawnPoint != null)
            {
                GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.transform.parent = null;
                nextFireTime = Time.time + 1f / fireRate;
            }
            
            else
            {
                Debug.Log("bulletSpawnPoint not assigned. Make sure GunBarrel is present in the gunPrefab.");
            }
        }
    }

    public void TransferOwnership()
    {
        if (photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We do not request ownership, already mine");
        }
        else
        {
            photonView.RequestOwnership();
        }
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if(targetView != photonView)
        {
            return;
        }
        Debug.Log("ownership requested for: "+ targetView.name + " from " + requestingPlayer.NickName);
        photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("ownership transfered for: " + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }


    [PunRPC]
    public void GunHeld()
    {
        isGunTaken = true;
    }

    [PunRPC]
    public void GunNotHeld()
    {
        isGunTaken = false;
    }
}
