using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_rainCout : MonoBehaviour
{
    private int life = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain")) {
            life -= 1;
            if (life == 0) Destroy(gameObject);
        }
    }
}
