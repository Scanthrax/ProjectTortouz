using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Achievement1 : MonoBehaviour
{
    public string achievementID;
    bool temp = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        SteamUserStats.GetAchievement(achievementID, out temp);
        if (!temp)
        {
            SteamUserStats.SetAchievement(achievementID);
            SteamUserStats.StoreStats();
        }
    }
}
