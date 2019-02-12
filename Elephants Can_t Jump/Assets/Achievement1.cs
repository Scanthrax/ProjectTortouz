using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Achievement1 : MonoBehaviour
{
    // in the inspector, set the achievement ID
    public string achievementID;

    // set a boolean that determines whether or not the achievement has been achieved
    // by default, it is set to true so it doesn't immediately pass the condition
    bool achieved = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // this checks if the achievement has been achieved & spits out the result in the bool
            SteamUserStats.GetAchievement(achievementID, out achieved);

            // if we don't have the achievement yet, we can say that we've achieved it
            if (!achieved)
            {
                // set the achievement
                SteamUserStats.SetAchievement(achievementID);
                // store it on steam's side
                SteamUserStats.StoreStats();
            }
        }
    }
}
