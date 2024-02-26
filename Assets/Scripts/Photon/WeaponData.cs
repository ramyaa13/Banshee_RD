using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData 
{
    public int ID;
    public string WeaponTag;
    public GameObject Weapon;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public float BulletDamage;
    public float SwordDamage;
    public bool isWeaponEquipped = false;
    public bool isSwordEquipped = false;
    
}
