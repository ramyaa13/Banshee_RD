using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Cinemachine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using static UnityEngine.EventSystems.EventTrigger;
using System.IO;

public class Gamemanager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public GameObject StartScreen;
    //public ZoomCamera zoomCamera;
    //public GameObject SceneCam;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI PingRateText;
    public TextMeshProUGUI HealthText;
    public TMP_Text message;
    public GameObject[] groundweapons;


    public static Gamemanager instance = null;

    //player respawn
    private bool b_Level;
    private float LevelTimeAmount = 5; //10
    public TextMeshProUGUI LevelTimer;
    public TextMeshProUGUI LevelCountText; 
    public GameObject LevelUI;


    public int maxLevels = 5;
    //player respawn
    private bool startRespawn;
    private float TimeAmount = 3;
    public TextMeshProUGUI spawnTimer;

    [Header ("UI Panels")]
    public GameObject respawnUI;
    public GameObject LeaderboardUI;
    public GameObject GameOverUI;
    public TextMeshProUGUI PlayerWintext;
    public GameObject HomePanel;
    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI GemCountText;
    public TextMeshProUGUI StarCountText;
    public TextMeshProUGUI CrystalCountText;

    [Space]
    public GameObject blastEffectPrefab;

    //[HideInInspector]
    internal int LevelCount = 0;
    [HideInInspector]
    public BansheePlayer LocalPlayer;
    [HideInInspector]
    public int KillCount;
    [HideInInspector]
    public int Deathcount;
    [HideInInspector]
    public int GemsCount;
    [HideInInspector]
    public int isPlayerDead;

    public TextMeshProUGUI KillCountText;
    public TextMeshProUGUI KillFeedtext;

    public TextMeshProUGUI AliveCountText;

    public Leaderboard leaderboard;

    public string[] x;
    public ObjectSpawner randomObjectSpawn;
    public Transform ObjectContainer;



    private bool GS;
    private int d;
    public CinemachineVirtualCamera CMvcam1;

    private int roomMaxPlayers;
    private List<Transform> playerSpawnPonits;

    private string filePath;
    private List<Vector2> loadedPositions; // List to store loaded Vector2 positions


    private void Awake()
    {
        //PhotonNetwork.OfflineMode = true;
        instance = this;
        //StartScreen.gameObject.SetActive(true);
        HealthText.text = "100";
        KillCountText.text = "0";
    }
    // Start is called before the first frame update
    void Start()
    {
        //SpawnPlayer();
        roomMaxPlayers = Data.instance.maxplayers - 1;
        Debug.Log(roomMaxPlayers + " room max players");
        GameStart();
        leaderboard = GetComponent<Leaderboard>();
        //randomObjectSpawn = GetComponent<ObjectSpawner>();

    }

    void GameStart()
    {
        GS = true;
        maxLevels = Globals.RoomMaxPlayers == 2 ? 3 : 5;

        LevelCount = 1;
        KillCount = 0;
        Deathcount = 0;
        GemsCount = 0;
        isPlayerDead = 0;
        LevelTimeAmount = 5f;
        d = 0;
        SetHashes();
        EnableLevel();

    }

    public void OnLoadUiScene()
    {
        PhotonNetwork.LeaveRoom();
        Globals.LoadFromGamePlay = true;
        SceneManager.LoadScene("Menu");
    }

    public void OnQuit()
    {
            print("Left room");
            //PhotonNetwork.DestroyPlayerObjects()
        OnLoadUiScene();
    }

    

    // This method is called when the local player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("player left ");
        // Find the GameObject associated with the leaving player and destroy it
        if (otherPlayer != null)
        {
            GameObject playerObject = FindPlayerObjectByPhotonPlayer(otherPlayer);
            if (playerObject != null)
            {
                PhotonNetwork.Destroy(playerObject);
            }
        }
    }

    // Helper method to find the GameObject associated with a Photon player
    private GameObject FindPlayerObjectByPhotonPlayer(Player player)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().Owner == player)
            {
                return obj;
            }
        }
        return null;
    }


    public void CameraTarget(GameObject target)
    {
        CMvcam1.Follow = target.transform;
    }

    //public void SetCinemachineConfiner(PolygonCollider2D polygonCollider2D)
    //{
    //    CMvcam1.GetComponent<CinemachineConfiner>().m_BoundingShape2D = polygonCollider2D;

    //    message.text = "Set Bounding box ";
    //}



    // Update is called once per frame
    void Update()
    {
        //Player Spawn Timer
        if (startRespawn)
        {
            StartRespawn();
        }
        //Level Loading Timer
        if (b_Level)
        {
            print("level stating =====");
            LevelStart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }

        string ping = PhotonNetwork.GetPing().ToString();
        PingRateText.text = ping;
    }

    public void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        spawnTimer.text = "Respawn in : " + TimeAmount.ToString("F0");
        if(TimeAmount <= 3)
        {
            LocalPlayer.GetComponent<HealthController>().RevivePlayer();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }

        if (TimeAmount <= 0)
        {
            HealthText.text = "100";
            respawnUI.SetActive(false);
            startRespawn = false;
            LocalPlayer.GetComponent<HealthController>().EnableInputs();

          

            LocalPlayer.GetComponent<WeaponController>().EquipWeapon();

            LocalPlayer.playerProfileData.isDead = false;
            float randomSpawn = Random.Range(-5.5f, 5.5f);

            LocalPlayer.transform.position = new Vector2(PlayerPrefab.transform.position.x + randomSpawn, PlayerPrefab.transform.position.y);
            LocalPlayer.transform.rotation = Quaternion.identity;
            isPlayerDead = 0;
            SetHashes();
        }
    }

    public void SetPlayerState(int x)
    {
        switch (x)
        {
            case 1: // player without weapons
                LocalPlayer.isIdle = true;
                LocalPlayer.isGunEquipped = false;
                LocalPlayer.isSwordEquipped = false;
                LocalPlayer.isdead = false;
                break;
            case 2: // player with Gun
                LocalPlayer.isIdle = false;
                LocalPlayer.isGunEquipped = true;
                LocalPlayer.isSwordEquipped = false;
                LocalPlayer.isdead = false;
                break;
            case 3: // player with Sword
                LocalPlayer.isIdle = false;
                LocalPlayer.isGunEquipped = false;
                LocalPlayer.isSwordEquipped = true;
                LocalPlayer.isdead = false;
                break;

            default:
                LocalPlayer.isIdle = true;
                LocalPlayer.isGunEquipped = false;
                LocalPlayer.isSwordEquipped = false;
                LocalPlayer.isdead = false;
                break;
        }
    }
    public void EnableRespawn()
    {
        LevelUI.SetActive(false);
        TimeAmount = 0.1f;
        startRespawn = true;
        //respawnUI.SetActive(true);
    }


    public void SpawnPlayer()
    {
        print("Player loaded");
        LevelUI.SetActive(false);
        float randomSpawn = Random.Range(-5.5f, 5.5f);

        Vector2 spawnPosition = new Vector2();
        List<Vector2> spawnPositionList = new List<Vector2>();

        if (PhotonNetwork.IsMasterClient)
        {
              //  Debug.LogError("Spawn MASTER PLAYER index found!");
            //spawnPosition = playerSpawnPonits[Random.Range(0, playerSpawnPonits.Count)].position;
        }
        else
        {
            string spawnIndexObj;
            //if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("SpawnPosition", out spawnIndexObj))
            //{
                //spawnPositionList = JsonUtility.FromJson<List<Vector2>>(spawnIndexObj.ToString());
                //spawnPositionList = spawnIndexObj;// JsonUtility.FromJson<List<Vector2>>(spawnIndexObj.ToString());
               /* string json = (string)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPosition"];
                Debug.LogError("Spawn index found! "+json);
                Vector2ListWrapper wrapper = JsonUtility.FromJson<Vector2ListWrapper>(json);
                List<Vector2> retrievedList = wrapper.vector2List;

                spawnPosition = retrievedList[Random.Range(0, retrievedList.Count)];
                */
               //spawnPosition = (Vector3)spawnIndexObj;
            //}
            //else
            //{
            //    Debug.LogError("Spawn index not found!");
            //}
        }
        // spawnPosition.x += randomSpawn;


        LoadFromJson();
        spawnPosition = loadedPositions[Random.Range(0, loadedPositions.Count)];
        spawnPosition = new Vector2(spawnPosition.x + randomSpawn, spawnPosition.y + 10);
        //PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(PlayerPrefab.transform.position.x * randomSpawn, PlayerPrefab.transform.position.y + 10), Quaternion.identity, 0);
        PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition, Quaternion.identity, 0);

        StartScreen.gameObject.SetActive(false);

        //SceneCam.gameObject.SetActive(false);
        
        leaderboard.SetPlayerName(LocalPlayer.PlayerName);
    }

    // Method to load the positions from the JSON file
    private void LoadFromJson()
    {
        filePath = "positions01";

        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile != null)
        {
            Vector2ListWrapper wrapper = JsonUtility.FromJson<Vector2ListWrapper>(jsonFile.text);
            loadedPositions = wrapper.vector2List;
            Debug.Log("Positions loaded from JSON!");
        }
        else
        {
            Debug.LogWarning("JSON file not found in Resources!");
        }
    }

    public void SpawnGameEssentials()
    {

        if (Data.instance.isPlayerMasterClient == true)
        {
            randomObjectSpawn.SpawnGE(LevelCount);
            
            Debug.Log("It's a master client");
        }
        else
        {
            Debug.Log("It's not a master client");
        }

    }

    public void DestroyGameEssentials()
    {

        if (Data.instance.isPlayerMasterClient == true && GS == false)
        {
            randomObjectSpawn.DestroyGE();
            Debug.Log("It's a master client");
        }
        else
        {
            Debug.Log("It's not a master client");
        }
    }

    public void LevelStart()
    {
        LevelTimeAmount -= Time.deltaTime;
        LevelTimer.text = "Level Starts in : " + LevelTimeAmount.ToString("F0");
        Debug.Log("Level Starts in : " + LevelTimeAmount.ToString("F0"));

        if (LevelTimeAmount <= 0)
        {
            print("Call this start on LevelTimeAmount 0");
            b_Level = false;
            if (GS == true)
            {
                LevelTimer.text = "Level Started!!!";

               
                SpawnPlayer();
                InvokeRepeating(nameof(IsPlayerDeadCounts), 5f, 5f);

                GS = false;
            }
            else
            {

                print("Call player dead and lobby");
                EnableRespawn();
                InvokeRepeating(nameof(IsPlayerDeadCounts), 5f, 5f);

            }
        }
    }

    public void EnableLevel()
    {
        if (LevelCount > maxLevels)
        {
            CancelInvoke("IsPlayerDeadCounts");
            DestroyGameEssentials();
            GameOver();
            //Game Over
        }
        else
        {
            CancelInvoke("IsPlayerDeadCounts");
            DestroyGameEssentials();
            //GameOver();

            SpawnGameEssentials();
            LevelUI.SetActive(true);
            LevelTimeAmount = 5f; //10f
            b_Level = true;
            //LevelCount += 1;
            print("Set next level ");
            LevelCountText.text = "Level : " + LevelCount;
        }
    }

    //GameOver
    public void GameOver()
    {
        print("======== Set Gam over ======");
        LocalPlayer.GetComponent<HealthController>().DisableInputs();
        LeaderboardUI.SetActive(true);
        GameOverUI.SetActive(true);
        leaderboard.PlayerWins();
        PlayerNameText.text = LocalPlayer.PlayerName;
    }

    public void PlayBlastEffect(Vector3 atPosition)
    {
        PhotonNetwork.Instantiate(blastEffectPrefab.name, atPosition, Quaternion.identity, 0);
    }



    //Player Wins
    public void IsPlayerWins(string player, int stars, int crstals)
    {
        PlayerWintext.text = player + " WINS THE GAME!!";
        StarCountText.text = "Stars : " + stars;
        DataManager.SetStars(DataManager.GetStars() + stars);
        CrystalCountText.text = "Crystals : " + crstals;
        GemCountText.text = "Gems : " + KillCount;
    }

    public void GRemoveSO(GameObject gameObject)
    {
        randomObjectSpawn.RemoveSO(gameObject);
    }
    public void GRemoveWO(GameObject gameObject)
    {
        randomObjectSpawn.RemoveWO(gameObject);
    }
    public void GRemoveOO(GameObject gameObject)
    {
        randomObjectSpawn.RemoveOO(gameObject);
    }

    public void GAddWO(GameObject gameObject)
    {
        randomObjectSpawn.AddWO(gameObject);
    }
    public void GAddOO(GameObject gameObject)
    {
        randomObjectSpawn.AddOO(gameObject);
    }


    public void IsPlayerDeadCounts()
    {
        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.UserId descending select player).ToList();

        int i = 0;

        int totalPlr = sortedPlayerList.Count;

        foreach (var player in sortedPlayerList)
        {
            if (player.CustomProperties["IsplayerDead"] != null)
            {
                x[i] = player.CustomProperties["IsplayerDead"].ToString();
                if (x[i] == "1")
                {
                    d++;
                    Debug.Log(d + " total player died out of "+roomMaxPlayers);
                }
                //Debug.Log(x[i] + player.NickName + i);
            }
            else
            {
                x[i] = "0";
            }
            i++;
        }

        //AliveCountText.text = (totalPlr - d).ToString();

        if (d == roomMaxPlayers)
        {
            print("======== load next level ===========");
            //next level load
            //cancel invoke
            d = 0;
            LevelCount++;
            EnableLevel();
        }
        else
        {
            d = 0;
        }

    }

    public void UpdateScore()
    {
        GemsCount += 10;
        ScoreText.text = GemsCount.ToString();
        SetHashes();
    }

    public void UpdateHealth(float x)
    {
        x = x * 100;
        LocalPlayer.playerProfileData.health = x;
        HealthText.text = x.ToString();
    }
    public void UpdateKillCount()
    {
        KillCount += 1;
        LocalPlayer.playerProfileData.kills = KillCount;
        SetHashes();
        KillCountText.text = KillCount.ToString();
    }

    public void UpdateYouKilledFeedText(string name)
    {
        KillFeedtext.text = "You Killed: " + name;
        KillFeedtext.color = Color.green;
    }
    public void UpdateDeathCount()
    {
        Deathcount += 1;
        isPlayerDead = 1;
        LocalPlayer.playerProfileData.deaths = Deathcount;
        LocalPlayer.playerProfileData.isDead = true;
        SetHashes();
    }
    public void UpdateYouGotKilledFeedText(string name)
    {
        KillFeedtext.text = "You Got Killed By: " + name;
        KillFeedtext.color = Color.red;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public void SetHashes()
    {
        try
        {
            Hashtable Hash = PhotonNetwork.LocalPlayer.CustomProperties;
            Hash["Kills"] = KillCount;
            Hash["Deaths"] = Deathcount;
            Hash["Gems"] = GemsCount;
            Hash["IsplayerDead"] = isPlayerDead;
            Hash["HairIndex"] = Data.instance.HairIndex;
            Hash["EyeIndex"] = Data.instance.EyesIndex;
            Hash["TopIndex"] = Data.instance.TopsIndex;
            Hash["ShoesIndex"] = Data.instance.ShoesIndex;
            Hash["IsKnickersOn"] = Data.instance.IsKnickersOn;
            Hash["IsShortsOn"] = Data.instance.IsShortsOn;
            Hash["IsMaskOn"] = Data.instance.IsMaskOn;
          
            PhotonNetwork.LocalPlayer.SetCustomProperties(Hash);
        }
        catch
        {
            //Do Nothing
            //(bool)p.CustomProperties["Dead"] == true
        }
    }


    internal void UpdatePlayerSpawnPoints(List<Transform> points)
    {
        playerSpawnPonits = points;

        message.text = "POINTs " + playerSpawnPonits.Count;
        print("Plr spwan positoin string " + playerSpawnPonits.Count);

        if (PhotonNetwork.IsMasterClient)
        {
            //int spawnIndex = Random.Range(0, spawnPoints.Length); // Choose a random spawn point
          //  var jsonString = JsonUtility.ToJson(playerSpawnPonits);
            //PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "SpawnIndex", jsonString } });

            List<Vector2> plrPosition = new List<Vector2>();
            foreach (var item in playerSpawnPonits)
            {
                plrPosition.Add(item.transform.position);
            }

            //Data.instance.playerSpawnPositions = playerSpawnPonits[0].position;
            Hashtable Hash = PhotonNetwork.CurrentRoom.CustomProperties;

            string json = JsonUtility.ToJson(new Vector2ListWrapper { vector2List = plrPosition });
           

            print("json string " + json);

            //string key = "SpawnPosition";
            Hash.Add("SpawnPosition", "1");
            PhotonNetwork.CurrentRoom.SetCustomProperties(Hash);
            //PhotonNetwork.CurrentRoom.SetCustomProperties
        }
    }

    public void ShowMessage(string msg)
    {
        message.text = msg;
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //foreach (DictionaryEntry entry in propertiesThatChanged)
        //{
        //    Debug.Log("Property updated: " + entry.Key + " = " + entry.Value);
        //    ShowMessage("Property updated: " + entry.Key + " = " + entry.Value);
        //}

        foreach (var item in PhotonNetwork.CurrentRoom.CustomProperties)
        {
            Debug.Log("Property updated: " + item.Key + " = " + item.Value);

        }
    }
}

// Wrapper class for serializing and deserializing the list
[System.Serializable]
public class Vector2ListWrapper
{
    public List<Vector2> vector2List;
}