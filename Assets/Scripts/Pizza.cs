using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pizza : MonoBehaviour
{
    public int CurrentHealth = 6;
    public int MaxHealth = 6;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("pizza hit by: " + other.gameObject.name);
        BaseProjectile proj = other.gameObject.GetComponent<BaseProjectile>();
        if (proj)
        {
            //TODO: pizza gets hit logic
            Destroy(other.gameObject);
        }
    }

}
