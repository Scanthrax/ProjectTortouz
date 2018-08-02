using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Room : MonoBehaviour {


    public Transform player;
    public delegate void RoomChange(Room fromRoom, Room toRoom);
    public static event RoomChange roomChange;
    Room thisRoom;

    void Awake()
    {
        thisRoom = this;
    }
    private void Start()
    {
        player = CameraCenterpoints.player.transform;
        player.GetComponent<PlayerMovement>().room = thisRoom;
    }
    void Update()
    {
        if(player != CameraCenterpoints.player.transform)
        {
            player = CameraCenterpoints.player.transform;
        }


        // if the player is outside the bounds of the room
        if (!(player.position.x < transform.position.x - Variables.horzExtent ||
            player.position.x > transform.position.x + Variables.horzExtent ||
            player.position.y < transform.position.y - Variables.vertExtent ||
            player.position.y > transform.position.y + Variables.vertExtent ))
        {
            // if new room is entered, change rooms
            if(player.GetComponent<PlayerMovement>().room != thisRoom)
            {
                roomChange(player.GetComponent<PlayerMovement>().room, thisRoom);
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
