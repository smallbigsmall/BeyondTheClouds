using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudFadeOut : MonoBehaviour
{
    //지도랑 플랫폼에 동일하게 사용

    [SerializeField] GameObject Clouds;
    public int day;
    

    void Start()
    {
        
        
        //이 부분은 실제맵에서만 작동하게 바꾸기
        //if (day > 0 && day - 1 < Clouds.transform.childCount) {
        //    FadeOutCloud();
        //}
    }

    public void initCloud(int day) {
        this.day = day;
        for (int i = 0; i < day - 1; i++)
        { //2일차부터 구름 비활성화됨
            if (i < Clouds.transform.childCount)
                Clouds.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void FadeOutCloud() {
        GameObject tomorrowCloud = Clouds.transform.GetChild(day-1).gameObject;
        Tilemap cloudTilemap = tomorrowCloud.GetComponent<Tilemap>();
        StartCoroutine(FadeOut(cloudTilemap));
    }

    IEnumerator FadeOut(Tilemap cloud) {
        if (cloud.color.a > 0)
        {
            yield return new WaitForSeconds(0.05f);
            cloud.color = new Color(cloud.color.r, cloud.color.g, cloud.color.b, cloud.color.a - 0.05f);
            StartCoroutine(FadeOut(cloud));
        }
        else {
            cloud.gameObject.SetActive(false);
            cloud.color = new Color(cloud.color.r, cloud.color.g, cloud.color.b, 1);
        }
    }
}
