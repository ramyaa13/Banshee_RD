using UnityEngine;
using UnityEngine.Tilemaps;
using GUPS.AntiCheat.Protected;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;

    public Tilemap tilemap; // Reference to the Tilemap component
    public ProtectedInt32 gridSizeX = 5;
    public ProtectedInt32 gridSizeY = 5;
    public GameObject playerPrefab; // Reference to the player prefab

    private GameObject playerInstance;
    private BoundsInt tilemapBounds;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is not set!");
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab reference is not set!");
            return;
        }

        tilemapBounds = tilemap.cellBounds;
        SpawnCharacter();
    }

    void SpawnCharacter()
    {
        int randomX = Random.Range(tilemapBounds.x, tilemapBounds.x + tilemapBounds.size.x);
        int randomY = Random.Range(tilemapBounds.y, tilemapBounds.y + tilemapBounds.size.y);

        Vector3 spawnPosition = tilemap.GetCellCenterWorld(new Vector3Int(randomX, randomY, 0));
        spawnPosition.z = 0; // Ensure the character is at the same z-coordinate as the tilemap

        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // Debug statements
        Debug.Log("Player Spawned at: " + spawnPosition);
        Debug.Log("Player Instance: " + playerInstance);

            Camera.main.GetComponent<CameraFollow>().target = playerInstance.transform;

    }
    

    void Update()
    {
        if (playerInstance != null)
        {
            

            // Check if the player is grounded based on the tilemap
            CheckGrounded();
          
        }
    }

    

    void CheckGrounded()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(playerInstance.transform.position);
        isGrounded = tilemap.HasTile(cellPosition);
    }
}
