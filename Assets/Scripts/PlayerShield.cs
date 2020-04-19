using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldState
{
    Inactive,
    Blocking,
    Parrying,
}

[RequireComponent(typeof(BoxCollider))]
public class PlayerShield : MonoBehaviour
{
    public ShieldState CurrentState = ShieldState.Inactive;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("shield collided with " + other.transform.name);
        BaseProjectile proj = other.GetComponent<BaseProjectile>();
        if (proj)
        {
            switch (CurrentState)
            {
                case ShieldState.Inactive:
                    return;
                case ShieldState.Blocking:
                    Blocking(proj, other);
                    break;
                case ShieldState.Parrying:
                    if (proj.Parryable)
                    {
                        Parrying(proj, other);
                    }
                    break;
            }
        }
    }

    private void Blocking(BaseProjectile proj, Collider other)
    {
        Debug.Log("Projectile destroyed!");
        Destroy(other.gameObject);
    }

    private void Parrying(BaseProjectile proj, Collider other)
    {
        Tower t = proj.TowerFiredFrom.GetComponent<Tower>();
        GameObject go = other.gameObject;
        Debug.LogFormat("Parry to {0}!", proj.TowerFiredFrom.name);

        go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        Vector3 lookTarget = proj.TowerFiredFrom.transform.position;
        lookTarget.y += 1.15f; // reflect back, but just a bit lower (hopefully avoid some collisions (this is dumb))
        Vector3 relativePos = lookTarget - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        go.transform.rotation = rotation;
        proj.HasBeenParried = true;

        //go.transform.LookAt(proj.TowerFiredFrom.transform);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * t.ProjectileSpeed * 110.0f);
        Destroy(go, 5.0f);
    }
}
