using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakableWall : MonoBehaviour, IBreakable
{
    /// <summary>
    /// Wall health; how many hits it can withstand
    /// </summary>
    public int health;

    public bool isBroken = false;

    public void Break(int damage)
    {
        // reduce wall health
        health -= damage;
        // destroy if health falls below 0
        if(health <= 0)
        {
            gameObject.SetActive(false);
            isBroken = true;
        }
    }

    private void Start()
    {
        if(isBroken)
            gameObject.SetActive(false);
    }
}
