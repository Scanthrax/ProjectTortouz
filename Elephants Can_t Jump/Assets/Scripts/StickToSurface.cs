using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour
{
    // get circle collider component of aim tentacle
    public static CircleCollider2D cc;
    public PlayerMove playerMove;

    private void Start()
    {
        // assign circle collider component
        cc = GetComponent<CircleCollider2D>();

        Physics2D.queriesStartInColliders = false;
    }

    private void LateUpdate()
    {
        // adjust the offset of the circle collider to be at the end of the tentacle
        cc.offset = new Vector2(transform.localPosition.x, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // detect what it is that we hit
        RaycastHit2D? hit = Physics2D.Raycast(transform.position, transform.right);
        // display if there is an error when colliding
        if (hit == null || hit.Value.point == Vector2.zero)
        {
            print("ERROR");
            Debug.Break();
            return;
        }
        // we have hit a surface; assign to tentacle's anchor position
        playerMove.aimTentacle.anchorPos = hit.Value.point;
        // disable the circle collider since we have a hit
        cc.enabled = false;
    }
}
