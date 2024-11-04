using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSetting : MonoBehaviour
{
    enum FlowerType { Sunflower, WhiteFlower1, WhiteFlower2 }

    [SerializeField] Sprite[] flowerSpriteList;
    private Color color;
    [SerializeField] SpriteRenderer flowerSpriteRenderer;
    [SerializeField] FlowerType flowerType;
    private int life = 3;
    private int MissionMode = 1; //0 == sprout -> flower, 1 == Drought, 2 == Overwatering
    private MyGardenSetting _myGardenSetting;
    private bool isTodayMission = false;

    void Start()
    {
        _myGardenSetting = gameObject.transform.parent.parent.gameObject.GetComponent<MyGardenSetting>();
        flowerSpriteRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        flowerSpriteRenderer.sprite = flowerSpriteList[((int)flowerType)];
        switch (flowerType) {
            case FlowerType.WhiteFlower1: //three white flower
                gameObject.transform.GetChild(0).localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
            case FlowerType.WhiteFlower2: //one big white flower
                gameObject.transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);
                break;
            default:
                break;
        }
    }

    public void InitToSprout() {
        MissionMode = 0;
        flowerSpriteRenderer.sprite = flowerSpriteList[flowerSpriteList.Length-1];
        gameObject.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
        isTodayMission = true;
    }

    public void FlowerColorSetting(string colorCode, bool isDrought) {
        if (isDrought) MissionMode = 1;
        else MissionMode = 2;

        ColorUtility.TryParseHtmlString(colorCode, out color);
        flowerSpriteRenderer.color = color;
        isTodayMission = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain") && MissionMode != 2 && isTodayMission)
        {
            life -= 1;
            if (life == 0 && MissionMode == 1)
            {
                ColorUtility.TryParseHtmlString("#FFFFFF", out color); //white
                flowerSpriteRenderer.color = color;
                _myGardenSetting.countFlowerComplete();
            } else if (life == 0 && MissionMode == 0) {
                switch (flowerType)
                {
                    case FlowerType.WhiteFlower1: //three white flower
                        gameObject.transform.GetChild(0).localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        break;
                    case FlowerType.WhiteFlower2: //one big white flower
                        gameObject.transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        break;
                    default:
                        break;
                }
                flowerSpriteRenderer.sprite = flowerSpriteList[((int)flowerType)];
                _myGardenSetting.countFlowerComplete();
            }
        } else if (collision.gameObject.CompareTag("Shadow") && MissionMode == 2 && isTodayMission) {
            ColorUtility.TryParseHtmlString("#FFFFFF", out color); //white
            flowerSpriteRenderer.color = color;
            _myGardenSetting.countFlowerComplete();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shadow") && MissionMode == 2 && isTodayMission)
        {
            ColorUtility.TryParseHtmlString("#CF9700", out color); //white
            flowerSpriteRenderer.color = color;
            _myGardenSetting.countFlowerDrought();
        }
    }
}
