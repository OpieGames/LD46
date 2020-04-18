using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    public int CurrentHealth = 6;
    public int MaxHealth = 6;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pizza hit by: " + other.name);
    }

}
