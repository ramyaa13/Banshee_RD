using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData 
{
    public int ID;
    public WeaponType weaponType;
    public string WeaponTag;
    public GameObject Weapon;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public float BulletDamage;
    public float SwordDamage;
    public bool isWeaponEquipped = false;
    public bool isSwordEquipped = false;
    public GameObject muzzleFlash;
    public float fireRate = 10;
}

public enum WeaponType {Pistol, Shotgun, Rifle, Sniper, Flamer, Sword}
