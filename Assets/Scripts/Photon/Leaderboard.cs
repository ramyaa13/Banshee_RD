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
    public TextMeshProUGUI[] Score;
    public TextMeshProUGUI[] KillandDeaths;
    private bool x = true;

    // Start is called before the first frame update
    void Start()
    {
        x = true;
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.CustomProperties["Kills"] descending select player).ToList();

        int i = 0;
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

            if(player.CustomProperties["Gems"] != null)
            {
                Score[i].text = player.CustomProperties["Gems"].ToString();
            }
            else
            {
                Score[i].text = "0";
            }

            i++;
        }
    }

    public void Update()
    {
        if(x)
            LeaderboardUI.SetActive(Input.GetKey(KeyCode.Tab));

    }

    public void PlayerWins()
    {
        x = false;
        string PlayerWin = PlayerName[0].text;
        Gamemanager.instance.IsPlayerWins(PlayerWin);
    }
}
