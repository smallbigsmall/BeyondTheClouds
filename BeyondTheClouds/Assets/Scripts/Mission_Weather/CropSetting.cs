using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSetting : MonoBehaviour
{
    enum CropType { Beetroot, Cabbage, Carrot, Cauliflower, Parsnip, Potato, Pumpkin, Radish }

    private FarmSetting _farmSetting;
    [SerializeField] Sprite[] cropSpriteList;
    private Color color;
    private SpriteRenderer cropSpriteRenderer;
    [SerializeField] CropType cropType;
    private int life = 15;
    private int lifeForShadow = 0;
    private bool isDroughtCrop = false;
    private bool isTodayMission = false;

    void Start()
    {
        _farmSetting = gameObject.transform.parent.parent.gameObject.GetComponent<FarmSetting>();
        cropSpriteRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        cropSpriteRenderer.sprite = cropSpriteList[((int)cropType)];
    }

    public void CropColorSetting(string colorCode, bool isDrought) 
    {
        isDroughtCrop = isDrought;
        ColorUtility.TryParseHtmlString(colorCode, out color);
        cropSpriteRenderer.color = color;
        isTodayMission = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain") && isDroughtCrop && isTodayMission)
        {
            life -= 1;
            if (life == 0)
            {
                ColorUtility.TryParseHtmlString("#FFFFFF", out color); //white
                cropSpriteRenderer.color = color;
                _farmSetting.countCropComplete();
            }
        }
        else {
            if (collision.gameObject.CompareTag("Shadow") && !isDroughtCrop && isTodayMission)
            {
                if(lifeForShadow == 0) _farmSetting.countCropOverwatering();
                lifeForShadow++;
                ColorUtility.TryParseHtmlString("#55D9FF", out color); //blue
                cropSpriteRenderer.color = color;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shadow") && !isDroughtCrop && isTodayMission)
        {
            lifeForShadow--;
            if (lifeForShadow == 0) {
                ColorUtility.TryParseHtmlString("#FFFFFF", out color); //white
                cropSpriteRenderer.color = color;
                _farmSetting.countCropComplete();
            }
        }
    }

    public void MissionCleared() {
        isTodayMission = false;
    }
}
