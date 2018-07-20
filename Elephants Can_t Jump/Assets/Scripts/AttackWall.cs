using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AttackWall : MonoBehaviour
{
    BoxCollider2D bc;
    public KeyCode key;
    PlayerMovement pm;
    Animator anim;

    private void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IBreakable component = collision.GetComponent<IBreakable>();
        if (component != null)
        {
            component.Break(1);
        }
    }

    //attackCollider.transform.eulerAngles = new Vector3(0, -180, 0);

    private void Update()
    {
        bc.offset = new Vector2(pm.faceDir*Mathf.Abs(bc.offset.x), bc.offset.y);

        if(Input.GetKeyDown(key) && pm.movement == Movement.Ground)
        {
            anim.SetTrigger("Wallbreak");
        }
    }

}
