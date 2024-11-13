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

    [Header("Night Mission Setting")]
    [SerializeField]
    private List<GameObject> nightEnemyPrefList;

    [SerializeField]
    private Transform cloudRegion;

    [SerializeField]
    private Transform gameEndPanel;

    [SerializeField]
    private Transform confidenceBar;

    private List<Dictionary<string, Vector2>> regionList;
    private int currentRegion = -1;
    private float boundOffset = 3.5f;
    private int playerHp = 100;
    private int nightEnemyNum = 1;

    private Image confidenceFilledImg;
    private TextMeshProUGUI confidencePercent;

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
            confidenceBar.gameObject.SetActive(true);
            confidenceFilledImg = confidenceBar.Find("Fill").GetComponent<Image>();
            confidencePercent = confidenceBar.Find("Percent").GetComponent<TextMeshProUGUI>();
            InitializeNighttimeGame();
        }
        else {
            player.position = new Vector2(-4, -20);
            FindAnyObjectByType<PlayerSkillManager>().gameObject.SetActive(true);
            confidenceBar.gameObject.SetActive(false);
        }

    }

    private void InitializeDatetimeGame() {

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
        confidencePercent.text = playerHp + "%";
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
            nextStepBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "다음 단계";
            resultText.text = "Game Clear";
        }
        else {
            nextStepBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "다시 플레이";
            resultText.text = "Game Over";
        }


    }

    private void ReloadMainMap() {
        string mainMap = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(mainMap);
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

        nextStepBtn.onClick.AddListener(ReloadMainMap);
        stageBtn.onClick.AddListener(GoToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
