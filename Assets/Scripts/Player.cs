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
        if (Input.GetButton("Block"))
        {
            Shield.SetActive(true);
            gameObject.GetComponent<CPMMovement>().maxTopSpeed = 3.0f;
        }
        else
        {
            Shield.SetActive(false);
            gameObject.GetComponent<CPMMovement>().ResetMaxTopSpeed();
        }


    }
}
