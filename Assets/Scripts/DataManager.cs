using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private const string TotalStarsKey = "TotalStars";

    // Method to set the total stars
    public static void SetStars(int stars)
    {
        PlayerPrefs.SetInt(TotalStarsKey, stars);
        PlayerPrefs.Save();
    }

    // Method to get the total stars
    public static int GetStars()
    {
        return PlayerPrefs.GetInt(TotalStarsKey, 0); // Default to 0 if no stars have been set
    }


    private const string TotalCoinsKey = "TotalCoins";

    public static void SetCoins(int value)
    {
        PlayerPrefs.SetInt(TotalCoinsKey, value);
        PlayerPrefs.Save();
    }

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(TotalCoinsKey, 0);
    }

    private const string TotalCrystalsKey = "TotalCrystalsKey";

    public static void SetCrystals(int value)
    {
        PlayerPrefs.SetInt(TotalCrystalsKey, value);
        PlayerPrefs.Save();
    }

    public static int GetCrystals()
    {
        return PlayerPrefs.GetInt(TotalCrystalsKey, 0);
    }


}
