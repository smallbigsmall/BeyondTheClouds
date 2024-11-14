using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //전체적인 게임 흐름 관리
    //MainMap scene으로 넘어가면 그 scene의 매니저에게 필요한 정보 넘겨주기

    private PlayerData playerData;
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //stage 단계에서 내부 값 변경
        playerData = new PlayerData();
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMap() {
        SceneManager.LoadScene("MainMap");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerData GetCurrentPlayerData() {
        return playerData;
    }

    public void SetCurrentPlayerGender(char gender) {
        playerData.gender = gender;
    }

    public void SetCurrentPlayerData(int day, bool isNight) {
        playerData.stageNum = day;
        playerData.dayCleared = isNight;

    }
}
