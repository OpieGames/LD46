﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TowerType
{
    Basic,
    Sniper,
    Shield,
    LaserWall,
}

public class Tower : MonoBehaviour
{
    public TowerType Type = TowerType.Basic;
    public GameObject Player;
    public GameObject Pizza;
    public GameObject Projectile;
    public GameObject ProjectileHolder;
    public GameObject ExplosionParticle;

    [Header("Stats")]
    [Range(0.0f, 40.0f)] public float Range = 10.0f;
    public float AttackTime = 3.0f;
    [Range(0.0f, 40.0f)] public float ProjectileSpeed = 15.0f;
    [Range(0.0f, 0.1f)] public float ProjectileInaccuracy = 0.0f;
    [Range(0.0f, 2.0f)] public float ShotPredictionFudgeFactor = 0.6f;

    private float timeSinceAttack;
    private int shotCount = 0;
    private int layerPizza;
    public LayerMask layersToIgnore;
    private bool losToPizza = false;
    public AudioClip fireSound;
    public AudioClip chargeSound;
    private bool chargeSoundFired = false;
    private AudioSource audioSource;

    void Start()
    {
        layerPizza = LayerMask.NameToLayer("Pizza");
        layersToIgnore = ~layersToIgnore.value;
        
        if (Projectile == null) { Debug.LogErrorFormat("{0}: Projectile reference is not set!", transform.name); }

        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) { Debug.LogErrorFormat("{0}: GameObject with tag 'Player' couldn't be found in the scene!", transform.name); }
        Pizza = GameObject.FindGameObjectWithTag("Pizza");
        if (Pizza == null) { Debug.LogErrorFormat("{0}: GameObject with tag 'Pizza' couldn't be found in the scene!", transform.name); }
        
        if (Type == TowerType.Sniper)
        {
            AttackTime *= 2.0f;
            timeSinceAttack = 0.0f;
        }
        else
        {
            timeSinceAttack = AttackTime;
        }


        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        float distToPizza = Vector3.Distance(transform.position, Pizza.transform.position);
        if (distToPizza <= Range)
        {
            losToPizza = CanLOS();
        }

        if (Type == TowerType.Sniper && distToPizza <= Range && CanLOS())
        {
            if (!chargeSoundFired && timeSinceAttack >= AttackTime - 1.7f) // ignore the initial max time
            {
                audioSource.PlayOneShot(chargeSound);
                chargeSoundFired = true;
            }
        }

        if (timeSinceAttack >= AttackTime)
        {
            bool fired = TowerTick();
            if (fired)
            {
                timeSinceAttack = 0.0f;
                if (Type == TowerType.Sniper)
                    chargeSoundFired = false;
            }
        }
        else
        {
            timeSinceAttack += 1.0f * Time.deltaTime;
        }
        
