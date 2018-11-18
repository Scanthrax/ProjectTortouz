using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Alien", menuName = "Alien")]
public class AlienObjects : ScriptableObject
{
    public Sprite[] sprites;
    public bool isCollected;
}
