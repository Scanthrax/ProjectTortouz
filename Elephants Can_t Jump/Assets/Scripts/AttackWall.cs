using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWall : MonoBehaviour
{
    BoxCollider2D bc;
    public KeyCode key;
    PlayerMovement pm;
    private void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IBreakable component = collision.GetComponent<IBreakable>();
        if (component != null)
        {
            component.Break(1);
        }
    }


    private void Update()
    {
        bc.offset = new Vector2(pm.faceDir*Mathf.Abs(bc.offset.x), bc.offset.y);

        if(Input.GetKey(key))
        {
            bc.enabled = true;
        }
        else
        {
            bc.enabled = false;
        }
    }
}
