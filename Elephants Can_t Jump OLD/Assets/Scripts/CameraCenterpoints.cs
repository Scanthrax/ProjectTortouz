﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CameraCenterpoints : MonoBehaviour {

    Camera cam;

    public GameObject[] playerGameObjects;
    public static GameObject player;

    float lerp;
    public bool panCam;
    

    public delegate void RoomName();
    public static event RoomName roomName;

    private void Awake()
    {
        player = playerGameObjects[0];
    }

    void Start ()
    {
        panCam = false;
        cam = Camera.main;
        cam.transform.position = player.GetComponent<PlayerMovement>().room.transform.position;
        
        Room.roomChange += ChangeRoom;
        
    }



    void ChangeRoom(Room from, Room to)
    {
        // pause the game while room is changing
        if(Time.timeScale != 0f) Time.timeScale = 0f;
        // increment lerp
        lerp += 0.03f;
        // update camera position
        cam.transform.position = Vector3.Lerp(from.transform.position, to.transform.position, lerp);
        // if camera destination reached...
        if (lerp > 1f)
        {
            // reset lerp for next change
            lerp = 0f;
            // update camera position
            cam.transform.position = to.transform.position;
            // set static variable to destination room
            Variables.room = to.transform;
            //roomName();
            // unpause the game
            Time.timeScale = 1f;
            player.GetComponent<PlayerMovement>().room = to;
        }
    }
}
