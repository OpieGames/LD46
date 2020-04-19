using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParryStaminaSlider : MonoBehaviour
{
    private Slider slider;
    private Player player;

    private void Start()
    {
        slider = GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    private void Update()
    {
        slider.value = 1.0f - player.CurrentParryStamina() / player.ParryHoldTime;
    }
}
