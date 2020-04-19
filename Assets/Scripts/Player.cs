using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] inventory = new int[(int)Pickup.Kind.NumKinds];
    public GameObject Shield;
    public float ParryHoldTime = 0.3f;
    public float ParryRefillRatio = 0.5f;

    private PlayerShield playerShield;
    private CPMMovement playerMovement;
    private float curParryingHoldTime = 0.0f;
    private bool parryingActive = false;
    private bool parryButtonReset = true;
    void Start()
    {
        Shield.SetActive(false);
        curParryingHoldTime = 0.0f;

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
    }

    public float CurrentParryStamina()
    {
        return curParryingHoldTime;
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
