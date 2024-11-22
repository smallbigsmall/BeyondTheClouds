using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue {
    public string Background;
    public string Text;
}

[System.Serializable]
public class DialogueList {
    public List<Dialogue> dialogues;
}

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Setting")]
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private GameObject nextBtn, endPanel;
    [SerializeField]
    private Image fadeOutBackground;

    [Header("Background Setting")]
    [SerializeField]
    private List<Sprite> mPlayerSprites, fPlayerSprites; //up 0 down 1 left 2 right 3

    [SerializeField]
    private Transform cloud;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject trash;

    private DialogueList totalDialogue = new DialogueList();
    private List<Dialogue> dialogueList;
    private int index = 0;
    private float delay = 0.1f;
    private bool onePartFinished;
    private bool isPrologue;
    private int roomCount = 0;
    private Transform sceneCamera;
    private List<Sprite> playerSprites;
    private SpriteRenderer playerRenderer;
    void Start()
    {
        sceneCamera = GameObject.FindWithTag("MainCamera").transform;
        playerRenderer = player.GetComponent<SpriteRenderer>();

        TextAsset textAsset = null;
        if(SceneManager.GetActiveScene().name == "Prologue") {
            textAsset = Resources.Load<TextAsset>("Prologue");
            isPrologue = true;
        }
        else if(SceneManager.GetActiveScene().name == "Ending") {
            textAsset = Resources.Load<TextAsset>("Ending");
            isPrologue = false;
        }
        totalDialogue = JsonUtility.FromJson<DialogueList>(textAsset.ToString());
        dialogueList = totalDialogue.dialogues;

        if (!isPrologue)
        {
            int gender = GameManager.Instance.GetCurrentPlayerData().gender;
            if (gender == 2) playerSprites = fPlayerSprites;
            else playerSprites = mPlayerSprites;
            Display();
        }
    }

    public void SetPlayerSprite(int gender) {
        gender = GameManager.Instance.GetCurrentPlayerData().gender;
        if (gender == 2) playerSprites = fPlayerSprites;
        else playerSprites = mPlayerSprites;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onePartFinished) {
            Next();
        }
    }

    public void Display() {       
        onePartFinished = false;
        nextBtn.SetActive(false);
        if (index < dialogueList.Count) {
            playerRenderer.flipY = false;
            string currentDialogue = dialogueList[index].Text;
            SetStoryBackground(dialogueList[index].Background);
            StartCoroutine(ShowText(currentDialogue));
        }
        else {
            //EndDialogueSetting();
            fadeOutBackground.gameObject.SetActive(true);
            StartCoroutine(FadeOut());
        }
    }

    private void SetStoryBackground(string background) {
        switch (background) {
            case "Room": {
                    RoomBackgroundSetting(isPrologue, roomCount);
                    roomCount++;
                }               
                break;
            case "CloudMap": {
                    player.position = new Vector2(-3, -20);
                    playerRenderer.sprite = playerSprites[1];
                    sceneCamera.position = new Vector3(-1, -21, -10);
                }
                break;
            case "DayMission": {
                    cloud.position = new Vector2(-3, 14);
                    player.position = cloud.position + new Vector3(0, 1);
                    playerRenderer.sprite = playerSprites[1];
                    sceneCamera.position = player.position + new Vector3(0, 0,-10);
                }
                break;
            case "FarmFail": {
                    cloud.position = new Vector2(26, 8);
                    player.position = cloud.position + new Vector3(0, 1);
                    playerRenderer.sprite = playerSprites[1];
                    sceneCamera.position = cloud.position - new Vector3(5, 0, 10);
                }
                break;
            case "Garden": {
                    player.position = new Vector2(2.5f, 0);
                    playerRenderer.sprite = playerSprites[1];
                    sceneCamera.position = player.position + new Vector3(0, 0, -10);
                }
                break;
            case "NightMap": {
                    player.position = new Vector2(-19, -80);
                    playerRenderer.sprite = playerSprites[1];
                    sceneCamera.position = player.position + new Vector3(0, 0, -10);
                }
                break;
            case "NPC":
                player.position = new Vector2(12, 18);
                playerRenderer.sprite = playerSprites[2];
                sceneCamera.position = player.position + new Vector3(0, 0, -10);
                break;
        }
    }

    private void RoomBackgroundSetting(bool isPrologue, int count) {
        if (isPrologue) {
            if (count == 0) {
                player.position = new Vector2(74, -12);
                playerRenderer.sprite = playerSprites[0];               
            }
            else {
                player.position = new Vector2(68, -16.5f);
                playerRenderer.sprite = playerSprites[2];
                playerRenderer.flipY = true;
            }
        }
        else {
            trash.SetActive(false);
            player.position = new Vector2(68, -16.5f);
            playerRenderer.sprite = playerSprites[1];
            playerRenderer.flipY = true;
        }

        sceneCamera.position = new Vector3(71, -15, -10);
    }

    private void EndDialogueSetting() {
        if (isPrologue) GameManager.Instance.SetCurrentPlayerData(0, true);
        else
            GameManager.Instance.SetCurrentPlayerData(
                GameManager.Instance.GetCurrentPlayerData().stageNum + 1, false);

        PlayerDataManager.Instance.UpdatePlayerData(GameManager.Instance.GetCurrentPlayerData());
        dialogueText.transform.parent.gameObject.SetActive(false);
        endPanel.SetActive(true);

        endPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {
            GameManager.Instance.LoadMainMap();
        });
        endPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => {
            GameManager.Instance.LoadMainMenu();
        });
    }

    IEnumerator FadeOut() {
        float alpha = 0f;
        while (alpha < 1) {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeOutBackground.color = new Color(0, 0, 0, alpha);
        }
        EndDialogueSetting();
    }

    public void OnClickNextBtn() {
        Next();
    }

    private void Next() {
        index++;
        dialogueText.text = "";
        Display();
    }

    IEnumerator ShowText(string text) {
        int idx = 0;
        while(idx < text.Length) {
            dialogueText.text += text[idx].ToString();
            idx++;
            yield return new WaitForSeconds(delay);
        }
        nextBtn.SetActive(true);
        onePartFinished = true;
    }
}
