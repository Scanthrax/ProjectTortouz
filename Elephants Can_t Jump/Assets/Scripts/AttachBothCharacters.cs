using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AttachBothCharacters : MonoBehaviour
{
    /// <summary>
    /// Layer for Akkoro
    /// </summary>
    int akkoro;
    public KeyCode attach;

    private void Start()
    {
        akkoro = LayerMask.NameToLayer("Akkoro");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Variables.controlling == Controlling.Akkoro)
        {
            if (collision.gameObject.layer == akkoro)
            {
                if (Input.GetKeyDown(attach))
                {
                    Controller.switchUnits(Controlling.Both, Vector3.zero);
                }
            }
        }
    }
}
