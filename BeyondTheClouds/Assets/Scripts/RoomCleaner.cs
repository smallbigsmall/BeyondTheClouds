using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCleaner : MissionSettingWithQuest
{
    [SerializeField]
    private List<GameObject> trashObjects;

    [SerializeField]
    private Transform trashParent, trashZone;

    [SerializeField]
    private GameObject portal, vacuum;  

    private int totalTrashNum;
    private bool allCleaned;
    private GameObject playerVacuum;
    private Transform player;

    [SerializeField] WeatherMissionManager _weatherMissionManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeRoomCleanMission() {
        portal.SetActive(false);

        totalTrashNum = trashParent.childCount;

        for(int i = 0; i < trashParent.childCount; i++)
        {
            trashParent.GetChild(i).gameObject.SetActive(true);
        }
        

        int additionalTrashNum = Random.Range(5, 9);
        Vector2 minPos = trashZone.GetChild(1).position;
        Vector2 maxPos = trashZone.GetChild(2).position;

        totalTrashNum += additionalTrashNum;

        int trashObjectsCount = trashObjects.Count;
        for (int i = 0; i < additionalTrashNum; i++) {
            int randObjIdx = Random.Range(0, trashObjectsCount);
            float randPosX = Random.Range(minPos.x, maxPos.x);
            float randPosY = Random.Range(minPos.y, maxPos.y);
            GameObject trash = Instantiate(trashObjects[randObjIdx], new Vector3(randPosX, randPosY), Quaternion.identity);
            trash.transform.SetParent(trashParent);
        }

        player = GameObject.FindWithTag("Player").transform;

        playerVacuum = Instantiate(vacuum, player);
        playerVacuum.transform.localPosition = new Vector2(-0.16f, -1f);
    }

    public void RemoveTrash() {
        totalTrashNum--;

        if(totalTrashNum == 0) {
            allCleaned = true;
            Debug.Log("All trash removed");
            _weatherMissionManager.MissionComplete();
            CompleteQuestUI();
        }
    }

    public bool GetAllCleaned() {
        return allCleaned;
    }
    
    public void FinishCleaning() {
        if(playerVacuum != null) Destroy(playerVacuum);
        portal.SetActive(true);
    }
}
