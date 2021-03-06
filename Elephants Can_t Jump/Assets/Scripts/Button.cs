﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Button : MonoBehaviour, IBreakable
{
    public bool active; //used to deactivate button for objects that only have 1 function such as doors that only open.
    public bool isPressed; //boolean used by other objects to see if button is pressed or not. USE THIS BOOL TO CHECK STATE OF BUTTON

    public bool press; // press/release delay system
    private bool release; // press/release delay system
    private bool moving; //bool to make sure the button is done moving before it can be pressed again
    public Transform StartP; //lerping point for button animation
    public Transform EndP; //lerping point for button animation
    private float time1 = 0f; //Lerp time start to end
    public float speed1 = 1f; //speed start to end
    private float time2 = 0f; //Lerp time end to start
    public float speed2 = 1f; //speed end to start

    public KeyCode key; //use key from the inspector

    bool playSound = true;

    public DoorMove door;
    public MovingPlatform platform;

    SpriteRenderer spriteRend;

    public string keyID;

    // Use this for initialization
    void Start ()
    {
        keyID = transform.position.ToString();

        active = true;
        //isPressed = false;
        spriteRend = GetComponent<SpriteRenderer>();

        

        //print("Button Start " + keyID);

        bool temp;
        if (SaveController.buttonsDict.ContainsKey(keyID))
        {
            temp = SaveController.buttonsDict[keyID];
            press = temp;
        }

    }
	
    public void Break(int damage)
    {
        if (!moving)
        {
            
            if (!isPressed && !press)
            {
                press = true;
                print("push");
                if (!SaveController.buttonsDict.ContainsKey(keyID))
                    SaveController.buttonsDict.Add(keyID, true);
                else
                    SaveController.buttonsDict[keyID] = true;
            }
            else if(isPressed && !release)
            {
                release = true;
                print("pull");

                if (!SaveController.buttonsDict.ContainsKey(keyID))
                    SaveController.buttonsDict.Add(keyID, false);
                else
                    SaveController.buttonsDict[keyID] = false;

            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (active)
        {
            //if button is pressed
            if (press == true)
            {
                if(playSound)
                {
                    SoundLibrary.AudioSource[1].clip = SoundLibrary.Button;
                    SoundLibrary.AudioSource[1].Play();
                    playSound = false;
                }
                //lerping of button
                time1 += speed1 * Time.deltaTime;
                this.transform.position = Vector3.Lerp(this.transform.position, EndP.position, time1);
                moving = true; //disable pressing
                isPressed = true;
                if (this.transform.position == EndP.position) //if button is done moving
                {
                    moving = false;
                    time1 = 0f;
                    press = false;
                    playSound = true;
                }

            }

            //if button is pressed a second time
            if (release == true)
            {
                //lerping
                time2 += speed2 * Time.deltaTime;
                this.transform.position = Vector3.Lerp(this.transform.position, StartP.position, time2);
                moving = true; //disable pressing
                isPressed = false;
                if (this.transform.position == StartP.position) //if button is done moving
                {
                    moving = false;
                    time2 = 0f;
                    release = false;
                }

            }

            if (isPressed)
            {
                //spriteRend.color = Color.gray;
            }
            else
            {
                //spriteRend.color = Color.red;
            }
        }

    }
    
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //   if(other.CompareTag("Player")) //check if the player is in the trigger
    //   {
    //        if (Input.GetKeyDown(key) && !moving) //check if key is pressed and if button is done moving
    //        {
    //            if (isPressed == false)
    //            {
    //                Debug.Log("pressing");
    //                press = true;
    //            }
    //            else if (isPressed == true)
    //            {
    //                Debug.Log("de-pressing");
    //                release = true;
    //            }
    //        }
        
    //   }
    //}
}
