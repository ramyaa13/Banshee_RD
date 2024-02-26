using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MenuManager : MonoBehaviourPunCallbacks
{
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
    private GameObject RoomPanel;

    



    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Mater");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected To Lobby");
        UserNamePanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        //play game scene
        PhotonNetwork.LoadLevel(1);
    }


    public void Login()
    {
        if(UserNameInput.text.Length >= 2)
        {
            RoomPanel.SetActive(true);
            PhotonNetwork.NickName = UserNameInput.text;
            Debug.Log(UserNameInput.text);
            ErrorText.text = "Login Sucess";
        }
        else
        {
            ErrorText.text = "please enter valid user name to login";
        }
    }

    public void CreateRoom()
    {
        RoomOptions RO = new RoomOptions();
        RO.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(CreateRoomInput.text, RO, null);
    }

    public void JoinRoom()
    {
        RoomOptions RO = new RoomOptions();
        RO.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, RO, TypedLobby.Default);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
