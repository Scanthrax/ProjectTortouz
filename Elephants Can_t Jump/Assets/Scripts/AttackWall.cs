using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWall : MonoBehaviour
{
    BoxCollider2D bc;
    public KeyCode key;
    private void Start()
    {
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
