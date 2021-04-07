using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeed : MonoBehaviour
{
    public static string GetKillFeed(string[] killfeed)
    {
        return $"{killfeed[0]} Killed {killfeed[1]}";
    }
}
