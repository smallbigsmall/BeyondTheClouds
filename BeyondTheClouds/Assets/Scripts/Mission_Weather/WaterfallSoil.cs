using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterfallSoil : MonoBehaviour
{
    private int life = 50;
    [SerializeField] Tilemap waterfallSoilRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain"))
        {
            life -= 1;
            Debug.Log("waterfall Rain detect");
            if (life == 0)
            {
                GetComponentInParent<WaterfallMission>().MissionComplete();
                gameObject.SetActive(false);

            }
            else if (life == 25)
            {
                waterfallSoilRenderer.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}
