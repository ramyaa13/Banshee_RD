using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class RandomWeaponSpawn : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public GameObject[] gunPrefabs;

    public int numberOfGuns = 6;
    public Transform ObjectContainer;
    private List<GameObject> spawnedGuns = new List<GameObject>();
    public PhotonView photonView;

    void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is not set!");
            return;
        }

        if (gunPrefabs == null || gunPrefabs.Length == 0)
        {
            Debug.LogError("Gun Prefab reference is not set!");
            return;
        }
        photonView = GetComponent<PhotonView>();
        // SpawnObjects();
    }

    public void SpawnObjects()
    {
        photonView.RPC("SpawnGameObjects", RpcTarget.Others);
    }

    public void DestroyObjects()
    {
        photonView.RPC("DestroyGameObjects", RpcTarget.Others);
    }

    [PunRPC]
    public void SpawnGameObjects()
    {
        for (int i = 0; i < numberOfGuns; i++)
        {
            Vector3 spawnPosition = GetRandomGunSpawnPosition();
            int randomGunIndex = Random.Range(0, gunPrefabs.Length);

            GameObject gunPrefab = gunPrefabs[randomGunIndex];
            GameObject gunInstance = PhotonNetwork.Instantiate(gunPrefab.name, spawnPosition, Quaternion.identity);
            gunInstance.transform.SetParent(ObjectContainer, false);
            spawnedGuns.Add(gunInstance);
        }
    }

    [PunRPC]
    public void DestroyGameObjects()
    {
        if (ObjectContainer.transform.childCount != 0 && spawnedGuns.Count != 0)
        {
            for (var i = ObjectContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(ObjectContainer.transform.GetChild(i));
            }

            for (int i = 0; i < spawnedGuns.Count; i++)
            {
                spawnedGuns.Remove(spawnedGuns[i]);
            }
        }
        else
        {
            //nothing to destroy
        }

    }



    Vector3 GetRandomGunSpawnPosition()
    {
        int maxAttempts = 100;
        int currentAttempt = 0;

        while (currentAttempt < maxAttempts)
        {
            int randomX = Random.Range(0, gridSizeX);
            int randomY = Random.Range(0, gridSizeY);

            float zAxis = -0.01f;

            Vector3 spawnPosition = tilemap.GetCellCenterWorld(new Vector3Int(randomX, randomY, 0));
            spawnPosition.z = zAxis; // Ensure the gun is at the same z-coordinate as the tilemap

            // Check proximity to existing guns
            bool isFarEnough = true;
            foreach (var gun in spawnedGuns)
            {
                float distance = Vector3.Distance(gun.transform.position, spawnPosition);
                if (distance < 40.0f) // Adjust the distance as needed
                {
                    isFarEnough = false;
                    break;
                }
            }

            // Check if the spawn position is visible in the camera
            if (isFarEnough && IsSpawnPositionVisible(spawnPosition))
            {
                // Check if the spawn position is not too close to other guns
                bool isUniquePosition = true;
                foreach (var otherSpawnedGun in spawnedGuns)
                {
                    float distanceToOtherGun = Vector3.Distance(otherSpawnedGun.transform.position, spawnPosition);
                    if (distanceToOtherGun < 1.0f) // Adjust the distance as needed
                    {
                        isUniquePosition = false;
                        break;
                    }
                }

                if (isUniquePosition)
                    return spawnPosition;
            }

            currentAttempt++;
        }

        Debug.LogWarning("Could not find a suitable position for the gun after multiple attempts.");
        return Vector3.zero;
    }

    bool IsSpawnPositionVisible(Vector3 spawnPosition)
    {
        Camera mainCamera = Camera.main;

        // Check if the spawn position is within the camera's view
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(spawnPosition);
        return (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1);
    }
}
