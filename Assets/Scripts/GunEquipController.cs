// using UnityEngine;

// public class GunEquipController : MonoBehaviour
// {
//     public GameObject gunEquip;
//     public GameObject gun;
//     public KeyCode equipKey = KeyCode.E;

//     private bool isGunEquipped = false;

//     private void Update()
//     {
//         // Check if the character is near the gun and press the equip key   
//         if (Input.GetKeyDown(equipKey) && IsCharacterNearGun())
//         {
//             Debug.Log("KEY PRESSED E");
//             ToggleGunEquip();

//         }
//     }

//     private bool IsCharacterNearGun()
//     {
//         float distance = Vector3.Distance(transform.position, gun.transform.position);
//         return distance < 2f; // Adjust the distance as needed.
//     }

//     private void ToggleGunEquip()
//     {
//         // Toggle between equipping and unequipping the gun to the GunEquip GameObject
//         if (isGunEquipped)
//         {
//             // Unequip the gun
//             gun.transform.parent = null;
//             gun.SetActive(false);
//             Rigidbody2D gunRigidbody2D = gun.GetComponent<Rigidbody2D>();

//             if (gunRigidbody2D != null)
//             {
//                 gunRigidbody2D.isKinematic = true;
//             }

//             // Print log when the gun is unequipped
//             Debug.Log("Gun Unequipped");
//         }


//            else
//             {

//                 gun.transform.parent = gunEquip.transform;
//                 gun.transform.localPosition = Vector3.zero;
//                 gun.SetActive(true);

//                 // Disable the Rigidbody2D when the gun is equipped
//                 Rigidbody2D gunRigidbody2D = gun.GetComponent<Rigidbody2D>();
//                 if (gunRigidbody2D != null)
//                 {
//                     gunRigidbody2D.simulated = false;
//                 }

//                 // Print log when the gun is equipped
//                 Debug.Log("Gun Equipped", gun);
//             }

//         isGunEquipped = !isGunEquipped;
//     }
// }





// perfect working script 
// using UnityEngine;

// public class GunEquipController : MonoBehaviour
// {
//     public GameObject gunEquip;
//     public KeyCode equipKey = KeyCode.E;

//     private bool isGunEquipped = false;
//     private GameObject nearGun;

//     private void Update()
//     {
//         // Check if the character is near the gun and press the equip key   
//         if (Input.GetKeyDown(equipKey) && IsCharacterNearGun())
//         {
//             Debug.Log("KEY PRESSED E");
//             ToggleGunEquip();
//         }
//     }

//     private bool IsCharacterNearGun()
//     {
//         // Use Physics2D.OverlapCircle to check for colliders in a circular area
//         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);

//         foreach (Collider2D collider in colliders)
//         {
//             if (collider.gameObject.tag == "Gun")
//             {
//                 // Store the reference to the near gun
//                 nearGun = collider.gameObject;
//                 return true;
//             }
//         }

//         return false;
//     }

//     private void ToggleGunEquip()
//     {
//         if (isGunEquipped)
//         {
//             // Unequip the gun
//             nearGun.transform.parent = null;
//             nearGun.SetActive(false);
//             Rigidbody2D gunRigidbody2D = nearGun.GetComponent<Rigidbody2D>();

//             if (gunRigidbody2D != null)
//             {
//                 gunRigidbody2D.isKinematic = true;
//             }

//             // Print log when the gun is unequipped
//             Debug.Log("Gun Unequipped");
//         }
//         else
//         {
//             // Equip the gun
//             nearGun.transform.parent = gunEquip.transform;
//             nearGun.transform.localPosition = Vector3.zero;
//             nearGun.SetActive(true);

//             // Disable the Rigidbody2D when the gun is equipped
//             Rigidbody2D gunRigidbody2D = nearGun.GetComponent<Rigidbody2D>();
//             if (gunRigidbody2D != null)
//             {
//                 gunRigidbody2D.simulated = false;
//             }

//             // Print log when the gun is equipped
//             Debug.Log("Gun Equipped", nearGun);
//         }

