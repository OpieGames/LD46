using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] inventory = new int[(int)Pickup.Kind.NumKinds];
    public GameObject Shield;
    private bool shieldActive = false;
    // Start is called before the first frame update
    void Start()
    {
        Shield.SetActive(false);
    }

    // Update is called once per frame
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
