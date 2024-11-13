using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] GameObject MapImage;

    public void MapUIButton() {
        if (MapImage.activeSelf)
        {
            MapImage.SetActive(false);
        }
        else {
            MapImage.SetActive(true);
        }
    }
}
