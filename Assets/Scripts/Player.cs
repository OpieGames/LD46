using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public struct Inventory
    {
        public int pizzaHeals;
        public int pizzaShields;
        public int pizzaBoosts;
        public int pizzaSlows;
        public int playerBoosts;
    }

    public enum PlayerState
    {
        Playing,
        Menus,
    }

    public Inventory inventory = new Inventory();
    public GameObject Shield;
    public PickupInfo[] pickupInfos = new PickupInfo[(int)PickupInfo.Kind.NumKinds];
    public float ParryHoldTime = 0.3f;
    public float ParryRefillRatio = 0.5f;

    public AudioClip ShieldBlockSound;
    public AudioClip ShieldParrySound;

    public GameObject PauseMenuRef;

    private AudioSource audioSource;
    private PlayerShield playerShield;
    private CPMMovement playerMovement;
    private float defXSens;
    private float defYSens;
    private Pizza pizza;
    private float curParryingHoldTime = 0.0f;
    private bool parryingActive = false;
    private bool parryButtonReset = true;

    private PlayerState state;

    void Start()
    {
        Shield.SetActive(false);
        curParryingHoldTime = 0.0f;

        audioSource = GetComponent<AudioSource>();
        playerShield = Shield.GetComponent<PlayerShield>();
        playerMovement = gameObject.GetComponent<CPMMovement>();
        pizza = GameObject.FindGameObjectWithTag("Pizza").GetComponent<Pizza>();

        playerShield.HoldingPlayer = this;
        state = PlayerState.Playing;

        defXSens = playerMovement.xMouseSensitivity;
        defYSens = playerMovement.yMouseSensitivity;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        RefreshSettings();
    }

    void Update()
    {
        pollItemButtons();

        if (state == PlayerState.Playing)
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
                    curParryingHoldTime = Mathf.Clamp(curParryingHoldTime - ParryRefillRatio * Time.deltaTime, 0.0f,
                        ParryHoldTime);
                }
            }
            else
            {
                ShieldInactive();
                curParryingHoldTime = Mathf.Clamp(curParryingHoldTime - ParryRefillRatio * Time.deltaTime, 0.0f,
                    ParryHoldTime);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case PlayerState.Playing:
                    Debug.Log("menus!");
                    state = PlayerState.Menus;
                    CursorMenuMode();
                    PauseMenu();
                    break;
                case PlayerState.Menus:
                    resume();
                    break;
            }
        }
    }

    private void CursorMenuMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.xMouseSensitivity = 0.0f;
        playerMovement.yMouseSensitivity = 0.0f;
    }

    private void CursorPlayMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMovement.xMouseSensitivity = defXSens;
        playerMovement.yMouseSensitivity = defYSens;
    }

    public void RefreshSettings()
    {
        //any player specific settings here
        
        playerMovement.RefreshSettings();
    }

    void pollItemButtons()
    {
        if (Input.GetButtonDown("ItemSlot1"))
        {
            activateItem(pickupInfos[(int)PickupInfo.Kind.PizzaHeal]);
        }
        else if (Input.GetButtonDown("ItemSlot2"))
        {
            activateItem(pickupInfos[(int)PickupInfo.Kind.PizzaShield]);
        }
        else if (Input.GetButtonDown("ItemSlot3"))
        {
            activateItem(pickupInfos[(int)PickupInfo.Kind.PizzaBoost]);
        }
        else if (Input.GetButtonDown("ItemSlot4"))
        {
            activateItem(pickupInfos[(int)PickupInfo.Kind.PizzaSlow]);
        }
        else if (Input.GetButtonDown("ItemSlot5"))
        {
            activateItem(pickupInfos[(int)PickupInfo.Kind.PlayerBoost]);
        }
    }

    void activateItem(PickupInfo info)
    {
        Debug.Log("Attempted to activate " + info.kind.ToString());
        switch (info.kind)
        {
            case PickupInfo.Kind.PizzaHeal:
                if (inventory.pizzaHeals > 0)
                {
                    inventory.pizzaHeals--;
                    pizza.Heal();
                }
                break;
            case PickupInfo.Kind.PizzaShield:
                if (inventory.pizzaShields > 0)
                {
                    inventory.pizzaShields--;
                    StartCoroutine(pizza.Shield(info.effectDuration));
                }
                break;
            case PickupInfo.Kind.PizzaBoost:
                if (inventory.pizzaBoosts > 0)
                {
                    inventory.pizzaBoosts--;
                    StartCoroutine(pizza.GetComponent<PathFollower>().ApplyBoost(info.effectDuration, info.effectStrength));
                }
                break;
            case PickupInfo.Kind.PizzaSlow:
                if (inventory.pizzaSlows > 0)
                {
                    inventory.pizzaSlows--;
                    StartCoroutine(pizza.GetComponent<PathFollower>().ApplySlow(info.effectDuration, info.effectStrength));
                }
                break;
            case PickupInfo.Kind.PlayerBoost:
                if (inventory.playerBoosts > 0)
                {
                    inventory.playerBoosts--;
                    // TODO: Apply player boost
                }
                break;
            default:
                break;
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

    #region UI

    public void PauseMenu()
    {
        PauseMenuRef.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResumeClicked()
    {
        FindObjectOfType<UIManager>().DestroySettingsMenu();
        resume();
    }

    private void resume()
    {
        state = PlayerState.Playing;
        CursorPlayMode();
        RefreshSettings();
        PauseMenuRef.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void SettingsClicked()
    {
        FindObjectOfType<UIManager>().CreateSettingsMenu();
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
    #endregion
}
