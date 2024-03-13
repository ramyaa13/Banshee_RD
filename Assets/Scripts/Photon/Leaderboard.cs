using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class Leaderboard : MonoBehaviour
{
    public float refreshRate = 1f;

    public GameObject LeaderboardUI;

    public GameObject[] slots;
    public TextMeshProUGUI[] PlayerName;
   
    public TextMeshProUGUI[] KillandDeaths;
    private bool x = true;

    
    private int stars = 4;
    private int crystals;
    public string LocalPlayerName;
    // Start is called before the first frame update
    void Start()
    {
        
        x = true;
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }
    public void SetPlayerName(string playerName)
    {
        LocalPlayerName = playerName;
    }
    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.CustomProperties["Kills"] descending select player).ToList();

        int i = 0;
        stars = 4;
        foreach (var player in sortedPlayerList)
        {
            slots[i].SetActive(true);
            PlayerName[i].text = player.NickName;
            


            if (player.CustomProperties["Kills"] != null)
            {
                KillandDeaths[i].text = player.CustomProperties["Kills"] + "/" + player.CustomProperties["Deaths"];
            }
            else
            {
                KillandDeaths[i].text = "0/0";
            }

            if(LocalPlayerName != null)
            {
                if (player.NickName == LocalPlayerName)
                {
                    stars = stars - i;
                    //Debug.Log("stars earned by: " + LocalPlayerName + stars);
                }
            }
            

            i++;
        }
    }

    public void Update()
    {
        //if (x)
        //    LeaderboardUI.SetActive(Input.GetKey(KeyCode.Tab));

    }

    public void PlayerWins()
    {
        x = false;
        //CancelInvoke("Refresh");
        string PlayerWin = PlayerName[0].text;
        Debug.Log(LocalPlayerName + PlayerWin + "crsytal matched");
        if(PlayerWin == LocalPlayerName)
        {
            crystals = 1;
        }
        else
        {
            crystals = 0;
        }
        Gamemanager.instance.IsPlayerWins(PlayerWin, stars, crystals);
    }
}
