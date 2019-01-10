using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AttackWall : MonoBehaviour
{
    public BoxCollider2D bc;
    public KeyCode key;
    PlayerMovement pm;
    Animator anim;

    private void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
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

        if((Input.GetKeyDown(key) || Input.GetButtonDown("Wall Bash")) && pm.movement == Movement.Ground && !pm.action)
        {
            anim.SetTrigger("Wallbreak");
            SoundLibrary.AudioSource[1].clip = SoundLibrary.WallBreak[1];
            SoundLibrary.AudioSource[1].volume = 0.35f;
            SoundLibrary.AudioSource[1].Play();
        }
    }

}
