using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_playerSpawnPonits;

  
    public List<Transform> PlayerSpawnPoints { get { return m_playerSpawnPonits; } }

    [HideInInspector]
    public List<Vector2> storedPositions; // List to store Vector2 positions

    // Path to store the JSON file
    private string filePath;

    private void Awake()
    {
       
    }

    // Method to store positions and save to JSON
    public void StorePositions()
    {
        filePath = Path.Combine(Application.dataPath, "Resources/positions01.json");
        EnsureFileExists();

        storedPositions = new List<Vector2>();
        foreach (var t in PlayerSpawnPoints)
        {
            if (t != null)
            {
                storedPositions.Add(new Vector2(t.position.x, t.position.y));
            }
        }
        SaveToJson();
        Debug.Log("Positions stored and saved to JSON!");
    }

    // Method to save the positions to a JSON file
    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(new Vector2ListWrapper { vector2List = storedPositions });
        Debug.Log("path "+filePath  +"    "+json);

        File.WriteAllText(filePath, json);
    }

    // Method to ensure the JSON file exists
    private void EnsureFileExists()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("JSON file not found. Creating a new empty JSON file.");
            Vector2ListWrapper emptyWrapper = new Vector2ListWrapper { vector2List = new List<Vector2>() };
            string emptyJson = JsonUtility.ToJson(emptyWrapper);
            File.WriteAllText(filePath, emptyJson);
        }
    }
}
