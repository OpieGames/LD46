using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Stats")]
    [Range(0.0f, 40.0f)] public float Range = 10.0f;
    public float AttackTime = 3.0f;
    [Range(0.0f, 40.0f)] public float ProjectileSpeed = 15.0f;
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
                    Debug.LogFormat("{0} is in range of pizza! ({1})", transform.name, distToPizza);
                    Vector3 spawnPos = transform.position;
                    spawnPos.y += 2.5f; // eh
                    GameObject proj = Instantiate(Projectile, spawnPos, Quaternion.identity);
                    proj.transform.LookAt(Pizza.transform);
                    proj.GetComponent<Rigidbody>().AddForce(proj.transform.forward * ProjectileSpeed * 100);

                    Destroy(proj, 3.0f);
                }
                break;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
