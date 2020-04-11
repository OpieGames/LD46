using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damageable : MonoBehaviour
{
    public int MaxHealth = 100;
    private int CurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int Damage)
    {
        Debug.Log(transform.name + " took " + Damage + " damage!");
        CurrentHealth -= Damage;
        if (CurrentHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(transform.name + " died!");
    }
}
