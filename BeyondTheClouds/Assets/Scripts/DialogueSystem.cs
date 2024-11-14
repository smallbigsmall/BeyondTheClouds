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
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private GameObject nextBtn, endPanel;
    private DialogueList totalDialogue = new DialogueList();
    private List<Dialogue> dialogueList;
    private int index = 0;
    private float delay = 0.1f;
    private bool onePartFinished;
    private bool isPrologue;
    void Start()
    {
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

        Display();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && onePartFinished) {
            Next();
        }
    }

    private void Display() {
        onePartFinished = false;
        nextBtn.SetActive(false);
        if (index < dialogueList.Count) {           
            string currentDialogue = dialogueList[index].Text;
            StartCoroutine(ShowText(currentDialogue));
        }
        else {
            Debug.Log("Prologue end");
            EndDialogueSetting();           
        }
    }

    private void EndDialogueSetting() {
        if (isPrologue) GameManager.Instance.SetCurrentPlayerData(0, true);
        else
            GameManager.Instance.SetCurrentPlayerData(
                GameManager.Instance.GetCurrentPlayerData().stageNum + 1, false);
        dialogueText.transform.parent.gameObject.SetActive(false);
        endPanel.SetActive(true);

        endPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {
            GameManager.Instance.LoadMainMap();
        });
        endPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => {
            GameManager.Instance.LoadMainMenu();
        });
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