//         isGunEquipped = !isGunEquipped;
//     }
// }





using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GunEquipController : MonoBehaviour
{
    public GameObject gunEquip;
    public KeyCode equipKey = KeyCode.E;
    public KeyCode unequipKey = KeyCode.R;

    public bool isGunEquipped = false;
    private GameObject nearGun;
    private Vector3 originalGunPosition;
    private bool originalGunState;

    public GunAttachBullet gunAttachBullet;

    public bool isBeingHeld = false;
    private PhotonView photonView;
    public bool IsCharNearGun;
    public Vector2 GetGDirection;
    private Transform refTransform;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
       

    }

    public void ToggleGunEquip()
    {
        photonView.RPC("ToggleGunEquiped", RpcTarget.AllBuffered);
    }

    public void Shoot()
    {
        photonView.RPC("GunShoot", RpcTarget.AllBuffered);
    }

    public bool IsCharacterNearGun()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Gun")
            {
                nearGun = collider.gameObject;

                // Store the original position and state when the gun is first equipped
                // originalGunPosition = nearGun.transform.position;
                Rigidbody2D gunRigidbody2D = nearGun.GetComponent<Rigidbody2D>();
                originalGunState = (gunRigidbody2D != null) ? gunRigidbody2D.simulated : true;
                photonView.RPC("IsCharacterNearGuns", RpcTarget.AllBuffered, true);
                return true;
            }
        }

        photonView.RPC("IsCharacterNearGuns", RpcTarget.AllBuffered, false);
        return false;
    }

    public void UnequipGun()
    {

        photonView.RPC("UnequipedGun", RpcTarget.AllBuffered);
    }

    public Vector2 GetGunDirection(Transform referenceTransform)
    {
        if (isGunEquipped && nearGun != null)
        {
            Vector3 direction = referenceTransform.position - nearGun.transform.position;
            return direction.normalized;
        }
        return Vector2.zero;
    }

    [PunRPC]
    public void IsCharacterNearGuns(bool x)
    {
        IsCharNearGun = x;
    }

    [PunRPC]
    public void ToggleGunEquiped()
    {
        if (isGunEquipped)
        {
            UnequipGun();
        }
        else
        {
            // Equip the gun
            nearGun.transform.parent = gunEquip.transform;
            nearGun.transform.localPosition = Vector3.zero;
            nearGun.SetActive(true);

            gunAttachBullet = nearGun.GetComponent<GunAttachBullet>();
            gunAttachBullet.GunIsHeld();
           // gunAttachBullet.TransferOwnership();

            // Disable the Rigidbody2D when the gun is equipped
            Rigidbody2D gunRigidbody2D = nearGun.GetComponent<Rigidbody2D>();
            if (gunRigidbody2D != null)
            {
                gunRigidbody2D.simulated = false;
            }

            // Print log when the gun is equipped
            Debug.Log("Gun Equipped", nearGun);
        }

        isGunEquipped = !isGunEquipped;
    }

    [PunRPC]
    public void GunShoot()
    {
        if(gunAttachBullet != null)
        {
            gunAttachBullet.GunShoot();
        }
        else
        {
            Debug.Log("Gun is not equipped and can't shoot");
        }
    }


    [PunRPC]
    public void UnequipedGun()
    {

        gunAttachBullet.GunIsNotHeld();
        gunAttachBullet = null;
        // Unequip the gun and return it to its original state
        nearGun.transform.parent = null;
        // nearGun.transform.position = originalGunPosition;
        nearGun.SetActive(true);

        Rigidbody2D gunRigidbody2D = nearGun.GetComponent<Rigidbody2D>();
        if (gunRigidbody2D != null)
        {
            gunRigidbody2D.simulated = originalGunState;
        }

        // Print log when the gun is unequipped
        Debug.Log("Gun Unequipped");
        isGunEquipped = false;
    }




    // New method to check if the gun is equipped
    //public bool IsGunEquipped()
    //{
        
    //    return isGunEquipped;
    //}


}
