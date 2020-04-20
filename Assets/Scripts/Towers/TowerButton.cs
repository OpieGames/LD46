using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public LaserWall laserWall;

    public void Activate()
    {
        laserWall.Disable();
        Debug.LogWarning("Button activated");
    }
}
