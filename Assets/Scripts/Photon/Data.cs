using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;
    public bool isPlayerMasterClient;

    public int maxplayers;

    public int HairIndex;
    public int EyesIndex;
    public int TopsIndex;
    public bool IsKnickersOn;
    public bool IsShortsOn;
    public bool IsMaskOn;
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

    public void CharacterCustomise(int H, int E, int T, bool K, bool S, bool M)
    {
        HairIndex = H;
        EyesIndex = E;
        TopsIndex = T;
        IsKnickersOn = K;
        IsShortsOn = S;
        IsMaskOn = M;
    }
}
