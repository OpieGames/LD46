using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedGun : MonoBehaviour
{
    public GunData CurrentGun;

    private GameObject SpawnedWeaponMesh;
    private WeaponType CurrentFiringMode; // Only used for ToggleRifle
    // Start is called before the first frame update
    void Start()
    {
        Equip();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
