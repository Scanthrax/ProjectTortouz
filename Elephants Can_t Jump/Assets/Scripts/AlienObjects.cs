using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Alien", menuName = "Alien")]
public class AlienObjects : ScriptableObject
{
    public Sprite sprite;
    public string name;
    public bool isCollected;
}
