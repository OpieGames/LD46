using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerShield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("shield collided with " + other.transform.name);
        BaseProjectile proj = other.GetComponent<BaseProjectile>();
        if (proj)
        {
            Debug.Log("Projectile destroyed!");
            Destroy(other.gameObject);
        }
    }
}
