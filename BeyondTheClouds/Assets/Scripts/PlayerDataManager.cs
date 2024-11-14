using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    private PlayerData playerData;
    private string filePath;

    //signleton
    private void Awake() {
        filePath = Application.persistentDataPath + "/PlayerData.json";

        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        playerData = new PlayerData();
    }


    public void SavePlayerData() {
        PlayerData currentData = GameManager.Instance.GetCurrentPlayerData();
        if (currentData.stageNum > playerData.stageNum || 
            (currentData.stageNum == playerData.stageNum && currentData.dayCleared != playerData.dayCleared)) {
            UpdatePlayerData(currentData.stageNum, currentData.dayCleared);
        }
        string write_str = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath, write_str);
        Debug.Log($"Save Player Data in {filePath}");
    }

    public PlayerData LoadPlayerData() {
        if (File.Exists(filePath) == false) {
            Debug.Log("There's no data to load");
            playerData = new PlayerData();
            playerData.stageNum = 0;
            playerData.dayCleared = false;
            return playerData;
        }

        string read_str = File.ReadAllText(filePath);
        playerData = JsonUtility.FromJson<PlayerData>(read_str);

        Debug.Log("Success to load player data. The current stage is " + playerData.stageNum);

        return playerData;

    }

    public int GetSavedStageNum() {
        return playerData.stageNum;
    }

    public bool GetSavedDayCleared() {
        return playerData.dayCleared;
    }

    public void SetPlayerGender(char gender) {
        playerData.gender = gender;
    }

    public void UpdatePlayerData(int day, bool dayCleared) {
        playerData.stageNum = day;
        playerData.dayCleared = dayCleared;
    }

    private void OnApplicationQuit() {
        //for testing
        playerData.gender = 'm';
        playerData.stageNum = 7;
        playerData.dayCleared = false;

        SavePlayerData();
    }

    

}
