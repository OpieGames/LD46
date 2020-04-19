using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] inventory = new int[(int)Pickup.Kind.NumKinds];
    public GameObject Shield;
    void Start()
    {
        Shield.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            Shield.SetActive(true);
        }
        else
        {
            Shield.SetActive(false);
        }


    }
}
