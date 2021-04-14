using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityAnalytics : MonoBehaviour
{
    public static UnityAnalytics Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnPlayerKillEvent(int kills)
    {
        Analytics.CustomEvent("PlayerKill", new Dictionary<string, object>()
        {
            {"Kills",  kills}
        });

        Debug.Log("*UnityAnalytic* OnKillEvent/ Kills: " + kills);
    }
}
