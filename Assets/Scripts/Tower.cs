using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType {
    Basic,
    Sniper,
    Shield,
    LaserWall
}

public class Tower : MonoBehaviour
{
    public TowerType type = TowerType.Basic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