        if (Type == TowerType.Sniper)
        {
            if (shotCount > 2)
            {
                Instantiate(ExplosionParticle, transform.position, new Quaternion());
                Destroy(this.gameObject);
            }
        }
    }

    private bool CanLOS()
    {
        Vector3 pizzaDir = Pizza.transform.position - ProjectileHolder.transform.position;
        pizzaDir.y += 0.5f;
        RaycastHit hit;
        if (Physics.Raycast(ProjectileHolder.transform.position, pizzaDir, out hit, Range, layersToIgnore))
        {
            if (hit.transform.gameObject.layer == layerPizza)
            {
                pizzaDir = PredictedPizzaTarget() - ProjectileHolder.transform.position;
                if (!Physics.Raycast(ProjectileHolder.transform.position, pizzaDir, out hit, pizzaDir.magnitude, layersToIgnore))
                {
                    return true;
                    // Debug.DrawRay(ProjectileHolder.transform.position, pizzaDir, Color.yellow, 0.1f);
                }
            }
            // Debug.DrawRay(ProjectileHolder.transform.position, pizzaDir, Color.green, 0.1f);
        }
        return false;
    }

    private void InvokeTowerTick()
    {
        if (Type == TowerType.Sniper)
        {
            InvokeRepeating(nameof(TowerTick), 0.0f, AttackTime * 2.0f);
        }
        else
        {
            InvokeRepeating(nameof(TowerTick), 0.0f, AttackTime);
        }
    }
    
    bool TowerTick()
    {
        float distToPizza = Vector3.Distance(transform.position, Pizza.transform.position);
        if (distToPizza <= Range && losToPizza)
        {
            switch (Type)
            {
                case TowerType.Basic:
                    BasicTower();
                    audioSource.PlayOneShot(fireSound);
                    break;
                case TowerType.LaserWall:
                    break;
                case TowerType.Shield:
                    break;
                case TowerType.Sniper:
                    SniperTower();
                    audioSource.PlayOneShot(fireSound);
                    break;
                default:
                    BasicTower();
                    audioSource.PlayOneShot(fireSound);
                    break;
            }
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision other)
    {
        BaseProjectile proj = other.gameObject.GetComponent<BaseProjectile>();
        //Debug.LogFormat("on collision enter (on: {0} | from: {1})", this.name, other.gameObject.name);
        //Debug.LogFormat("proj: {0} | parryable? {1} | hasbeenparried? {2}", proj, proj.Parryable, proj.HasBeenParried);
        if (proj && proj.Parryable && proj.HasBeenParried)
        {
            Debug.LogFormat("{0} hit by parried projectile!", this.name);
            Instantiate(ExplosionParticle, transform.position, new Quaternion());
            Pizza.GetComponent<Pizza>().PathFollower.Speed += 0.25f;
            Destroy(proj.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    private void OnDrawGizmos()
    {
        if (!Pizza)
            return;
        Gizmos.color = Color.red;
        Vector3 targetLoc = PredictedPizzaTarget();
        Gizmos.DrawLine(ProjectileHolder.transform.position, targetLoc);
    }

    private Vector3 PredictedPizzaTarget()
    {
        Vector3 pizzaPos = Pizza.transform.position;
        pizzaPos.y += 0.5f;
        
        float distance = (transform.position - pizzaPos).magnitude;
        float flightTime = distance / ProjectileSpeed;
        flightTime += Random.Range(-ProjectileInaccuracy, ProjectileInaccuracy);

        return pizzaPos + ((Pizza.transform.forward * (Pizza.GetComponent<PathFollower>().CurrentSpeed * ((distance/Range) * ShotPredictionFudgeFactor))) * flightTime);
    }

    private void BasicTower()
    {
        //Debug.LogFormat("{0} is in range of pizza! ({1})", transform.name, distToPizza);
        GameObject projGO = Instantiate(Projectile, ProjectileHolder.transform.position, Quaternion.identity);
        BaseProjectile proj = projGO.GetComponent<BaseProjectile>();
        proj.TowerFiredFrom = this.gameObject;

        Vector3 targetLoc = PredictedPizzaTarget();
        projGO.transform.LookAt(targetLoc);

        float rand = Random.Range(0.0f, 1.0f);

        if (rand <= 0.5f)
        {
            proj.Parryable = true;
            projGO.GetComponent<Rigidbody>().AddForce(projGO.transform.forward * ProjectileSpeed * 90);
            projGO.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            shotCount = 0;
            projGO.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("albedo_", new Color(0.8679245f, 0.05322179f, 0.4156687f, 1.0f));
        }
        else
        {
            proj.Parryable = false;
            projGO.GetComponent<Rigidbody>().AddForce(projGO.transform.forward * ProjectileSpeed * 90);
            shotCount++;
        }

        Destroy(projGO, 4.0f); //TODO: add better cleanup
    }
    
    private void SniperTower()
    {
        //Debug.LogFormat("{0} is in range of pizza! ({1})", transform.name, distToPizza);
        GameObject projGO = Instantiate(Projectile, ProjectileHolder.transform.position, Quaternion.identity);
        BaseProjectile proj = projGO.GetComponent<BaseProjectile>();
        proj.TowerFiredFrom = this.gameObject;

        Vector3 targetLoc = PredictedPizzaTarget();
        projGO.transform.LookAt(targetLoc);

        proj.Parryable = false;
        projGO.GetComponent<Rigidbody>().AddForce(projGO.transform.forward * ProjectileSpeed * 160);
        shotCount++;

        Destroy(projGO, 4.0f); //TODO: add better cleanup
    }
}
