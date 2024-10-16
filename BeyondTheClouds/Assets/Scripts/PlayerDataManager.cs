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
    }


    public void SavePlayerData() {
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

    private void OnApplicationQuit() {
        playerData.stageNum = 3;
        playerData.dayCleared = true;

        SavePlayerData();
    }

}
