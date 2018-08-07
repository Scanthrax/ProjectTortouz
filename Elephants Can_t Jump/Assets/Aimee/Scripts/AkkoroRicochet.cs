using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkkoroRicochet : MonoBehaviour
{  
    // How fast Akkoro moves when bouncing around.
    private int akkoroSpeed = 10;
    // The count variable controls when Akkoro goes back to Pengin.
    private int count;
    // These variables below will be used for lerping back to Pengin.
    private GameObject returnToPengin;
    private Transform returnPos;
    // Speed Akkoro takes to lerp/return back to Pengin.
    private float returnSpeed;
    // When true, we can destroy the Akkoro prefab.
    private bool destroyIt;

    public GameObject Pengin;
 
    private void Start()
    {
        // Initializing variables.
        count = 0;
        returnSpeed = 0.07f;
        destroyIt = false;
    }

    private void Update()
    {
        // Akkoro lerps/returns to Pengin.
        if (count >= 5)
        {
            // Debugging.
            print("Return!");
            // Makes the Akkoro prefab destoyable.
            destroyIt = true;

          
            // transform.position is where we currently are, returnPos.position is where we'd like to be,
            // and returnSpeed is how fast we can get from point a to point b. This is the return function.
            transform.position = Vector2.Lerp(transform.position, Pengin.transform.position, returnSpeed);
        }
        // Akkoro is moving and bouncing around the walls until count reaches 5 bounces.
        else if (count < 5)
        {
            //transform.position += transform.right * akkoroSpeed * Time.deltaTime;
        }

        // The raycasting code below allows Akkoro to ricochet around the walls. 
        Ray2D ray = new Ray2D(transform.position, transform.right);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Time.deltaTime * akkoroSpeed);

        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Vector2 v = Vector2.Reflect(ray.direction, hit.normal);
            float rot = 45 - Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, rot);

            // Every time there's a "bounce" from a collision, count is incremented
            count++;
            print("count: " + count);
        }
    }

    // If Akkoro collides with Pengin & destroyIt is set to true, then he's DESTROYED. HAHAHA. Poor kid...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && destroyIt == true)
        {
            Destroy(gameObject);
        }
    }
}
