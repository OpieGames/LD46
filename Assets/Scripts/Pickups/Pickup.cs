using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    public PickupInfo info;
    Collider col;
    public AudioClip pickupSound;

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
            player.GetComponent<AudioSource>().PlayOneShot(pickupSound);
            switch (info.kind)
            {
                case PickupInfo.Kind.PizzaHeal:
                    player.inventory.pizzaHeals++;
                    Debug.Log("Picked up a pickup with type " + info.kind.ToString() + ", new count: " + player.inventory.pizzaHeals);
                    break;
                case PickupInfo.Kind.PizzaShield:
                    player.inventory.pizzaShields++;
                    Debug.Log("Picked up a pickup with type " + info.kind.ToString() + ", new count: " + player.inventory.pizzaShields);
                    break;
                case PickupInfo.Kind.PizzaBoost:
                    player.inventory.pizzaBoosts++;
                    Debug.Log("Picked up a pickup with type " + info.kind.ToString() + ", new count: " + player.inventory.pizzaBoosts);
                    break;
                case PickupInfo.Kind.PizzaSlow:
                    player.inventory.pizzaSlows++;
                    Debug.Log("Picked up a pickup with type " + info.kind.ToString() + ", new count: " + player.inventory.pizzaSlows);
                    break;
                case PickupInfo.Kind.PlayerBoost:
                    player.inventory.playerBoosts++;
                    Debug.Log("Picked up a pickup with type " + info.kind.ToString() + ", new count: " + player.inventory.playerBoosts);
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pickup trigger entered");
        Player player = other.GetComponent<Player>();
        if (!player) return;
        // PickedUp(player);
        PickedUp(player);
        GameObject.Destroy(this.gameObject);
    }
}
