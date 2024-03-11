using Photon.Pun;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviourPunCallbacks
{
    //public Tilemap tilemap;
    public GameObject[] gunPrefabs;
    public GameObject[] backgroundPrefabs;
    public GameObject playerPrefab;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnObjects();
            }
        }
        //else
        //{
        //    SpawnObjects();
        //}
    }

    private void SpawnObjects()
    {
        SpawnBackground();

        // Check if the player prefab is spawned
        if (PhotonNetwork.InstantiateSceneObject(playerPrefab.name, Vector3.zero, Quaternion.identity) != null)
        {
            SpawnGunsNearPlayer();
        }
        else
        {
            Debug.LogWarning("Player prefab not spawned!");
        }
    }

    private void SpawnBackground()
    {
        // Randomly choose a background prefab
        GameObject selectedBackgroundPrefab = backgroundPrefabs[Random.Range(0, backgroundPrefabs.Length)];

        // Calculate the world position for the background
        Vector3 worldPosition = transform.position;

        // Instantiate the background prefab at the calculated position
        PhotonNetwork.Instantiate(selectedBackgroundPrefab.name, worldPosition, Quaternion.identity);
    }

    private void SpawnGunsNearPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Adjust the range and number of guns as needed
            int numberOfGuns = 3;
            float spawnRadius = 5f;

            for (int i = 0; i < numberOfGuns; i++)
            {
                // Randomly choose a gun prefab
                GameObject selectedGunPrefab = gunPrefabs[Random.Range(0, gunPrefabs.Length)];

                // Calculate a random position near the player within the spawnRadius
                Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPosition = player.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

                // Instantiate the gun prefab at the calculated position
                PhotonNetwork.Instantiate(selectedGunPrefab.name, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Player not found to spawn guns near.");
        }
    }
}
