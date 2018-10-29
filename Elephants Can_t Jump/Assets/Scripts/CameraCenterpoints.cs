using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.UI;

public class CameraCenterpoints : MonoBehaviour {

    Camera cam;
    public SaveController saveController;

    public static GameObject player;

    float lerp;
    

    public delegate void RoomName();
    public static event RoomName roomName;


    public Text text;

    void Start ()
    {
        cam = Camera.main;
        cam.transform.position = player.GetComponent<PlayerMovement>().room.transform.position;
        
        Room.roomChange += ChangeRoom;

        Application.targetFrameRate = 30;
    }


    private void FixedUpdate()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }



    void ChangeRoom(Room from, Room to)
    {
        if (cam == null) return;
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
            // unpause the game
            Time.timeScale = 1f;
            player.GetComponent<PlayerMovement>().room = to;

            text.text = to.transform.parent.name;

            saveController.SaveGame(true);
        }
    }
}
