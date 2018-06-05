using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IBreakable component = collision.GetComponent<IBreakable>();
        if (component != null)
        {
            component.Break(1);
        }
    }
}
