using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterfallMission : MonoBehaviour
{
    private int life = 50;
    [SerializeField] Tilemap waterfallSoilRenderer;
    [SerializeField] WeatherMissionManager _weatherMissionManager;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void missionSetting() {
        waterfallSoilRenderer.color = new Color(1, 1, 1, 1);
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain"))
        {
            life -= 1;
            if (life == 0)
            {
                _weatherMissionManager.MissionComplete();
                gameObject.SetActive(false);

            } else if (life == 25) {
                waterfallSoilRenderer.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}
