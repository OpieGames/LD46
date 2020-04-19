using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Pizza : MonoBehaviour
{
    public int CurrentHealth = 6;
    public int MaxHealth = 6;

    private void OnCollisionEnter(Collision other)
    {
        BaseProjectile proj = other.gameObject.GetComponent<BaseProjectile>();
        if (proj)
        {
            Debug.Log("pizza hit by projectile!");
            CurrentHealth--;
            Destroy(other.gameObject);

            if (CurrentHealth <= 0)
            {
                SceneManager.LoadScene("Scenes/Lose");
            }
        }
    }

}
