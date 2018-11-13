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
    public float x, y, z;
    [SerializeField]
    public int faceDirection;
    [SerializeField]
    
    public Dictionary<int, bool> activatedItems = new Dictionary<int, bool>();
    public Dictionary<string, bool> alienCollectables = new Dictionary<string, bool>();

}

