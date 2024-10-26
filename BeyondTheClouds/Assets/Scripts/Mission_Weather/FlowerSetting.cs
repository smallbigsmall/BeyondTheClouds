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
    private int MissionMode = 1; //0 == sprout -> flower, 1 == change flower color
    private MyGardenSetting _myGardenSetting;

    void Start()
    {
        _myGardenSetting = gameObject.transform.parent.gameObject.GetComponent<MyGardenSetting>();
        flowerSpriteRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        flowerSpriteRenderer.sprite = flowerSpriteList[((int)flowerType)];
        switch (flowerType) {
            case FlowerType.WhiteFlower1: //three white flower
                gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
            case FlowerType.WhiteFlower2: //one big white flower
                gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                break;
            default:
                break;
        }
    }

    public void InitToSprout() {
        MissionMode = 0;
        flowerSpriteRenderer.sprite = flowerSpriteList[flowerSpriteList.Length-1];
    }

    public void FlowerColorSetting(string colorCode) {
        MissionMode = 1;
        ColorUtility.TryParseHtmlString(colorCode, out color);
        flowerSpriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rain"))
        {
            life -= 1;
            if (life == 0 && MissionMode == 1)
            {
                ColorUtility.TryParseHtmlString("#FFFFFF", out color); //white
                flowerSpriteRenderer.color = color;
                _myGardenSetting.countFlowerComplete();
            } else if (life == 0 && MissionMode == 0) {
                flowerSpriteRenderer.sprite = flowerSpriteList[((int)flowerType)];
                _myGardenSetting.countFlowerComplete();
            }
        }
    }
}
