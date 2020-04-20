using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Pizza : MonoBehaviour
{
    public int CurrentHealth = 6;
    public int MaxHealth = 6;
    public bool shielded = false;
    public Transform shieldGraphic;

    public void Update()
    {
        if (shielded)
        {
            shieldGraphic.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        } else {
            shieldGraphic.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        BaseProjectile proj = other.gameObject.GetComponent<BaseProjectile>();
        if (proj)
        {
            //Debug.Log("pizza hit by projectile!");

            Destroy(other.gameObject);

            if (!shielded) TakeDamage();

        }
        
    }

    public void TakeDamage()
    {
        CurrentHealth--;
        if (CurrentHealth <= 0)
        {
            SceneManager.LoadScene("Scenes/Lose");
        }
    }

    public void Heal()
    {
        CurrentHealth = Mathf.Min(CurrentHealth + 1, MaxHealth);
    }

    public IEnumerator Shield(float duration)
    {
        shielded = true;
        yield return new WaitForSeconds(duration);
        shielded = false;
    }
}
