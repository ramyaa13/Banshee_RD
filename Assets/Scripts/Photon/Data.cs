using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;
    public bool isPlayerMasterClient;

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
}
