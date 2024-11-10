using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CloudFadeOut : MonoBehaviour
{
    //������ �÷����� �����ϰ� ���

    [SerializeField] GameObject Clouds;
    public int day;
    

    void Start()
    {
        for (int i = 0; i < day - 1; i++) { //2�������� ���� ��Ȱ��ȭ��
            if(i < Clouds.transform.childCount)
                Clouds.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        //�� �κ��� �����ʿ����� �۵��ϰ� �ٲٱ�
        //if (day > 0 && day - 1 < Clouds.transform.childCount) {
        //    FadeOutCloud();
        //}
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