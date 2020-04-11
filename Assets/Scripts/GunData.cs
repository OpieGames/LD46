using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Shotgun,
    FullAutoRifle,
    SemiAutoRifle,
    BurstRifle,
    ToggleRifle,
    Sniper,
    RocketLauncher,
    SMG
}

[CreateAssetMenu(fileName = "GunData", menuName = "Data/Gun", order = 1)]
public class GunData : ScriptableObject
{
    public GameObject Mesh;
    public WeaponType Type = WeaponType.FullAutoRifle;
    public bool IsProjectile = false;
    // public bool ThrowAwayRemainingAmmoInClipOnReload = false;
    public float Damage = 10f;
    public float FireRate = 240f; // in RoundsPerMinute
    public float ReloadTime = 1.5f; // in Seconds
    public int MagazineSize = 30;
    public int ReserveAmmoMax = 90;
    public int ReserveAmmoInit = 90;
}
