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
    public MeshRenderer shieldGraphic;

    public void Start()
    {
        shieldGraphic.enabled = shielded;
    }

    public void Update()
    {
        if (shielded && !shieldGraphic.enabled)
        {
            shieldGraphic.enabled = true;
        } else if (!shielded && shieldGraphic.enabled) {
            shieldGraphic.enabled = false;
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
