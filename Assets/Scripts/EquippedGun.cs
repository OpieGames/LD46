using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedGun : MonoBehaviour
{
    public GunData CurrentGun;
    public Transform CameraTransform;

    private GameObject SpawnedWeaponMesh;
    private WeaponType CurrentFiringMode; // Only used for ToggleRifle

    public float CurrentAmmo;
    public float CurrentShotCooldown;

    private float ShotCooldown;
    // Start is called before the first frame update
    void Start()
    {
        Equip();
        CurrentAmmo = CurrentGun.MagazineSize;
        ShotCooldown = 1f / (CurrentGun.FireRate / 60.0f);
        Debug.LogWarning(ShotCooldown);
        if (!CameraTransform)
        {
            Debug.LogError("Set the CameraTransform for the EquippedWeapon!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if (CurrentShotCooldown > 0f)
        {
            CurrentShotCooldown -= dt;
        } else if (-CurrentShotCooldown < dt)
        {
            CurrentShotCooldown = 0.0f;
        }
        if (Input.GetButton("Fire1") && CurrentShotCooldown < Mathf.Epsilon)
        {
            Fire();
            if (CurrentShotCooldown < 0f)
            {
                CurrentShotCooldown = ShotCooldown - (-CurrentShotCooldown);
            } else {
                CurrentShotCooldown = ShotCooldown;
            }
            Debug.Log(CurrentShotCooldown);
            Debug.DrawLine(CameraTransform.position, CameraTransform.position + CameraTransform.forward*10f, Color.red, 0.3f);
        }
    }

    void Equip()
    {
        SpawnedWeaponMesh = Instantiate(CurrentGun.Mesh, transform.GetChild(0).transform, false);
        SetLayerRecursively(SpawnedWeaponMesh, LayerMask.NameToLayer("FPS_Models"));
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }
       
        obj.layer = newLayer;
       
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void Fire()
    {
        if (CurrentGun.IsProjectile)
        {

        } else {
            RaycastHit HitInfo;
            if ( Physics.Raycast(CameraTransform.position, CameraTransform.forward, out HitInfo, CurrentGun.Range) )
            {
                Damageable dmgable = HitInfo.transform.gameObject.GetComponent<Damageable>();
                if (dmgable)
                {
                    dmgable.TakeDamage(CurrentGun.Damage);
                }
            }
            
        }
        // Debug.LogError("FIRE! " + CurrentShotCooldown + " / " + ShotCooldown);
    }
}
