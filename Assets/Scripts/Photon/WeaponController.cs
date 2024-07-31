using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public List<WeaponData> weapons = new List<WeaponData>();

    public int weaponId;
    //public float fireRate = 10f;

    public GameObject[] WO;
    public Image filledImage;


    //private GameObject WeaponObject;
    //private GameObject bulletPrefab;
    //private Transform bulletSpawnPoint;
    private float nextFireTime;
    private float fireRate;

    //public bool isGunEquipped = false;
    //public bool isSwordEquipped = false;

    private PhotonView photonView;
    private float BulletDamage = 0f;


    private bool isGunDetect = false;
    private bool isSwordDetect = false;
    private bool isWeaponHeld = false;
    private GameObject DestroyweaponObj;
    private GameObject DestroyObj;
    private string WeaponName;
    private GameObject SpawnweaponObj;

    private WeaponData EquipedWeapon;
    internal int animationID;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        photonView.RPC("DisableWeapons", RpcTarget.AllBuffered);
        if (photonView.IsMine)
        {
            //if (weapons.Count != 0)
            //{
            //    foreach (var weapon in weapons)
            //    {
            //        weapon.Weapon.gameObject.SetActive(false);
            //    }
            //}

            WO = new GameObject[Gamemanager.instance.groundweapons.Length];

            for (int i = 0; i < Gamemanager.instance.groundweapons.Length; i++)
            {
                WO[i] = Gamemanager.instance.groundweapons[i];
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time <= nextFireTime)
        {
            //print("   2  " + (nextFireTime - Time.time));
            filledImage.fillAmount = (1 - ((nextFireTime - Time.time) * fireRate));
        }
    }

    public bool IsGunEquiped()
    {
        if (EquipedWeapon != null)
            return EquipedWeapon.isWeaponEquipped;
        return false;
    }

    public bool IsSwordEquiped()
    {
        if (EquipedWeapon != null)
            return EquipedWeapon.isSwordEquipped;
        return false;
    }

    public void WeaponSpawnObj(string name)
    {

        foreach (var weapon in WO)
        {
            if (weapon.name == name)
            {
                SpawnweaponObj = weapon;
                Gamemanager.instance.ShowMessage(SpawnweaponObj.name);
            }
        }

    }

    [PunRPC]
    public void DisableWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.Weapon.gameObject.SetActive(false);
            weapon.isWeaponEquipped = false;
            weapon.isSwordEquipped = false;

            //isGunEquipped = false;
            //isSwordEquipped = false;

            BulletDamage = 0f;
            EquipedWeapon = null;
            animationID = 0;

            //playerMovementController.ChangeAnimation(gunEquipController.animationID);
        }
    }

    [PunRPC]
    public void EnableGunWeapon(int id)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.ID == id)
            {
                //photonView.RPC("DisableWeapons", RpcTarget.AllBuffered);
                weapon.Weapon.gameObject.SetActive(true);
                weapon.isWeaponEquipped = true;
                weapon.isSwordEquipped = false;

                //WeaponObject = weapon.Weapon;

                //isGunEquipped = weapon.isWeaponEquipped;
                //isSwordEquipped = weapon.isSwordEquipped;

                //bulletPrefab = weapon.BulletPrefab;
                //bulletSpawnPoint = weapon.BulletSpawnPoint;
                BulletDamage = weapon.BulletDamage;

                EquipedWeapon = weapon;
                animationID = weapon.ID;
                Gamemanager.instance.ShowMessage(animationID.ToString());
                // Debug.Log("Weapon equipped: " + weapon.WeaponTag);
            }
        }
    }

    [PunRPC]
    public void EnableSword(int id)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.ID == id)
            {
                //photonView.RPC("DisableWeapons", RpcTarget.AllBuffered);
                weapon.Weapon.gameObject.SetActive(true);
                weapon.isWeaponEquipped = false;
                weapon.isSwordEquipped = true;

                //isGunEquipped = weapon.isWeaponEquipped;
                //isSwordEquipped = weapon.isSwordEquipped;

                weapon.Weapon.gameObject.GetComponent<Sword>().UpdateDamage(weapon.SwordDamage);
                //weapon.Weapon.GetComponent<Sword>().LocalPlayerObj = this.gameObject;


                EquipedWeapon = weapon;
                animationID = weapon.ID;

                // Debug.Log("Weapon equipped: " + weapon.WeaponTag);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWeaponHeld)
        {
            return;
        }

        foreach (var weapon in weapons)
        {
            if (collision.gameObject.tag == "Sword")
            {
                if (collision.gameObject.tag == weapon.WeaponTag)
                {
                    weaponId = weapon.ID;
                    isSwordDetect = true;
                    isGunDetect = false;
                    DestroyweaponObj = collision.gameObject;
                    WeaponName = collision.gameObject.tag;
                    WeaponSpawnObj(collision.gameObject.tag);
                }
            }
            else if (collision.gameObject.tag == "Weapon")
            {
                var weaponT = collision.GetComponent<Weapon>();
                if (weaponT != null)
                {
                    if (weaponT.weaponType == weapon.weaponType)
                    {
                        weaponId = weapon.ID;
                        isGunDetect = true;
                        isSwordDetect = false;
                        DestroyweaponObj = collision.gameObject;
                        WeaponName = weaponT.weaponType.ToString();
                        WeaponSpawnObj(WeaponName);
                    }
                }
            }
            /*
            if (collision.gameObject.tag == weapon.WeaponTag)
            {
                if (weapon.WeaponTag == "Sword")
                {
                    Debug.Log(collision.gameObject.tag + weapon.WeaponTag + "collided");
                    weaponId = weapon.ID;
                    isSwordDetect = true;
                    isGunDetect = false;
                    DestroyweaponObj = collision.gameObject;
                    WeaponName = collision.gameObject.tag;
                    WeaponSpawnObj(collision.gameObject.tag);
                }
                else
                {
                    Debug.Log(collision.gameObject.tag + weapon.WeaponTag + "collided");
                    weaponId = weapon.ID;
                    isGunDetect = true;
                    isSwordDetect = false;
                    DestroyweaponObj = collision.gameObject;
                    WeaponName = collision.gameObject.tag;
                    WeaponSpawnObj(collision.gameObject.tag);
                }
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (var weapon in weapons)
        {
            if (collision.gameObject.tag == weapon.WeaponTag)
            {
                isGunDetect = false;
                isSwordDetect = false;
                DestroyweaponObj = null;
                weaponId = 0;
                DestroyweaponObj = null;
            }
        }
    }

    public void EquipWeapon()
    {
        if (isWeaponHeld == false)
        {
            Debug.Log("No Weapon Detected");
            if (isGunDetect == true && isSwordDetect == false)
            {
                isWeaponHeld = true;
                //EnableGunWeapon(weaponId);
                Debug.Log(weaponId + "wid");
                photonView.RPC("EnableGunWeapon", RpcTarget.AllBuffered, weaponId);
                WeaponEquippedbyPlayer();
                Gamemanager.instance.SetPlayerState(2);
                //photonView.RPC("WeaponEquippedbyPlayer", RpcTarget.AllBuffered);
            }
            else if (isGunDetect == false && isSwordDetect == true)
            {
                isWeaponHeld = true;
                //EnableSword(weaponId);
                Debug.Log(weaponId + "wid");
                photonView.RPC("EnableSword", RpcTarget.AllBuffered, weaponId);
                WeaponEquippedbyPlayer();
                Gamemanager.instance.SetPlayerState(3);
                //photonView.RPC("WeaponEquippedbyPlayer", RpcTarget.AllBuffered);
            }
            else
            {
                Debug.Log("No Gun Detected");
            }
        }
        else
        {

            print("Drop Gun by me ");
            isGunDetect = false;
            isSwordDetect = false;
            isWeaponHeld = false;
            DestroyweaponObj = null;
            // DisableWeapons();
            photonView.RPC("DisableWeapons", RpcTarget.AllBuffered);
            Gamemanager.instance.SetPlayerState(1);
            //photonView.RPC("WeaponDroppedbyPlayer", RpcTarget.AllBuffered);
            WeaponDroppedbyPlayer();
        }
    }


    public void WeaponEquippedbyPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Gamemanager.instance.GRemoveWO(DestroyweaponObj);
            Gamemanager.instance.GRemoveOO(DestroyweaponObj);
            PhotonNetwork.Destroy(DestroyweaponObj);
        }
        else
        {
            int viewId = DestroyweaponObj.GetComponent<PhotonView>().ViewID;
            Debug.Log("ViewID of weapon: " + viewId);
            photonView.RPC("RPC_ForceMasterClientWeapon", RpcTarget.MasterClient, viewId);
        }
        Debug.Log("Weapon Destroyed and synced");
    }

    [PunRPC]
    void RPC_ForceMasterClientWeapon(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);

    }

    public void WeaponDroppedbyPlayer()
    {
        Vector3 PlayerPos = this.transform.position;
        PlayerPos.x += 1f;
        PlayerPos.y += 1.5f;
        if (SpawnweaponObj != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Gamemanager.instance.ShowMessage(SpawnweaponObj.name);
                GameObject Weapon = PhotonNetwork.Instantiate(SpawnweaponObj.name, PlayerPos, Quaternion.identity);
                Weapon.transform.SetParent(Gamemanager.instance.ObjectContainer, false);
                Weapon.SetActive(true);
                Gamemanager.instance.GAddWO(Weapon);
                Gamemanager.instance.GAddOO(Weapon);
            }
            else
            {
                Debug.Log("client dropped weqapon");
                photonView.RPC("RPC_ForceMasterSpawnWeapon", RpcTarget.MasterClient, PlayerPos, SpawnweaponObj.name);
            }
        }
    }

    [PunRPC]
    void RPC_ForceMasterSpawnWeapon(Vector3 _position, string name)
    {
        SpawnEquippedWeapon(_position, name);
    }

    public void SpawnEquippedWeapon(Vector3 position, string SpawnObjectName)
    {
        Gamemanager.instance.ShowMessage(SpawnObjectName);
        GameObject obj = PhotonNetwork.InstantiateRoomObject(SpawnObjectName, position, Quaternion.identity);
        obj.transform.SetParent(Gamemanager.instance.ObjectContainer, false);
        obj.SetActive(true);
        Gamemanager.instance.GAddWO(obj);
        Gamemanager.instance.GAddOO(obj);
    }

    public Vector2 GetGunDirection(Transform referenceTransform)
    {

        if (photonView.IsMine && IsGunEquiped())
        {
            Vector3 direction = referenceTransform.position - EquipedWeapon.Weapon.transform.position;
            return direction.normalized;
            // return nearGun.transform.right;
        }
        Vector3 dir = referenceTransform.position - EquipedWeapon.Weapon.transform.position;
        return dir.normalized;
        // return Vector2.zero;

    }

    public void Shoot(bool isFacingRight)
    {
        fireRate = 10f;
        if (EquipedWeapon != null)
        {
            fireRate = EquipedWeapon.fireRate;
            if (Time.time >= nextFireTime && IsGunEquiped() == true && IsSwordEquiped() == false)
            {
                if (EquipedWeapon.BulletSpawnPoint != null)
                {
                    GameObject bullet = PhotonNetwork.Instantiate(EquipedWeapon.BulletPrefab.name, EquipedWeapon.BulletSpawnPoint.position, EquipedWeapon.BulletSpawnPoint.rotation);
                    var bulletScript = bullet.GetComponent<BulletFire>();

                    bulletScript.UpdateDamage(BulletDamage);
                    bulletScript.LocalPlayerObj = this.gameObject;
                    //if (!isFacingRight)

                    if (isFacingRight == true)
                    {
                        bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
                    }
                    else
                        bullet.GetComponent<PhotonView>().RPC("ChangeBulletFacing", RpcTarget.AllBuffered);

                    if (EquipedWeapon.muzzleFlash)
                    {
                        photonView.RPC("EnableGunMuzzleFlash", RpcTarget.AllBuffered);
                    }

                    bullet.transform.parent = null;

                    filledImage.fillAmount = 0;
                    nextFireTime = Time.time + 1f / fireRate;
                    //Debug.Log("firieng and rate: " + fireRate + "and :" + nextFireTime);
                }
            }

            if (Time.time >= nextFireTime && IsSwordEquiped() == true && IsGunEquiped() == false)
            {
                //sword Attack
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    [PunRPC]
    public void EnableGunMuzzleFlash()
    {
        EquipedWeapon.muzzleFlash.SetActive(true);
    }
}
