using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResetData : MonoBehaviour
{
    public int player_gender;
    public int day;
    public bool dayCleared;

    private void OnApplicationQuit() {
        string filePath = Application.persistentDataPath + "/PlayerData.json";
        if (File.Exists(filePath)) {
            if((player_gender != 1 && player_gender != 2) || (day<=0 && day >= 8)) {
                Debug.Log("Reset data");
                try {
                    File.Delete(filePath);
                    Debug.Log("Success to delete");
                }
                catch (Exception e) {
                    Debug.Log(e);
                }
            }
            else {
                Debug.Log($"Reset data: gender - {player_gender}, day - {day}, dayCleared - {dayCleared}");
                string write_str = JsonUtility.ToJson(new PlayerData() {gender = player_gender, stageNum=day, dayCleared=dayCleared});
                File.WriteAllText(filePath, write_str);
            }
            
        }
    }
}
