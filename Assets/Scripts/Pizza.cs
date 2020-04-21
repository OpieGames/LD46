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
    public AudioClip HurtSound;
    public AudioClip HealSound;
    public AudioClip ShieldSound;
    public AudioClip SlowSound;
    public AudioClip BoostSound;
    public int TowersKilled = 0;
    public PathFollower PathFollower;

    private AudioSource audioSource;
    public void Start()
    {
        shieldGraphic.enabled = shielded;
        audioSource = GetComponent<AudioSource>();
        PathFollower = GetComponent<PathFollower>();
        
        InvokeRepeating(nameof(TowerCheckTick), 1.0f, 1.0f);
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

    void TowerCheckTick()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        if (towers.Length <= 0)
        {
            Debug.Log("all towers dead, zoomin'");
            PathFollower.Speed = 8.0f;
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
        audioSource.PlayOneShot(HurtSound);
        if (CurrentHealth <= 0)
        {
            SceneManager.LoadScene("Scenes/Lose");
        }
    }

    public void Heal()
    {
        CurrentHealth = Mathf.Min(CurrentHealth + 1, MaxHealth);
        audioSource.PlayOneShot(HealSound);
    }

    public IEnumerator Shield(float duration)
    {
        shielded = true;
        audioSource.PlayOneShot(ShieldSound);
        yield return new WaitForSeconds(duration);
        shielded = false;
    }

    public void PizzaBoosted()
    {
        audioSource.PlayOneShot(BoostSound);
    }

    public void PizzaSlowed()
    {
        audioSource.PlayOneShot(SlowSound);
    }
}
