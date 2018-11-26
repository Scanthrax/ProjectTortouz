using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

[Serializable]
public class Save
{
    // save the position of Pengin & Akkoro
    [SerializeField]
    public float x, y, z, camX, camY, camZ;
    // which direction were they facing when they last saved?
    [SerializeField]
    public int faceDirection;
    // used for the offset the 1st time they load into a new room
    [SerializeField]
    public bool lastSave;

    [SerializeField]
    public Dictionary<string, bool> buttonsDict = new Dictionary<string, bool>();
    [SerializeField]
    public Dictionary<string, bool> breakableDict = new Dictionary<string, bool>();
    [SerializeField]
    public Dictionary<string, bool> alienCollectables = new Dictionary<string, bool>();

}

