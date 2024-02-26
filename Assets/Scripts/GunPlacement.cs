using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GunPlacement : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject[] guns;
    private List<GameObject> gunInstances = new List<GameObject>();

    void Start()
    {
        PlaceGuns();
    }

    void PlaceGuns()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            // Randomly find a position that does not have a gun
            Vector3Int randomPosition;
            do
            {
                randomPosition = new Vector3Int(
                    Random.Range(tilemap.cellBounds.x, tilemap.cellBounds.xMax),
                    Random.Range(tilemap.cellBounds.y, tilemap.cellBounds.yMax),
                    0
                );
            } while (tilemap.GetTile(randomPosition) != null);

            // Set the z.position to -0.01
            Vector3 gunPosition = tilemap.GetCellCenterWorld(randomPosition);
            gunPosition.z = -0.01f;

            // Instantiate the gun at the calculated position
            GameObject instantiatedGun = Instantiate(guns[i], gunPosition, Quaternion.identity);
            gunInstances.Add(instantiatedGun);

            // Set the tile at the random position to represent the gun
            tilemap.SetTile(randomPosition, instantiatedGun.GetComponent<Tile>());
        }
    }



public GameObject GetRandomGun()
    {
        if (gunInstances.Count == 0)
            return null;

        int randomIndex = Random.Range(0, gunInstances.Count);
        GameObject randomGun = gunInstances[randomIndex];
        gunInstances.RemoveAt(randomIndex);
        return randomGun;
    }   

}
