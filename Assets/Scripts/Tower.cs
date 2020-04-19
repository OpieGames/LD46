using System;
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

    

    private int shotCount = 0;
    void Start()
    {
        if (Projectile == null) { Debug.LogErrorFormat("{0}: Projectile reference is not set!", transform.name); }

        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) { Debug.LogErrorFormat("{0}: GameObject with tag 'Player' couldn't be found in the scene!", transform.name); }
        Pizza = GameObject.FindGameObjectWithTag("Pizza");
        if (Pizza == null) { Debug.LogErrorFormat("{0}: GameObject with tag 'Pizza' couldn't be found in the scene!", transform.name); }

        InvokeRepeating("TowerTick", 0.0f, AttackTime);

    }

    void Update()
    {

    }

    void TowerTick()
    {
        float distToPizza = Vector3.Distance(transform.position, Pizza.transform.position);
        switch (Type)
        {
            case TowerType.Basic:
            case TowerType.LaserWall:
            case TowerType.Shield:
            case TowerType.Sniper:
            default:
                if (distToPizza <= Range)
                {
                    //Debug.LogFormat("{0} is in range of pizza! ({1})", transform.name, distToPizza);
                    GameObject projGO = Instantiate(Projectile, ProjectileHolder.transform.position, Quaternion.identity);
                    BaseProjectile proj = projGO.GetComponent<BaseProjectile>();
                    proj.TowerFiredFrom = this.gameObject;

                    Vector3 targetLoc = PredictedPizzaTarget();
                    projGO.transform.LookAt(targetLoc);

                    if (shotCount >= 2)
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
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        BaseProjectile proj = other.gameObject.GetComponent<BaseProjectile>();
        Debug.LogFormat("on collision enter (on: {0} | from: {1})", this.name, other.gameObject.name);
        Debug.LogFormat("proj: {0} | parryable? {1} | hasbeenparried? {2}", proj, proj.Parryable, proj.HasBeenParried);
        if (proj && proj.Parryable && proj.HasBeenParried)
        {
            Debug.LogFormat("{0} hit by parried projectile!", this.name);
            Instantiate(ExplosionParticle, transform.position, new Quaternion());
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
        float distance = (transform.position - Pizza.transform.position).magnitude;
        float flightTime = distance / ProjectileSpeed;
        flightTime += Random.Range(-ProjectileInaccuracy, ProjectileInaccuracy);

        return Pizza.transform.position + ((Pizza.transform.forward * Pizza.GetComponent<PathFollower>().CurrentSpeed) * flightTime);
    }
}
