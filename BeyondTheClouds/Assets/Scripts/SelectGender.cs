using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGender : MonoBehaviour
{
    [SerializeField]
    private GameObject genderPanel, dialogueBox;

    void Start()
    {
        PlayerData currentPlayerData = GameManager.Instance.GetCurrentPlayerData();
        Debug.Log("Current player gender: " + currentPlayerData.gender);
        if (currentPlayerData.gender != 'm' && currentPlayerData.gender != 'f') {
            genderPanel.SetActive(true);
            dialogueBox.SetActive(false);
        }
    }

    public void SelectPlayerGender(string gender) {
        PlayerDataManager.Instance.SetPlayerGender(gender[0]);
        GameManager.Instance.SetCurrentPlayerGender(gender[0]);

        int startBtnIdx = genderPanel.transform.childCount - 1;
        genderPanel.transform.GetChild(startBtnIdx).GetComponent<Button>().interactable = true;
    }

    public void FinishSelecting() {
        genderPanel.SetActive(false);
        dialogueBox.SetActive(true);

        transform.GetComponent<DialogueSystem>().Display();
    }

}
