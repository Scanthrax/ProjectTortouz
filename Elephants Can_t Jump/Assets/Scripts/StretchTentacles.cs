using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class StretchTentacles : MonoBehaviour {

    public Tentacle tentacleArm;
    public float scale = 0.03f;
    public float rate = 0.01f;
    public SpriteRenderer rend;
    public Transform rot;
    public CircleCollider2D cc;
    public Vector2 wallPos;

    public bool expand = false;
    public bool canExpand = true;




    void Start()
    {
        ClickPoint.notifyTentacle += ThrowTentacle;
        rend = GetComponentInChildren<SpriteRenderer>();
        rot = transform.parent;
        cc = GetComponent<CircleCollider2D>();
    }

    void ThrowTentacle(Tentacle tent, Vector2 aim)
    {
        // only throw if the correct input is pressed and can expand
        if (tent != tentacleArm || !canExpand) return;
        // aim the tentacle
        rot.right = -new Vector2(transform.position.x, transform.position.y) + aim;
        // tentacle can expand
        expand = true;
        // tentacle cannot be thrown while it is in the process of being thrown already
        canExpand = false;

        // tentacle starts expanding forward
        rate = Mathf.Abs(rate);
    }

    void Update()
    {
        if(expand)
        {
            // expand the tentacle
            transform.localPosition = new Vector3(scale, 0f, 0f);
            rend.size = new Vector2(2 * scale, rend.size.y);

            // the trigger will be at the end of the tentacle
            cc.offset = new Vector2(scale, 0f);

            scale += rate;

            if (scale > 0.5f)
            {
                rate = -rate;
            }

            if(scale <= 0f)
            {
                expand = false;
                canExpand = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        if (hit.point == Vector2.zero)
        {
            print("ERROR");
            return;
        }
        Debug.Log("Point of contact: " + hit.point);
        wallPos = hit.point;
    }
}
