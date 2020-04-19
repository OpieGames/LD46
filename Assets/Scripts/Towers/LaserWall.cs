using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWall : MonoBehaviour
{
    public MeshRenderer LaserMesh;
    public AudioClip DisableSound;

    private bool Enabled = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pizza")
        {
            Debug.Log("Pizza got hit by lazer wall");

            other.gameObject.GetComponent<Pizza>().TakeDamage();
        }
    }

    public void Disable()
    {
        if (Enabled)
        {
            Debug.Log("Laser Wall disabled");
            gameObject.GetComponent<BoxCollider>().enabled = false;
            LaserMesh.enabled = false;
            this.GetComponent<AudioSource>().PlayOneShot(DisableSound);
            Enabled = false;
        }       
    }
}
