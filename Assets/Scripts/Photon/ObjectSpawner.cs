using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;  

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { Gem, Sheild, SprintShoes};
    public Tilemap tilemap;
    public GameObject[] ObjectPrefabs;
    public GameObject[] WeaponPrefabs;
    public float ShieldProbablity = 0.2f;
    public float SprintShoesProbablity = 0.1f;
  
    public int MaxObjects = 20;
    public int MaxWeaponObjects = 6;
    public float ShieldLifeTime = 10f;
    public float SpawnInterval = 0.5f;

    private List<Vector3> validSpawnPoints  = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    private List<GameObject> weaponObjects = new List<GameObject>();
    public List<GameObject> OverallObjects = new List<GameObject>();
    public Transform ObjectContainer;
    public Transform WeaponContainer;


    // Start is called before the first frame update
    void Start()
    {
        GatherValidPoints();
        SpawnGameObjects();
    }
    public void SpawnGameObjects()
    {
        StartCoroutine(SpawnObjectsIfNeeded());
        StartCoroutine(SpawnWeaponObjectsIfNeeded());
    }

    public void DestroyGameObjects()
    {
        DestroySpawnObjects();
    }
    // Update is called once per frame
    void Update()
    {
        if(!tilemap.gameObject.activeInHierarchy)
        {
            //level change
        }
        //if(!isSpawning && ActiveObjectsCount() < MaxObjects)
        //{
        //    StartCoroutine(SpawnObjectsIfNeeded());
        //}
    }

    private int ActiveObjectsCount()
    {
        spawnObjects.RemoveAll(item => item == null);
        return spawnObjects.Count;
    }

    private int ActiveWeaponObjectsCount()
    {
        weaponObjects.RemoveAll(item => item == null);
        return weaponObjects.Count;
    }
    private IEnumerator SpawnObjectsIfNeeded()
    {
        while(ActiveObjectsCount() < MaxObjects)
        {
            SpawnObjects();
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
    private IEnumerator SpawnWeaponObjectsIfNeeded()
    {
        while (ActiveWeaponObjectsCount() < MaxWeaponObjects)
        {
            SpawnWeaponObjects();
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    private bool PositionHasObjects(Vector3 positionToCheck)
    {
        return OverallObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 15.0f);
    }

    private ObjectType RandomObjectType()
    {
        float RandomChoice = Random.value;
        if(RandomChoice <= ShieldProbablity) //0.5f
        {
            Debug.Log("gem" + RandomChoice);
            return ObjectType.Sheild;
        }
        if (RandomChoice <= (ShieldProbablity + SprintShoesProbablity)) //0.5+0.2
        {
            Debug.Log("Shield" + RandomChoice);
            return ObjectType.SprintShoes;
        }
        else
        {
            Debug.Log("Weapon" + RandomChoice);
            return ObjectType.Gem;
        }
    }

    private void SpawnObjects()
    {
        if (validSpawnPoints.Count == 0)
            return;

        Vector3 SpawnPositions = Vector3.zero;
        bool isValidPositionFound  = false;

        while(!isValidPositionFound && validSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPoints.Count);
            Vector3 potentialPositions = validSpawnPoints[randomIndex];
            Vector3 lestPosition = potentialPositions + Vector3.left;
            Vector3 rightPosition = potentialPositions + Vector3.right;

            if(!PositionHasObjects(lestPosition) && !PositionHasObjects(rightPosition))
            {
                SpawnPositions = potentialPositions;
                isValidPositionFound = true;
            }
            validSpawnPoints.RemoveAt(randomIndex);
        }

        if(isValidPositionFound)
        {
            ObjectType objectType = RandomObjectType();
           // int randomGunIndex = Random.Range(0, ObjectPrefabs.Length);
            //Instantiate
            GameObject gameObject = Instantiate(ObjectPrefabs[(int)objectType], SpawnPositions, Quaternion.identity);
            gameObject.transform.SetParent(ObjectContainer, false);
            spawnObjects.Add(gameObject);
            OverallObjects.Add(gameObject);
            ////Destroy Shield only after time
            //if(objectType != ObjectType.Weapon)
            //{
            //    StartCoroutine(DestroyGameObjectAfterTime(gameObject, ShieldLifeTime));
            //}

        }
    }

    private void SpawnWeaponObjects()
    {
        if (validSpawnPoints.Count == 0)
            return;

        Vector3 SpawnPositions = Vector3.zero;
        bool isValidPositionFound = false;

        while (!isValidPositionFound && validSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPoints.Count);
            Vector3 potentialPositions = validSpawnPoints[randomIndex];
            Vector3 lestPosition = potentialPositions + Vector3.left;
            Vector3 rightPosition = potentialPositions + Vector3.right;

            if (!PositionHasObjects(lestPosition) && !PositionHasObjects(rightPosition))
            {
                SpawnPositions = potentialPositions;
                isValidPositionFound = true;
            }
            validSpawnPoints.RemoveAt(randomIndex);
        }

        if (isValidPositionFound)
        {
           // ObjectType objectType = RandomObjectType();
             int randomGunIndex = Random.Range(0, WeaponPrefabs.Length);
            //Instantiate
            GameObject gameObject = Instantiate(WeaponPrefabs[randomGunIndex], SpawnPositions, Quaternion.identity);
            gameObject.transform.SetParent(WeaponContainer, false);
            weaponObjects.Add(gameObject);
            OverallObjects.Add(gameObject);
        }
    }
    private void DestroySpawnObjects()
    {
        foreach(GameObject gameObject in spawnObjects)
        {
            if(gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        foreach (GameObject gameObject in weaponObjects)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        foreach (GameObject gameObject in OverallObjects)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        spawnObjects.Clear();
        weaponObjects.Clear();
        OverallObjects.Clear();
    }
    private IEnumerator DestroyGameObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        if(gameObject)
        {
            spawnObjects.Remove(gameObject);
            validSpawnPoints.Add(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    
    private void GatherValidPoints()
    {
        validSpawnPoints.Clear();
        BoundsInt boundsInt = tilemap.cellBounds;
        Debug.Log(boundsInt + "Bounds Int");
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for(int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                    TileBase tile = allTiles[x + y * boundsInt.size.x];
                if(tile != null)
                {
                    Vector3 place = start + new Vector3(x + 3f, y + 5f, 0);
                    validSpawnPoints.Add(place);
                }
            }
        }

        Debug.Log(validSpawnPoints.Count + "valid spawn points");

    }
}
