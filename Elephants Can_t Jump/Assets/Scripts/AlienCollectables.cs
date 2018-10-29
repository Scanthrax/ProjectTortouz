using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCollectables : MonoBehaviour {

    public static Dictionary<string, bool> alienDictionary = new Dictionary<string, bool>();
    public AlienObjects[] aliens;

	void Awake ()
    {
		foreach(AlienObjects obj in aliens)
        {
            if(alienDictionary.ContainsKey(obj.name))
                alienDictionary.Add(obj.name, false);
        }
	}
}
