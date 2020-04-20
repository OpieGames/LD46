using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    public PickupInfo info;
    Collider col;
    public AudioClip pickupSound;

    private float yPos;
    float bounceHeight = 0.3f;
    float bounceSpeed = 2.0f;
    float rotSpeed = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        yPos = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime*rotSpeed, 0), Space.World);
        transform.position = new Vector3(transform.position.x, yPos + Mathf.Sin(Time.time*bounceSpeed)*bounceHeight, transform.position.z);
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
