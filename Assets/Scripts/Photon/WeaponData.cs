using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUPS.AntiCheat.Protected;

[System.Serializable]
public class WeaponData 
{
    public int ID;
    public WeaponType weaponType;
    public string WeaponTag;
    public GameObject Weapon;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public ProtectedFloat BulletDamage;
    public ProtectedFloat SwordDamage;
    public bool isWeaponEquipped = false;
    public bool isSwordEquipped = false;
    public GameObject muzzleFlash;
    public ProtectedFloat fireRate = 10;
}

public enum WeaponType {Pistol, Shotgun, Rifle, Sniper, Flamer, Sword}
