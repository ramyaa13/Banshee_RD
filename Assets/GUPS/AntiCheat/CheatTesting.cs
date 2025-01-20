using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Reflection;
using GUPS.AntiCheat.Protected;


public class CheatTesting : MonoBehaviour
{
    public TestingScript testingScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateByCheating()
    {
           // Get the private "score" field from the score manager via reflection.
            ProtectedInt32 protectedScore = (ProtectedInt32) this.testingScript.GetType().GetField("score", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this.testingScript);
            print(protectedScore);
            protectedScore = 100;
            
             FieldInfo fieldInfo = typeof(int).GetField("fakeValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Modify the honeypot score to 1000.
            object boxedProtectedScore = protectedScore;

            // fieldInfo.SetValue(boxedProtectedScore, 1000);

            protectedScore = 1000;//(int) boxedProtectedScore;

            // Set the private "score" field from the score manager via reflection to the modified protected score.
            this.testingScript.GetType().GetField("score", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this.testingScript, protectedScore);

            // Refresh the score.
            this.testingScript.RefreshScore();

    }
}
