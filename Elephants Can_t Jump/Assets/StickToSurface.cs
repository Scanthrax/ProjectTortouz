using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour
{
    Vector2 wallPos;
    public static CircleCollider2D cc;

    public delegate void PointOfContact();
    public static event PointOfContact pointOfContact;

    private void Start()
    {
        cc = GetComponent<CircleCollider2D>();
    }

    private void LateUpdate()
    {
        cc.offset = new Vector2(transform.localPosition.x, 0f);
    }




    void OnTriggerEnter2D(Collider2D collision)
    {
        RaycastHit2D? hit = Physics2D.Raycast(transform.position, transform.right);
        if (hit == null || hit.Value.point == Vector2.zero)
        {
            print("ERROR");
            Debug.Break();
            return;
        }
        Debug.Log("Point of contact: " + hit.Value.point);
        PlayerMove.pointOfContact = hit.Value.point;
        cc.enabled = false;
    }
}
