using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CameraCenterpoints : MonoBehaviour {

    public Transform startRoom;
    public Camera cam;
    public int selectPoint;
    float lerp;
    public bool panCam;
    
    public GameObject player;

    public delegate void RoomName();
    public static event RoomName roomName;

    void Start ()
    {
        panCam = false;
        cam.transform.position = startRoom.position;
        Variables.room = startRoom;
        cam = Camera.main;
        Room.roomChange += ChangeRoom;
        
    }



    void ChangeRoom(Transform from, Transform to)
    {
        cam = Camera.main;
        // pause the game while room is changing
        if(Time.timeScale != 0f) Time.timeScale = 0f;
        // increment lerp
        lerp += 0.03f;
        // update camera position
        cam.transform.position = Vector3.Lerp(from.position, to.position, lerp);
        // if camera destination reached...
        if (lerp > 1f)
        {
            // reset lerp for next change
            lerp = 0f;
            // update camera position
            cam.transform.position = to.position;
            // set static variable to destination room
            Variables.room = to;
            roomName();
            // unpause the game
            Time.timeScale = 1f;
        }
    }
}
