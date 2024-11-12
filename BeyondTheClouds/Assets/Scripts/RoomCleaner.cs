using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCleaner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> trashObjects;

    [SerializeField]
    private Transform trashParent, trashZone;

    private int totalTrashNum;
    private bool allCleaned;
    // Start is called before the first frame update
    void Start()
    {
        totalTrashNum = trashParent.childCount;

        int additionalTrashNum = Random.Range(5, 9);
        Vector2 minPos = trashZone.GetChild(1).position;
        Vector2 maxPos = trashZone.GetChild(2).position;

        totalTrashNum += additionalTrashNum;

        int trashObjectsCount = trashObjects.Count;
        for (int i=0; i< additionalTrashNum; i++) {
            int randObjIdx = Random.Range(0, trashObjectsCount);
            float randPosX = Random.Range(minPos.x, maxPos.x);
            float randPosY = Random.Range(minPos.y, maxPos.y);
            GameObject trash = Instantiate(trashObjects[randObjIdx], new Vector3(randPosX, randPosY), Quaternion.identity);
            trash.transform.SetParent(trashParent);
        }
    }

    public void RemoveTrash() {
        totalTrashNum--;

        if(totalTrashNum == 0) {
            allCleaned = true;
            Debug.Log("All trash removed");
        }
    }

    public bool GetAllCleaned() {
        return allCleaned;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
