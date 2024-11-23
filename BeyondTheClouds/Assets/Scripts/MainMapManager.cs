using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMapManager : MonoBehaviour
{
    [Header("Default Setting")]
    [SerializeField]
    private GameObject fPlayer;

    [SerializeField]
    private GameObject mPlayer;

    [SerializeField]
    private GameObject fadeOutImg;

    [SerializeField] 
    private SoundManager _soundManager;

    [Header("Night Mission Setting")]
    [SerializeField]
    private List<GameObject> nightEnemyPrefList;

    [SerializeField]
    private Transform cloudRegion;

    [SerializeField]
    private Transform gameEndPanel;

    [SerializeField]
    private Transform confidenceBar;

    [Header("Day Mission Setting")]
    [SerializeField]
    private WeatherMissionManager _weatherMissionManager;

    [SerializeField]
    private CloudFadeOut _cloudFadeOut;

    [SerializeField]
    private CloudFadeOut _cloudFadeOutMap;

    [SerializeField]
    private RoomCleaner roomCleaner;

    [SerializeField]
    private CompositeCollider2D borderCollider;

    private GameObject MyGardenQuestMark;

    private List<Dictionary<string, Vector2>> regionList;
    private int currentRegion = -1;
    private float boundOffset = 3.5f;
    private int playerHp = 100;
    private int nightEnemyNum = 1;
    private Transform cloudMap;

    private Image confidenceFilledImg;
    private TextMeshProUGUI confidencePercent;

    PlayerData currentPlayerData;

    private Transform player;
    private CameraController cameraController; 

    private TextMeshProUGUI resultText, gameEndGuideText;
    private Button nextStepBtn, stageBtn;
    private GameObject skillPanel;
    private bool allMissionAccepted;

    private void Awake() {
        currentPlayerData = GameManager.Instance.GetCurrentPlayerData();
    }

    private void InitializeMainMap() {
        // Instantiate player prefab
        if(currentPlayerData.gender == 2) {
            player = Instantiate(fPlayer).transform;
        }
        else {
            player = Instantiate(mPlayer).transform;
        }

        Debug.Log("Current day: " + currentPlayerData.stageNum);

        cameraController.FindPlayer(player.transform);
        skillPanel = FindAnyObjectByType<PlayerSkillManager>().gameObject;
        player.transform.GetChild(1).GetComponent<NPCQuest>().MapQuestMark = MyGardenQuestMark;

        if (currentPlayerData.dayCleared) { //start nighttime game
            player.position = new Vector2(-20, -80);
            SetSkillPanel(false, true);
            confidenceBar.gameObject.SetActive(true);
            confidenceFilledImg = confidenceBar.Find("Fill").GetComponent<Image>();
            confidencePercent = confidenceBar.Find("Percent").GetComponent<TextMeshProUGUI>();
            cameraController.FollowPlayer();
            InitializeNighttimeGame();
        }
        else {
            player.position = new Vector2(53, -74);
            if(currentPlayerData.stageNum != 1) cameraController.ShowMap();
            if (currentPlayerData.stageNum == 1) cameraController.FollowPlayer();
            SetSkillPanel(false, true);
            confidenceBar.gameObject.SetActive(false);
            InitializeDatetimeGame();          
        }

    }

    private void InitializeDatetimeGame() {
        _soundManager.SetDay(currentPlayerData.stageNum, currentPlayerData.dayCleared);
        _cloudFadeOut.initCloud(currentPlayerData.stageNum);
        _cloudFadeOutMap.initCloud(currentPlayerData.stageNum);
        if (currentPlayerData.stageNum == 1) roomCleaner.InitializeTrash();
        _cloudFadeOut.FadeOutCloud(currentPlayerData.stageNum);
    }

    private void InitializeNighttimeGame() {
        
        InitializeRegions();
        // according dayNum, change setting of enemy 
        // adjust for loop bound

        int stage = currentPlayerData.stageNum;
        nightEnemyNum = Random.Range(7-stage, 10-stage);
        for(int i = 0; i<nightEnemyNum; i++) {
            SpawnEnemy();
        }
        _soundManager.SetDay(currentPlayerData.stageNum, currentPlayerData.dayCleared);
    }

    private void InitializeRegions() {
        regionList = new List<Dictionary<string, Vector2>>();

        for (int i = 0; i < 4; i++) {
            Dictionary<string, Vector2> region = new Dictionary<string, Vector2>();
            region["minBound"] = new Vector2(0, 0);
            region["maxBound"] = new Vector2(0, 0);
            regionList.Add(region);
        }

        Vector2 cloudRegion_pos = cloudRegion.position;
        Vector2 cloudRegion_scale = cloudRegion.localScale;
        regionList[0]["minBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2,
            cloudRegion_pos.y - cloudRegion_scale.y / 2);
        regionList[0]["maxBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2 + boundOffset,
            cloudRegion_pos.y + cloudRegion_scale.y / 2);

        regionList[1]["minBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2,
            cloudRegion_pos.y + cloudRegion_scale.y / 2);
        regionList[1]["maxBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2,
            cloudRegion_pos.y + cloudRegion_scale.y / 2 + boundOffset);

        regionList[2]["minBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2 - boundOffset,
            cloudRegion_pos.y - cloudRegion_scale.y / 2);
        regionList[2]["maxBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2,
            cloudRegion_pos.y + cloudRegion_scale.y / 2);

        regionList[3]["minBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2,
            cloudRegion_pos.y - cloudRegion_scale.y / 2 - boundOffset);
        regionList[3]["maxBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2,
            cloudRegion_pos.y - cloudRegion_scale.y / 2);
    }

    private void SpawnEnemy() {
        currentRegion = Random.Range(0, 4);

        Vector2 minBound = regionList[currentRegion]["minBound"];
        Vector2 maxBound = regionList[currentRegion]["maxBound"];

        float xPos = Random.Range(minBound.x, maxBound.x);
        float yPos = Random.Range(minBound.y, maxBound.y);

        int randEnemyNum = Random.Range(0, nightEnemyPrefList.Count-1);

        GameObject enemy = Instantiate(nightEnemyPrefList[randEnemyNum]);
        NightEnemy nightEnemy = enemy.GetComponent<NightEnemy>();
        nightEnemy.SetRegionList(regionList);
        nightEnemy.SetCurrentRegion(currentRegion);
        nightEnemy.SetCurrentPos(new Vector2(xPos, yPos));
        nightEnemy.SetTotalLife(5);
        nightEnemy.StartWandering();
    }

    public void DecreasePlayerHp(int amount) {
        playerHp -= amount;
        confidenceFilledImg.fillAmount = (float)playerHp / 100;
        Color32 newColor = confidenceFilledImg.color;
        if (playerHp <= 60) {
            newColor = new Color32(240, 215, 23, 255);
        }else if(playerHp <= 30) {
            newColor = new Color32(222, 16, 29, 255);
        }
        confidenceFilledImg.color = newColor;
        confidencePercent.text = playerHp + "%";
        confidencePercent.color = newColor;
        if (playerHp <= 0) {
            playerHp = 0;
            confidencePercent.text = playerHp + "%";
            //Show pop-up
            Debug.Log($"Day {currentPlayerData.stageNum} Night game over");

            gameEndPanel.gameObject.SetActive(true);
            SetGameEndUI(true, false);
        }
    }

    public void DecreaseNightEnemyCount() {
        nightEnemyNum--;
        if(nightEnemyNum == 0) {
            int currentDay = currentPlayerData.stageNum;
            GameManager.Instance.SetCurrentPlayerData(currentDay+1, false);
            PlayerDataManager.Instance.UpdatePlayerData(GameManager.Instance.GetCurrentPlayerData());
            Debug.Log($"{currentPlayerData.stageNum} night game clear");

            gameEndPanel.gameObject.SetActive(true);
            SetGameEndUI(true, true);
        }
    }

    private void SetGameEndUI(bool isNight, bool isCleared) {
        if (!isNight) {
            gameEndGuideText.text = "Night 게임을 플레이하시겠습니까?";
            resultText.text = "Mission Clear";
        }
        else {
            if (isCleared) {
                gameEndGuideText.text = "다음 날짜를\n플레이하시겠습니까?";
                resultText.text = "Game Clear";
            }
            else {
                gameEndGuideText.text = "다시 플레이하시겠습니까?";
                resultText.text = "Game Over";
            }
        }
    }

    public void FinishDay() {
        fadeOutImg.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() {
        float alpha = 0f;
        while(alpha < 1) {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeOutImg.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        }
        gameEndPanel.gameObject.SetActive(true);
        SetGameEndUI(false, true);
    }

    private void ReloadMainMap() {
        string mainMap = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(mainMap);
    }
   

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void CleaningMissionFinished() {
        Debug.Log("Cleaning Mission finished");
        roomCleaner.FinishCleaning();
    }

    public void SetSkillPanel(bool showing, bool onCloudMap) {
        if (showing) {
            skillPanel.SetActive(true);
            if (onCloudMap) {
                for(int i = 1; i < skillPanel.transform.childCount-1; i++) {
                    skillPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
                }
            }
            else {
                for (int i = 0; i < skillPanel.transform.childCount; i++) {
                    skillPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
        }
        else {
            skillPanel.SetActive(false);
        }
    }

    public void GoToCloudMap() {
        borderCollider.isTrigger = true;
    }

    public void LeaveCloudMap() {
        borderCollider.isTrigger = false;
    }

    public void TakeOffCloud() {
        cloudMap.GetComponent<CompositeCollider2D>().isTrigger = false;
    }

    public void SetAllMissionAccepted(bool accepted) {
        allMissionAccepted = accepted;
    }

    public bool GetAllMissionAccepted() {
        return allMissionAccepted;
    }

    void Start()
    {     
        resultText = gameEndPanel.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        gameEndGuideText = gameEndPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        nextStepBtn = gameEndPanel.GetChild(1).GetChild(1).GetComponent<Button>();
        stageBtn = gameEndPanel.GetChild(1).GetChild(2).GetComponent<Button>();

        nextStepBtn.onClick.AddListener(ReloadMainMap);
        stageBtn.onClick.AddListener(GoToMainMenu);

        cloudMap = GameObject.FindWithTag("CloudMap").transform;
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();

        InitializeMainMap();
    }

}
