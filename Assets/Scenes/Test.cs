using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUPS.AntiCheat.Protected;

public class Test : MonoBehaviour
{
    public ProtectedFloat Mspeed = 6f;
    float damage = 1f;
    public ProtectedBool cantravel;

    private void Start()
    {
        cantravel = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cantravel = !cantravel;
            Mspeed -= damage;
        }

        if (cantravel)
        {
            transform.Translate(Vector2.right * Mspeed * Time.deltaTime);
        }

    }
}
