using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    private bool open;
    public GameObject ButtonSystem;

    public GameObject door; //moving platform
    public Transform Target; //target point
    public float speed;         //speed
    private Button button;

    public Color color;
    // Use this for initialization

    public bool playSound = true;


    void Start()
    {
        button = ButtonSystem.transform.Find("Button").GetComponent<Button>();
        
        button.door = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if a button is assigned to the platform
        if (SaveController.buttonsDict.ContainsKey(button.keyID))
            open = SaveController.buttonsDict[button.keyID];
        else
            open = button.isPressed;

        if (open)
        {
            if(playSound)
            {
                SoundLibrary.AudioSource[0].clip = SoundLibrary.DoorOpen;
                SoundLibrary.AudioSource[0].Play();
                playSound = false;
            }

            door.transform.position = Vector3.MoveTowards(door.transform.position, Target.position, Time.deltaTime * speed); //move the platform towards the next point in the array
            if(ButtonSystem.transform.Find("Button").transform.position == ButtonSystem.transform.Find("EndP").transform.position)
            {
                button.active = false;
                //playSound = true;
            }
        }

    }


    private void OnValidate()
    {
        door.GetComponent<SpriteRenderer>().color = color;
        ButtonSystem.GetComponentInChildren<SpriteRenderer>().color = color;
    }

    //gizmos to help see the path of the platform
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(door.transform.position, Target.position);

    }
}
