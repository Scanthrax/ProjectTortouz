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

    void Start ()
    {
        panCam = false;
        cam.transform.position = startRoom.position;
        Variables.room = startRoom;
        Room.roomChange += ChangeRoom;
    }



    void ChangeRoom(Transform from, Transform to)
    {
        print("should be changing room");
        Time.timeScale = 0f;
        cam.transform.position = Vector3.Lerp(from.position, to.position, lerp);
        lerp += 0.08f;
        if (lerp > 1f)
        {
            lerp = 0f;
            cam.transform.position = to.position;
            Variables.room = to;
            Time.timeScale = 1f;
        }

    }



}
