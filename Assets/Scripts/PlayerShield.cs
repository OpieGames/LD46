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
            if (proj.Parryable)
            {
                Tower t = proj.TowerFiredFrom.GetComponent<Tower>();
                GameObject go = other.gameObject;
                Debug.LogFormat("Parry to {0}!", proj.TowerFiredFrom.name);

                go.GetComponent<Rigidbody>().velocity = Vector3.zero;
                go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                Vector3 lookTarget = proj.TowerFiredFrom.transform.position;
                lookTarget.y += 1.25f;
                Vector3 relativePos = lookTarget - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                go.transform.rotation = rotation;

                //go.transform.LookAt(proj.TowerFiredFrom.transform);
                go.GetComponent<Rigidbody>().AddForce(go.transform.forward * t.ProjectileSpeed * 100.0f);
                Destroy(go, 5.0f);
            }
            else
            {
                Debug.Log("Projectile destroyed!");
                Destroy(other.gameObject);
            }
        }
    }
}
