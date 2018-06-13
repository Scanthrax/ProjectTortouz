using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Room : MonoBehaviour {


    public Transform player;
    public Rooms thisRoom;
    void Update()
    {
        //print(player.position.x);
        //print(transform.position.x - Variables.horzExtent);
        print(Variables.room);

        if (player.position.x < transform.position.x - Variables.horzExtent ||
            player.position.x > transform.position.x + Variables.horzExtent ||
            player.position.y < transform.position.y - Variables.vertExtent ||
            player.position.y > transform.position.y + Variables.vertExtent)
        {
            print("Outside of " + thisRoom);
        }
        else
        {
            if(Variables.room != thisRoom)
            {
                Variables.room = thisRoom;
            }
        }
    }


}
