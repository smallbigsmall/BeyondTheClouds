using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_rainCout : MonoBehaviour
{
    private int life = 5;
    private float animSpeed = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain")) {
            life -= 1;
            if (life == 0) {
                transform.parent.parent.gameObject.GetComponent<FireRandomInit>().FireComplete();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        life = 3;
        animSpeed = Random.Range(0.7f, 1f);
        gameObject.GetComponent<Animator>().speed = animSpeed;
    }
}
