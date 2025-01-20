using UnityEngine;
using Cinemachine;
using System.Drawing;
using Color = UnityEngine.Color;
using Photon.Pun;
using System.Collections.Generic;
using GUPS.AntiCheat.Protected;


public class ZoomCamera : MonoBehaviourPunCallbacks
{
    //public Transform[] players; // Array of player Transforms
    [SerializeField] ProtectedFloat minZoom = 5f; // Minimum orthographic size
    [SerializeField] ProtectedFloat maxZoom = 10f; // Maximum orthographic size
    [SerializeField] ProtectedFloat zoomLerpSpeed = 5f; // Speed of zooming transition

    private CinemachineVirtualCamera virtualCamera;

    public float aspect;

    private Bounds playersBounds;

    private List<Transform> players = new List<Transform>(); // List to hold player Transforms

    int frmCounter = 0;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        InvokeRepeating("FindPlayers", 2,1);
    }



    void Update()
    {
        if (Gamemanager.instance.enemyLeft)
        {
            this.gameObject.SetActive(false);
        }

        if (players.Count == 0)
            return;

        // Calculate the bounds of all players
        if(!Gamemanager.instance.enemyLeft)
            playersBounds = CalculatePlayersBounds();

        // Calculate the aspect ratio of the players bounds
        if (playersBounds.extents.x > playersBounds.extents.y)
            aspect = playersBounds.extents.x;
        else
            aspect = playersBounds.extents.y;

        aspect += 2;

        // Calculate the target orthographic size based on the aspect ratio
        float targetSize = Mathf.Clamp(aspect, minZoom, maxZoom);

        // Smoothly interpolate between the current and target orthographic size
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetSize, Time.deltaTime * zoomLerpSpeed);

        var camPosition = playersBounds.center;
        camPosition.z = -10;
        virtualCamera.transform.position = camPosition;
    }

    private Bounds CalculatePlayersBounds()
    {
        if (players.Count == 0)
            return new Bounds();

        Bounds bounds = new Bounds(players[0].position, Vector3.zero);

        foreach (Transform player in players)
        {
            bounds.Encapsulate(player.position);
        }

        return bounds;

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("New player entered room");
        FindPlayers();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Player left room");
        FindPlayers();
    }

    private void FindPlayers()
    {
        frmCounter++;

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players.Clear();
        foreach (GameObject playerObject in playerObjects)
        {
            //if (playerObject.GetPhotonView().IsMine)
            //{
                players.Add(playerObject.transform);

            //}
        }
//        Gamemanager.instance.AliveCountText.text = players.Count.ToString();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Calculate the bounds of the square
        //Bounds bounds = new Bounds(playersBounds.center,  playersBounds.size);

        // Draw the wireframe square
        Gizmos.DrawWireCube(playersBounds.center - Vector3.one, playersBounds.size + Vector3.one);
    }

}
