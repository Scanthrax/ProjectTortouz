using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class RoomText : MonoBehaviour {

    Text text;

	void Start () {
        CameraCenterpoints.roomName += changeText;
        text = GetComponent<Text>();
    }


    void changeText()
    {
        text = GetComponent<Text>();
        text.text = "Room: " + Variables.room.parent;
    }
}
