
using UnityEngine;
using Photon.Pun;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int LobbyGameSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        StartGame();
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient) 
        { 
            PhotonNetwork.LoadLevel(LobbyGameSceneIndex);
        }
    }
}
