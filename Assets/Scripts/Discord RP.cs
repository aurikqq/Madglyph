using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordRP : MonoBehaviour
{
    Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(1276230348862390272, (ulong)Discord.CreateFlags.NoRequireDiscord);
        changeActivity();
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    public void changeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity{
            State = "In Main Menu",
            Details = "Testing DRP",
            Assets = {
                LargeImage = "icon",
                LargeText = "Madglyph"
            },
            Timestamps = {
                Start = 1507665886
            }
        };
        activityManager.UpdateActivity(activity, (res) => {
            Debug.Log("Activity updated.");
        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }
}
