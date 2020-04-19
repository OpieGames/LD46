using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public GameObject TowerFiredFrom;
    public bool Parryable = false;
    public bool HasBeenParried = false;

    public Gradient ParryableParticleColorOverLife;

    void Start()
    {
        if (Parryable)
        {
            var colOverTime = gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
            colOverTime.color = ParryableParticleColorOverLife;
            this.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("albedo_", new Color(0.8679245f, 0.05322179f, 0.4156687f, 1.0f));
        }
    }

}
