using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI PizzaHealText;
    public TextMeshProUGUI PizzaShieldText;
    public TextMeshProUGUI PizzaBoostText;
    public TextMeshProUGUI PizzaSlowText;
    public TextMeshProUGUI PlayerBoost;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        PizzaHealText.text = player.inventory.pizzaHeals.ToString();
        PizzaShieldText.text = player.inventory.pizzaShields.ToString();
        PizzaBoostText.text = player.inventory.pizzaBoosts.ToString();
        PizzaSlowText.text = player.inventory.pizzaSlows.ToString();
        PlayerBoost.text = player.inventory.playerBoosts.ToString();
    }
}