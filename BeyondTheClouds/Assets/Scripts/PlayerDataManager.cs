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

        //playerData = new PlayerData();
        playerData = LoadPlayerData();
    }


    public void SavePlayerData() {
        PlayerData currentData = GameManager.Instance.GetCurrentPlayerData();
        UpdatePlayerData(currentData);
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

    public void UpdatePlayerData(PlayerData currentPlayerData) {
        //whenever mission finished, update player data
        // playerData vs currentPlayerData
        if(playerData.gender != currentPlayerData.gender) {
            playerData.gender = currentPlayerData.gender;
        }

        if(playerData.stageNum < currentPlayerData.stageNum) {
            playerData.stageNum = currentPlayerData.stageNum;
            if(playerData.dayCleared && !currentPlayerData.dayCleared) {
                playerData.dayCleared = false;
            }
        }else if(playerData.stageNum == currentPlayerData.stageNum) {
            if(!playerData.dayCleared && currentPlayerData.dayCleared) {
                playerData.dayCleared = true;
            }
        }

        if(playerData.sttKey != currentPlayerData.sttKey) {
            playerData.sttKey = currentPlayerData.sttKey;
        }
    }

    public PlayerData GetPlayerData() {
        return playerData;
    }

    private void OnApplicationQuit() {
        SavePlayerData();
    }

}
