using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatStroke : MonoBehaviour
{
    private WeatherMissionManager _weatherMissionManager;
    private Coroutine countdownCoroutine;
    private int shadowCount = 0;
    [SerializeField] GameObject heatStrokePopup;
    private Color color;
    [SerializeField] SpriteRenderer npcSpriteRenderer;
    private bool isTodayMission = false;

    public void MakeNPCHeatStroke(WeatherMissionManager script) {
        _weatherMissionManager = script;
        ColorUtility.TryParseHtmlString("#FF8E88", out color);
        npcSpriteRenderer.color = color;
        heatStrokePopup.SetActive(true);
        isTodayMission = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shadow") && isTodayMission) {
            if (shadowCount == 0)
            {
                shadowCount++;
                countdownCoroutine = StartCoroutine(CountDownShadow());
            }
            else {
                shadowCount++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shadow") && isTodayMission) {
            shadowCount--;
            if (shadowCount == 0) {
                StopCoroutine(countdownCoroutine);
            }
        }
    }

    private IEnumerator CountDownShadow() {
        yield return new WaitForSeconds(3);
        ColorUtility.TryParseHtmlString("#FFFFFF", out color);
        npcSpriteRenderer.color = color;
        heatStrokePopup.SetActive(false);
        _weatherMissionManager.MissionComplete();
        transform.parent.GetComponent<MissionSettingWithQuest>().CompleteQuestUI();
    }
}
