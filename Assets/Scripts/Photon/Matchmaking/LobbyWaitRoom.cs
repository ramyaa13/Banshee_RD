using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class LobbyWaitRoom : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    [SerializeField]
    private int MainGameSceneIndex;
    [SerializeField]
    private int MenuSceneIndex;

    private int playerCount;
    private int roomSize;

    [SerializeField]
    private int MinPlayerToStart;

    [SerializeField]
    private TextMeshProUGUI PlayerCountDisplay;
    [SerializeField]
    private TextMeshProUGUI TimerToStartDisplay;

    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    [SerializeField]
    private float MaxWaitTime;
    [SerializeField]
    private float MaxFullGameWaitTime;

    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI[] PlayerNameTexts;
    public GameObject[] UI_Slots;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        fullGameTimer = MaxFullGameWaitTime;
        notFullGameTimer = MaxWaitTime;
        timerToStartGame = MaxWaitTime;
        RoomNameText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.CurrentRoom.MaxPlayers == 4)
        {
            MinPlayerToStart = 4;
            Data.instance.SetRoomMaxPlayers(MinPlayerToStart);
            for (int i = 0; i < MinPlayerToStart; i++)
            {
                UI_Slots[i].SetActive(true);
            }
        }
        else if (PhotonNetwork.CurrentRoom.MaxPlayers == 3)
        {
            MinPlayerToStart = 3;
            Data.instance.SetRoomMaxPlayers(MinPlayerToStart);
            for (int i = 0; i < MinPlayerToStart; i++)
            {
                UI_Slots[i].SetActive(true);
            }
        }
        else
        {
            MinPlayerToStart = 2;
            Data.instance.SetRoomMaxPlayers(MinPlayerToStart);
            for (int i = 0; i < MinPlayerToStart; i++)
            {
                UI_Slots[i].SetActive(true);
            }
        }

        PlayerCountUpdate();
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        PlayerCountDisplay.text = playerCount + " : " + roomSize;
        Refresh();

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if(playerCount >= MinPlayerToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToStart = false;
            readyToCountDown = false;
        }
    }
    public void Refresh()
    {
        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.UserId descending select player).ToList();

        int i = 0;
        foreach (var player in sortedPlayerList)
        {
            PlayerNameTexts[i].text = player.NickName;
            i++;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();

        if(PhotonNetwork.IsMasterClient)
        {
            Data.instance.isPlayerMasterClient = true;
            photonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float TimeIn)
    {
        timerToStartGame = TimeIn;
        notFullGameTimer = TimeIn;
        if(TimeIn < fullGameTimer)
        {
            fullGameTimer = TimeIn;
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    private void WaitingForMorePlayers()
    {
        if(playerCount <= 1)
        {
            ResetTimer();
        }
        if(readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        TimerToStartDisplay.text = tempTimer;

        if(timerToStartGame <=0f)
        {
            if (startingGame)
                return;

            StartGame();

        }
    }

    private void ResetTimer()
    {
        fullGameTimer = MaxFullGameWaitTime;
        notFullGameTimer = MaxWaitTime;
        timerToStartGame = MaxWaitTime;
    }

    public void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(MainGameSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(MenuSceneIndex);
    }
}
