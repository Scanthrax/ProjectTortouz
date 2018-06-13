using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Room : MonoBehaviour {


    public Transform player;
    public Rooms thisRoom;

    public delegate void RoomChange(Transform fromRoom, Transform toRoom);
    public static event RoomChange roomChange;


    void Update()
    {
        // outside of room
        if (player.position.x < transform.position.x - Variables.horzExtent ||
            player.position.x > transform.position.x + Variables.horzExtent ||
            player.position.y < transform.position.y - Variables.vertExtent ||
            player.position.y > transform.position.y + Variables.vertExtent )
        {
            print("Outside of " + thisRoom);
        }
        // inside of room
        else
        {
            if(Variables.room != transform)
            {
                roomChange(Variables.room, transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position - new Vector3(Variables.horzExtent, -Variables.vertExtent, 0f), transform.position + new Vector3(Variables.horzExtent, Variables.vertExtent, 0f));
        Gizmos.DrawLine(transform.position - new Vector3(Variables.horzExtent, Variables.vertExtent, 0f), transform.position + new Vector3(Variables.horzExtent, -Variables.vertExtent, 0f));
        Gizmos.DrawLine(transform.position - new Vector3(Variables.horzExtent, -Variables.vertExtent, 0f), transform.position + new Vector3(-Variables.horzExtent, -Variables.vertExtent, 0f));
        Gizmos.DrawLine(transform.position - new Vector3(-Variables.horzExtent, Variables.vertExtent, 0f), transform.position + new Vector3(Variables.horzExtent, Variables.vertExtent, 0f));
    }

}
