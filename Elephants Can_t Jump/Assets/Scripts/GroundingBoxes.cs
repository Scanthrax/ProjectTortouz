using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class GroundingBoxes : MonoBehaviour {

    public PlayerMovement playerMovement;
    public int i;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            playerMovement.groundingBoxes[i] = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            playerMovement.groundingBoxes[i] = false;
        }
    }
}
