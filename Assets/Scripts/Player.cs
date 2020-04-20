using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] inventory = new int[(int)Pickup.Kind.NumKinds];
    public GameObject Shield;
    public float ParryHoldTime = 0.3f;
    public float ParryRefillRatio = 0.5f;

    public AudioClip ShieldBlockSound;
    public AudioClip ShieldParrySound;

    private AudioSource audioSource;
    private PlayerShield playerShield;
    private CPMMovement playerMovement;
    private float curParryingHoldTime = 0.0f;
    private bool parryingActive = false;
    private bool parryButtonReset = true;
    void Start()
    {
        Shield.SetActive(false);
        curParryingHoldTime = 0.0f;

        audioSource = GetComponent<AudioSource>();
        playerShield = Shield.GetComponent<PlayerShield>();
        playerMovement = gameObject.GetComponent<CPMMovement>();

        playerShield.HoldingPlayer = this;
    }

    void Update()
    {
        pollItemButtons();

        if (Input.GetButton("Block"))
        {
            ShieldBlock();
        }
        else if (Input.GetButton("Parry") && parryButtonReset)
        {
            if (curParryingHoldTime <= 0.001f)
            {
                ShieldParry();
                curParryingHoldTime += 1.0f * Time.deltaTime;
                parryingActive = true;
            }
            else if (parryingActive)
            {
                ShieldParry();
                curParryingHoldTime += 1.0f * Time.deltaTime;
                if (curParryingHoldTime >= ParryHoldTime)
                {
                    parryingActive = false;
                    parryButtonReset = false;
                }
            }
            else
            {
                Debug.Log("Player tried to parry but can't!");
                ShieldInactive();
                parryingActive = false;
                parryButtonReset = false;
                curParryingHoldTime = Mathf.Clamp(curParryingHoldTime - ParryRefillRatio * Time.deltaTime, 0.0f, ParryHoldTime);
            }
        }
        else
        {
            ShieldInactive();
            curParryingHoldTime = Mathf.Clamp(curParryingHoldTime - ParryRefillRatio * Time.deltaTime, 0.0f, ParryHoldTime);
        }

        if (Input.GetButtonUp("Parry"))
        {
            parryButtonReset = true;
        }

        if (Input.GetButtonDown("Use") || Input.GetButtonDown("Parry"))
        {
            // Debug.Log("PRESSED USE");
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2.0f))
            {
                // Debug.Log("Got a Hit!");
                TowerButton but;
                but = hit.transform.gameObject.GetComponent<TowerButton>();
                if (but)
                {
                    // Debug.Log("Got a button!");
                    but.Activate();
                }
            }
        }
    }

    void pollItemButtons()
    {
        if (Input.GetButtonDown("ItemSlot1"))
        {
            activateItem(Pickup.Kind.PizzaHeal);
        }
        else if (Input.GetButtonDown("ItemSlot2"))
        {
            activateItem(Pickup.Kind.PizzaHeal);
        }
        else if (Input.GetButtonDown("ItemSlot3"))
        {
            activateItem(Pickup.Kind.PizzaHeal);
        }
        else if (Input.GetButtonDown("ItemSlot4"))
        {
            activateItem(Pickup.Kind.PizzaHeal);
        }
        else if (Input.GetButtonDown("ItemSlot5"))
        {
            activateItem(Pickup.Kind.PizzaHeal);
        }
    }

    void activateItem(Pickup.Kind kind)
    {
        if (inventory[(int)kind] > 0)
        {
            inventory[(int)kind]--;

            switch(kind)
            {
                case Pickup.Kind.PizzaHeal:
                break;
                case Pickup.Kind.PizzaShield:
                break;
                case Pickup.Kind.PizzaBoost:
                break;
                case Pickup.Kind.PizzaSlow:
                break;
                case Pickup.Kind.PlayerBoost:
                break;
                default:
                break;
            }
        }
    }

    public float CurrentParryStamina()
    {
        return curParryingHoldTime;
    }

    public void SuccessfulParry()
    {
        audioSource.clip = ShieldParrySound;
        audioSource.Play();
    }

    public void SuccessfulBlock()
    {
        audioSource.clip = ShieldBlockSound;
        audioSource.Play();
    }

    private void ShieldInactive()
    {
        Shield.SetActive(false);
        playerShield.CurrentState = ShieldState.Inactive;
        playerMovement.ResetMaxTopSpeed();
    }
    private void ShieldBlock()
    {
        Shield.SetActive(true);
        playerShield.CurrentState = ShieldState.Blocking;
        playerMovement.maxTopSpeed = 3.0f;
    }
    private void ShieldParry()
    {
        Shield.SetActive(true);
        playerShield.CurrentState = ShieldState.Parrying;
        playerMovement.maxTopSpeed = 3.0f;
    }
}
