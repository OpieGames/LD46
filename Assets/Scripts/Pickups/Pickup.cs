using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    // Used to index into player's inventory
    // NumKinds must be last to pick up the number of categories we have
    public enum Kind
    {
        PizzaHeal, PizzaShield, PizzaBoost, PizzaSlow,
        PlayerBoost,
        NumKinds
    };

    public float effectDuration;
    [Range(0.0f, 100.0f)]
    public float effectStrength;

    public Kind kind;

    Collider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void PickedUp(Player player)
    {
        if (player)
        {
            player.inventory[(int)kind]++;
            Debug.Log("Picked up a pickup with type " + kind.ToString() + ", new count: " + player.inventory[(int)kind]);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pickup trigger entered");
        Player player = other.GetComponent<Player>();
        if (!player) return;
        PickedUp(player);

        GameObject.Destroy(this.gameObject);
    }
}
