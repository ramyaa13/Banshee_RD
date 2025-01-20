using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using GUPS.AntiCheat.Protected;



public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    //[SerializeField]
    //private GameObject quickStartButton;

    //[SerializeField]
    //private GameObject quickCancelButton;

    #region Public Variables

    [SerializeField]
    public int roomMaxPlayer =  4;
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
    private Button PlayButton;

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

    [SerializeField]
    private GameObject CharacterCustomPanel;
    [SerializeField]
    private GameObject CharacterCustomAvatar;

    [SerializeField]
    private GameObject GameModePanel;
    [SerializeField]
    private GameObject MenuPanel;

    [SerializeField] TMP_Text crystalText;
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text starText;

    #endregion Public Variables

    private int stars;
    public int Stars { get { return stars; }
        set {
            stars = value;
            starText.text = stars.ToString();
                 
        } }

    private int crystal;
    public int Crystal
    {
        get { return crystal; }
        set
        {
            crystal = value;
            crystalText.text = crystal.ToString();
        }
    }

    private int coins;
    public int Coins
    {
        get { return coins; }
        set
        {
            coins = value;
            coinText.text = coins.ToString();
        }
    }




    public bool IsTesting;


    private CharacterCustomisation CC;
    private void Awake()
    {
        if(PhotonNetwork.IsConnected)
        {
            if (!Globals.LoadFromGamePlay)
            {
                PhotonNetwork.ConnectUsingSettings();

                print("Is connection ready  " + PhotonNetwork.IsConnectedAndReady);
                print("IN lobby " + PhotonNetwork.IsConnectedAndReady);
            
            }
        }
        else
            PhotonNetwork.ConnectUsingSettings();

        Coins = DataManager.GetCoins();
        Crystal = DataManager.GetCrystals();
        Stars = DataManager.GetStars();
    }


    private void Update()
    {
        if (IsTesting)
        {
            if (UserNamePanel.activeSelf && Input.GetKeyDown(KeyCode.L))
            {
                if (UserNameInput.text.Length < 2)
                {
                    UserNameInput.text = "FF";
                }
                Login();
            }

            if (MenuPanel.activeSelf && Input.GetKeyDown(KeyCode.G))
            {
                GameModeButton();
            }

            if (GameModePanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                    SetRoomMaxPlayer(2);
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    SetRoomMaxPlayer(3);
            }
            if (RoomPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (CreateRoomInput.text.Length < 2)
                    {
                        CreateRoomInput.text = "rr";
                    }
                    CreateRoom();
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    if (JoinRoomInput.text.Length < 2)
                    {
                        JoinRoomInput.text = "rr";
                    }
                    JoinRoom();
                }
            }
        }
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
        //PlayButton.interactable = true;

        if (Globals.LoadFromGamePlay)
        {
            MenuPanel.SetActive(true);
            Globals.LoadFromGamePlay = false;
        }
        else
        {
            UserNamePanel.SetActive(true);
            CC = GetComponent<CharacterCustomisation>();
        }
    }

    public void OnPlay()
    {
        //if(PhotonNetwork.InLobby)
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
    public void SetRoomMaxPlayer(int x)
    {
        roomMaxPlayer = x;
        Globals.RoomMaxPlayers = roomMaxPlayer;
        GameModePanel.SetActive(false);
        RoomPanel.SetActive(true);
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
        // RoomPanel.SetActive(true);
        UserNamePanel.SetActive(false);
        MenuPanel.SetActive(true);
        
        PhotonNetwork.NickName = UserNameInput.text;
        Debug.Log(UserNameInput.text);
        ErrorText.text = "Login Success";
    }

    public void GameModeButton()
    {
        MenuPanel.SetActive(false);
        GameModePanel.SetActive(true);
    }
    public void CharactercustomButton()
    {
        GameModePanel.SetActive(false);
        MenuPanel.SetActive(false);
        CharacterCustomPanel.SetActive(true);
        CharacterCustomAvatar.SetActive(true);
    }
    public void CharacterCustomDone()
    {
        CharacterCustomPanel.SetActive(false);
        CharacterCustomAvatar.SetActive(false);
        Data.instance.CharacterCustomise(CC.HeadIndex, CC.TopsIndex, CC.ShoesIndex, CC.FaceIndex);
        MenuPanel.SetActive(true);
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
            RO.MaxPlayers = roomMaxPlayer;
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
        RO.MaxPlayers = roomMaxPlayer;
        print("room max plr on join room : " + RO.MaxPlayers);
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
