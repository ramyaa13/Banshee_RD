using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;
    public bool isPlayerMasterClient;

    public int maxplayers;

    public int HeadIndex;
    public int FaceIndex;
    public int TopsIndex;
    public int ShoesIndex;
    // public bool IsKnickersOn;
    // public bool IsShortsOn;
    // public bool IsMaskOn;
    public int spawnIndex;
    public Vector3 spawnPosition;
    public List<Vector3> playerSpawnPositions;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        isPlayerMasterClient = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomMaxPlayers(int mp)
    {
        maxplayers = mp;
    }

    public void CharacterCustomise(int H, int T, int si, int f)
    {
        HeadIndex = H;
        TopsIndex = T;
        ShoesIndex = si;
        FaceIndex = f;
    }
}
