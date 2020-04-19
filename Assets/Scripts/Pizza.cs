using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pizza : MonoBehaviour
{
    public int CurrentHealth = 6;
    public int MaxHealth = 6;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pizza hit by: " + other.name);
        BaseProjectile proj = other.GetComponent<BaseProjectile>();
        if (proj)
        {
            //TODO: pizza gets hit logic
            Destroy(other);
        }
    }

}
