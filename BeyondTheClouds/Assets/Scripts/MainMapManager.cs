using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fPlayer, mPlayer;

    [SerializeField]
    private GameObject nightEnemyObj;

    [SerializeField]
    private Transform cloudRegion;

    [SerializeField]
    private Transform gameEndPanel;

    private List<Dictionary<string, Vector2>> regionList;
    private int currentRegion = -1;
    private float boundOffset = 6f;
    private int playerHp = 100;
    private int nightEnemyNum = 1;

    PlayerData currentPlayerData;
    // Start is called before the first frame update

    private Transform player;

    private TextMeshProUGUI resultText;
    private Button nextStepBtn, stageBtn;

    private void Awake() {
        currentPlayerData = GameManager.Instance.GetCurrentPlayerData();
    }

    private void InitializeMainMap() {
        // Instantiate player prefab
        if(currentPlayerData.gender == 'f') {
            player = Instantiate(fPlayer).transform;
        }
        else {
            player = Instantiate(mPlayer).transform;
        }

        if (currentPlayerData.dayCleared) { //start nighttime game
            player.position = new Vector2(-20, -80);
            FindAnyObjectByType<PlayerSkillManager>().gameObject.SetActive(false);
            InitializeNighttimeGame();
        }
        else {
            player.position = new Vector2(-4, -20);
            FindAnyObjectByType<PlayerSkillManager>().gameObject.SetActive(true);
        }

    }

    private void InitializeDatetimeGame() {

    }

    private void InitializeNighttimeGame() {
        
        InitializeRegions();
        // according dayNum, change setting of enemy 
        // adjust for loop bound
        SpawnEnemy();
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

        GameObject enemy = Instantiate(nightEnemyObj);
        NightEnemy nightEnemy = enemy.GetComponent<NightEnemy>();
        nightEnemy.SetRegionList(regionList);
        nightEnemy.SetCurrentRegion(currentRegion);
        nightEnemy.SetCurrentPos(new Vector2(xPos, yPos));
        nightEnemy.SetTotalLife(5);
        nightEnemy.StartWandering();
    }

    public void DecreasePlayerHp(int amount) {
        playerHp -= amount;

        if (playerHp <= 0) {
            playerHp = 0;
            //Show pop-up
            Debug.Log($"Day {currentPlayerData.stageNum} Night game over");

            gameEndPanel.gameObject.SetActive(true);
            SetGameEndUI(false);
        }
    }

    public void DecreaseNightEnemyCount() {
        nightEnemyNum--;
        if(nightEnemyNum == 0) {
            int currentDay = currentPlayerData.stageNum;
            GameManager.Instance.SetCurrentPlayerData(currentDay, true);
            PlayerDataManager.Instance.UpdatePlayerData(currentDay + 1, false);
            Debug.Log($"{currentPlayerData.stageNum} night game clear");

            gameEndPanel.gameObject.SetActive(true);
            SetGameEndUI(true);
        }
    }

    private void SetGameEndUI(bool isCleared) {
        if (isCleared) {
            nextStepBtn.onClick.AddListener(InitializeNextStepGame);
            nextStepBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "다음 단계";
            resultText.text = "Game Clear";
        }
        else {
            nextStepBtn.onClick.AddListener(Replay);
            nextStepBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "다시 플레이";
            resultText.text = "Game Over";
        }


    }

    private void Replay() {

    }

    private void InitializeNextStepGame() {

    }

    private void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        InitializeMainMap();

        resultText = gameEndPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        nextStepBtn = gameEndPanel.GetChild(1).GetComponent<Button>();
        stageBtn = gameEndPanel.GetChild(2).GetComponent<Button>();

        stageBtn.onClick.AddListener(GoToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
