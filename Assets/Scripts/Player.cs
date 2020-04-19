using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] inventory = new int[(int)Pickup.Kind.NumKinds];
    public GameObject Shield;
    public float MaxParryTime = 0.3f;

    private PlayerShield playerShield;
    private CPMMovement playerMovement;
    private float curParryingTime = 0.0f;
    private bool parryingActive = false;
    private bool parryButtonReset = true;
    void Start()
    {
        Shield.SetActive(false);
        curParryingTime = 0.0f;

        playerShield = Shield.GetComponent<PlayerShield>();
        playerMovement = gameObject.GetComponent<CPMMovement>();
    }

    void Update()
    {
        if (Input.GetButton("Block"))
        {
            ShieldBlock();
        }
        else if (Input.GetButton("Parry") && parryButtonReset)
        {
            if (curParryingTime <= 0.001f)
            {
                ShieldParry();
                curParryingTime += 1.0f * Time.deltaTime;
                parryingActive = true;
            }
            else if (parryingActive)
            {
                ShieldParry();
                curParryingTime += 1.0f * Time.deltaTime;
                if (curParryingTime >= MaxParryTime)
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
                curParryingTime = Mathf.Clamp(curParryingTime - 1.0f * Time.deltaTime, 0.0f, MaxParryTime);
            }
        }
        else
        {
            ShieldInactive();
            curParryingTime = Mathf.Clamp(curParryingTime - 1.0f * Time.deltaTime, 0.0f, MaxParryTime);
        }

        if (Input.GetButtonUp("Parry"))
        {
            parryButtonReset = true;
        }
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
