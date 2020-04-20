using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Info", menuName = "Pickup Info")]
public class PickupInfo : ScriptableObject
{
    public enum Kind
    {
        PizzaHeal, PizzaShield, PizzaBoost, PizzaSlow,
        PlayerBoost, NumKinds
    };

    public Kind kind;
    // Probably a multiplicative scale
    [Range(0.0f, 100.0f)]
    public float effectStrength;
    // Duration in seconds
    public float effectDuration;
}
