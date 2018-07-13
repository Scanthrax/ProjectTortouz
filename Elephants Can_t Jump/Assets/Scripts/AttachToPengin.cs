using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AttachToPengin : MonoBehaviour
{
    /// <summary>
    /// Layer for Akkoro
    /// </summary>
    int pengin;
    public KeyCode attach;

    private void Start()
    {
        pengin = LayerMask.NameToLayer("Pengin");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == pengin)
        {
            if (Input.GetKeyDown(attach))
            {
                Controller.switchUnits(Controlling.Both, Vector3.zero);
            }
        }
    }
}
