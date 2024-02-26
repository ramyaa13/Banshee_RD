using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    //[SerializeField]
    //private GameObject quickStartButton;

    //[SerializeField]
    //private GameObject quickCancelButton;

    [SerializeField]
    private int roomMaxPlayer =  4;

    //Menu Manager
    [SerializeField]
    private TMP_InputField UserNameInput;
    [SerializeField]
    private TMP_InputField JoinRoomInput;
    [SerializeField]
    private TMP_InputField CreateRoomInput;

    [SerializeField]
    private TextMeshProUGUI ErrorText;

    [SerializeField]
    private GameObject UserNamePanel;

    [SerializeField]
    private GameObject LoginButtonObj;
    [SerializeField]
    private GameObject CreateButtonObj;
    [SerializeField]
    private GameObject JoinButtonObj;

    [SerializeField]
    private GameObject RoomPanel;

    [SerializeField]
    private GameObject CreateRoomPanel;

    [SerializeField]
    private GameObject JoinRoomPanel;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Mater");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        // quickStartButton.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected To Lobby");
        UserNamePanel.SetActive(true);
    }

    //Join Random Room
    public void QuickJoinRandomRoom()
    {
        //quickStartButton.SetActive(false);
        //quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Join Room");
        CreateRandomRoom();
    }

    public void CreateRandomRoom()
    {
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomMaxPlayer };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Create Room, trying Again");
        CreateRandomRoom();
    }

    public void QuickCancel()
    {
        //quickStartButton.SetActive(true);
        //quickCancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void LoginCheck()
    {
        if (UserNameInput.text.Length >= 2)
        {
            LoginButtonObj.SetActive(true);
        }
        else
        {
            ErrorText.text = "please enter valid user name to login";
        }
    }

    public void Login()
    {
        RoomPanel.SetActive(true);
        PhotonNetwork.NickName = UserNameInput.text;
        Debug.Log(UserNameInput.text);
        ErrorText.text = "Login Sucess";
    }

    public void CreateRoomCheck()
    {
        if (CreateRoomInput.text.Length >= 2)
        {
            CreateButtonObj.SetActive(true);
        }
        else
        {
            ErrorText.text = "please enter valid room name to create";
        }
        
    }

    public void CreateRoom()
    {
            RoomOptions RO = new RoomOptions();
            RO.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(CreateRoomInput.text, RO, null);
    }

    public void JoinRoomCheck()
    {
        if (JoinRoomInput.text.Length >= 2)
        {
            JoinButtonObj.SetActive(true);
        }
        else
        {
            ErrorText.text = "please enter valid room name to Join";
        }

    }

    public void JoinRoom()
    {
        RoomOptions RO = new RoomOptions();
        RO.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, RO, TypedLobby.Default);
    }

  

    public void CreateRoomButton()
    {
        RoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        CreateRoomPanel.SetActive(true);

    }

    public void JoinRoomButton()
    {
        RoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(true);
        CreateRoomPanel.SetActive(false);
    }

    public void BackButton()
    {
        RoomPanel.SetActive(true);
        JoinRoomPanel.SetActive(false);
        CreateRoomPanel.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
