using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControllerLocal : MonoBehaviour
{
    public List<WeaponData> weapons = new List<WeaponData>();

    public int weaponId;
    //public float fireRate = 10f;

    public GameObject[] WO;
    //public Image filledImage;

    //private GameObject WeaponObject;
    //private GameObject bulletPrefab;
    //private Transform bulletSpawnPoint;
    private float nextFireTime;
    private float fireRate;
    //public bool isGunEquipped = false;
    //public bool isSwordEquipped = false;
    //private float BulletDamage = 0f;


    private bool isGunDetect = false;
    private bool isSwordDetect = false;
    private bool isWeaponHeld = false;
    private GameObject DestroyweaponObj;
    private GameObject DestroyObj;
    private string WeaponName;
    private GameObject SpawnweaponObj;

    WeaponData equipedWeaponData;

    internal int animationID;

    // Start is called before the first frame update
    void Start()
    {
        //WO = new GameObject[Gamemanager.instance.groundweapons.Length];

        //for (int i = 0; i < Gamemanager.instance.groundweapons.Length; i++)
        //{
        //    WO[i] = Gamemanager.instance.groundweapons[i];
        //    Debug.Log(WO[i].name + " : WObj");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time <= nextFireTime)
        {
            //print("   2  " + (nextFireTime - Time.time));
            //filledImage.fillAmount = (1 - ((nextFireTime - Time.time) * fireRate));
        }

    }

    public bool IsGunEquiped()
    {
        if (equipedWeaponData != null)
            return equipedWeaponData.isWeaponEquipped;
        return false;
    }

    public bool IsSwordEquiped()
    {
        if (equipedWeaponData != null)
            return equipedWeaponData.isSwordEquipped;
        return false;
    }

    public void WeaponSpawnObj(string name)
    {
        Debug.Log(name + "  wsobj entered");
        foreach (var weapon in WO)
        {
            if (weapon.name == name)
            {
                SpawnweaponObj = weapon;
                Debug.Log(SpawnweaponObj.name + ": spawn weapon object assigned");
            }
            else
               Debug.Log(weapon.name + ": spawn weapon object not assigned");

        }

    }

    //[PunRPC]
    public void DisableWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.Weapon.gameObject.SetActive(false);
            weapon.isWeaponEquipped = false;
            weapon.isSwordEquipped = false;

            //isGunEquipped = false;
            //isSwordEquipped = false;
            //BulletDamage = 0f;
        }
    }

    //[PunRPC]
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


                equipedWeaponData = weapon;
                animationID = weapon.ID;
                // Debug.Log("Weapon equipped: " + weapon.WeaponTag);
            }
        }
    }

    //[PunRPC]
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
                if(weapon.Weapon.gameObject.GetComponent<SwordLocal>() != null)
                    weapon.Weapon.gameObject.GetComponent<SwordLocal>().UpdateDamage(weapon.SwordDamage);
                //weapon.Weapon.GetComponent<Sword>().LocalPlayerObj = this.gameObject;

                // Debug.Log("Weapon equipped: " + weapon.WeaponTag);

                equipedWeaponData = weapon;
                animationID = weapon.ID;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWeaponHeld)
        {
            return;
        }

        print(collision.transform.name);
        print(collision.transform.tag);
        foreach (var weapon in weapons)
        {
            if (collision.gameObject.tag == "Sword")
            {
                if (collision.gameObject.tag == weapon.WeaponTag)
                {
                    weaponId = weapon.ID;
                    print(weaponId);
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
                    print(weaponT.weaponType +"    " + weapon.weaponType);

                    if (weaponT.weaponType == weapon.weaponType)
                    {
                        weaponId = weapon.ID;
                        isGunDetect = true;
                        isSwordDetect = false;
                        DestroyweaponObj = collision.gameObject;
                        WeaponName = weaponT.weaponType.ToString();
                        print(weaponId + " Wepon name " + WeaponName);
                        WeaponSpawnObj(WeaponName);
                    }
                }
            }
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
            if (isGunDetect == true && isSwordDetect == false)
            {
                isWeaponHeld = true;
                //EnableGunWeapon(weaponId);
                Debug.Log( "Set gun with "+weaponId + "  wid");
                EnableGunWeapon(weaponId);
                WeaponEquippedbyPlayer();
                //Gamemanager.instance.SetPlayerState(2);
                //photonView.RPC("WeaponEquippedbyPlayer", RpcTarget.AllBuffered);
            }
            else if (isGunDetect == false && isSwordDetect == true)
            {
                isWeaponHeld = true;
                //EnableSword(weaponId);
                Debug.Log(weaponId + "wid");
                EnableSword(weaponId);
                WeaponEquippedbyPlayer();
                //Gamemanager.instance.SetPlayerState(3);
                //photonView.RPC("WeaponEquippedbyPlayer", RpcTarget.AllBuffered);
            }
            else
            {
                Debug.Log("No Gun Detected");
            }
        }
        else
        {
            isGunDetect = false;
            isSwordDetect = false;
            isWeaponHeld = false;
            DestroyweaponObj = null;
            // DisableWeapons();
            DisableWeapons();
            //Gamemanager.instance.SetPlayerState(1);
            //photonView.RPC("WeaponDroppedbyPlayer", RpcTarget.AllBuffered);
            WeaponDroppedbyPlayer();
        }
    }


    public void WeaponEquippedbyPlayer()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Gamemanager.instance.GRemoveWO(DestroyweaponObj);
        //    Gamemanager.instance.GRemoveOO(DestroyweaponObj);
        //    PhotonNetwork.Destroy(DestroyweaponObj);
        //}
        //else
        //{
        //    int viewId = DestroyweaponObj.GetComponent<PhotonView>().ViewID;
        //    Debug.Log("ViewID of weapon: " + viewId);
        //    photonView.RPC("RPC_ForceMasterClientWeapon", RpcTarget.MasterClient, viewId);
        //}
        Debug.Log("Weapon Destroyed and synced");
    }

    //[PunRPC]
    //void RPC_ForceMasterClientWeapon(int viewID)
    //{
    //    PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);

    //}

    public void WeaponDroppedbyPlayer()
    {
        Vector3 PlayerPos = this.transform.position;
        PlayerPos.x += 1f;
        PlayerPos.y += 1.5f;
        animationID = 0;
        if (SpawnweaponObj != null)
        {
            //GameObject Weapon = (GameObject) Instantiate(SpawnweaponObj.name, PlayerPos, Quaternion.identity);
            //Weapon.transform.SetParent(Gamemanager.instance.ObjectContainer, false);
            //Weapon.SetActive(true);
            //Gamemanager.instance.GAddWO(Weapon);
            //Gamemanager.instance.GAddOO(Weapon);
           
        }
    }

    [PunRPC]
    void RPC_ForceMasterSpawnWeapon(Vector3 _position, string name)
    {
        SpawnEquippedWeapon(_position, name);
    }

    public void SpawnEquippedWeapon(Vector3 position, string SpawnObjectName)
    {

        //GameObject obj = PhotonNetwork.InstantiateRoomObject(SpawnObjectName, position, Quaternion.identity);
        //obj.transform.SetParent(Gamemanager.instance.ObjectContainer, false);
        //obj.SetActive(true);
        //Gamemanager.instance.GAddWO(obj);
        //Gamemanager.instance.GAddOO(obj);
    }

    public Vector2 GetGunDirection(Transform referenceTransform)
    {

        if (equipedWeaponData.isWeaponEquipped)
        {
            Vector3 direction = referenceTransform.position - equipedWeaponData.Weapon.transform.position;
            return direction.normalized;
            // return nearGun.transform.right;
        }
        Vector3 dir = referenceTransform.position - equipedWeaponData.Weapon.transform.position;
        return dir.normalized;
        // return Vector2.zero;

    }

    public void Shoot(bool isFacingRight)
    {
        fireRate = equipedWeaponData.fireRate;
        if (Time.time >= nextFireTime && equipedWeaponData.isWeaponEquipped == true && equipedWeaponData.isSwordEquipped == false)
        {
            if (equipedWeaponData.BulletSpawnPoint != null)
            {
                GameObject bullet = (GameObject) Instantiate(equipedWeaponData.BulletPrefab, equipedWeaponData.BulletSpawnPoint.position,equipedWeaponData.BulletSpawnPoint.rotation);
                var localBullet = bullet.GetComponent<BulletFireLocal>();
                if (!isFacingRight)
                    localBullet.transform.localEulerAngles = new Vector3(0, 0, 0);
                localBullet.UpdateDamage(equipedWeaponData.BulletDamage);
                localBullet.LocalPlayerObj = this.gameObject;
                //if (isFacingRight == true)
                //{
                //    localBullet.ChangeDirection();
                //}
                localBullet.ChangeDirection();

                if (equipedWeaponData.muzzleFlash != null)
                    equipedWeaponData.muzzleFlash.SetActive(true);

                bullet.transform.parent = null;
                //filledImage.fillAmount = 0;

                nextFireTime = Time.time + 1f / fireRate;
                Debug.Log("firieng and rate: " + fireRate + "and :" + (Time.time -  nextFireTime));
            }
        }

        if (equipedWeaponData.isWeaponEquipped == false && equipedWeaponData.isSwordEquipped == true)
        {
            //sword Attack

        }
    }

}
