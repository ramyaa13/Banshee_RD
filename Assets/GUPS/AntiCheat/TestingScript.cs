using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GUPS.AntiCheat.Protected;
using System;

public class TestingScript : MonoBehaviour
{

    private ProtectedInt32 score;

    [SerializeField]
     private TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        score += 10;
        RefreshScore();
    }

      public void RefreshScore()
        {
            // Update the score text.
             scoreText.text = "Score : "+score;

               // Get the private "fakeValue" field from the protected score via reflection.
            int honeypotScore = (Int32)this.score.GetType().GetField("fakeValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this.score);

            // Update the honeypot score text.
            // this.honeypotScoreText.text = honeypotScore.ToString();

            // Update the honeypot score text.

            // // Get the private "fakeValue" field from the protected score via reflection.
            // int honeypotScore = (Int32)this.score.GetType().GetField("fakeValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this.score);

            // // Update the honeypot score text.
            // this.honeypotScoreText.text = honeypotScore.ToString();

            // Reset the score (only for the demonstration purposes, you will not need this).
            this.score = this.score.Value;
             scoreText.text = "Score : "+score;
        }


}
