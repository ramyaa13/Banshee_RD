using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadPositionsFromJson : MonoBehaviour
{
    private string filePath;
    private List<Vector2> loadedPositions; // List to store loaded Vector2 positions

    // Start is called before the first frame update
    void Start()
    {
        print("ldksfsnhdfkln klwfen");
        LoadFromJson();
        if (loadedPositions != null)
            print("Data loded");
        else
            print("Data doesn't loded");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to load the positions from the JSON file
    private void LoadFromJson()
    {
        filePath = "positions01";
        print("flie at " + filePath);
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        if (jsonFile != null)
        {
            Vector2ListWrapper wrapper = JsonUtility.FromJson<Vector2ListWrapper>(jsonFile.text);
            loadedPositions = wrapper.vector2List;
            Debug.Log("Positions loaded from JSON!");
        }
        else
        {
            Debug.Log("JSON file not found in Resources!");
        }
    }
}
