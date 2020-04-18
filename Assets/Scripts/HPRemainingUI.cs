using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HPRemainingUI : MonoBehaviour
{

    private Pizza pizza;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        pizza = GameObject.FindGameObjectWithTag("Pizza").GetComponent<Pizza>();
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Slices remaining: " + pizza.CurrentHealth + "/" + pizza.MaxHealth;
    }
}
