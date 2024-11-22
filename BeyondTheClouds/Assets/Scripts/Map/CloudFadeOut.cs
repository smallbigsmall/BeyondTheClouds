using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudFadeOut : MonoBehaviour
{
    //지도랑 플랫폼에 동일하게 사용

    [SerializeField] GameObject Clouds;
    [SerializeField] WeatherMissionManager _weatherMissionManager;
    private int day;
    public bool isMap;

    public void initCloud(int day) {
        this.day = day;
        int value = 2;
        if (isMap) value = 1;

        for (int i = 0; i < day - value; i++)
        { //3일차부터 시작시 구름 비활성화됨(2일차는 페이드아웃으로 비활성화)
            if (i < Clouds.transform.childCount)
                Clouds.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void initCloudMap(int day)
    {
        this.day = day;
        for (int i = 0; i < day - 1; i++)
        { //2일차부터 시작시 구름 비활성화됨
            if (i < Clouds.transform.childCount)
                Clouds.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void FadeOutCloud(int day) {
        if (day > 1 && day - 1 < Clouds.transform.childCount)
        {
            GameObject todayCloud = Clouds.transform.GetChild(day - 2).gameObject;
            Tilemap cloudTilemap = todayCloud.GetComponent<Tilemap>();
            StartCoroutine(Delay(cloudTilemap));
        }
        else {
            _weatherMissionManager.StartMissoinSetting(0, day);
        }
    }

    IEnumerator Delay(Tilemap cloud) {
        yield return new WaitForSeconds(3);
        StartCoroutine(FadeOut(cloud));
    }

    IEnumerator FadeOut(Tilemap cloud) {
        if (cloud.color.a > 0)
        {
            yield return new WaitForSeconds(0.05f);
            cloud.color = new Color(cloud.color.r, cloud.color.g, cloud.color.b, cloud.color.a - 0.05f);
            StartCoroutine(FadeOut(cloud));
        }
        else {
            //여기에서 카메라 다시 원래대로 돌아간 뒤에 아래 실행되어야함
            GameObject.FindWithTag("MainCamera").GetComponent<CameraController>().FollowPlayer();
            cloud.gameObject.SetActive(false);
            cloud.color = new Color(cloud.color.r, cloud.color.g, cloud.color.b, 1);
            _weatherMissionManager.StartMissoinSetting(0, day);
        }
    }
}
