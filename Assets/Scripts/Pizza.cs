using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pizza hit by: " + other.name);
    }
}
