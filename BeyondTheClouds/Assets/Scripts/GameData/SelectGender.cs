using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGender : MonoBehaviour
{
    [SerializeField]
    private GameObject genderPanel, dialogueBox;

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        PlayerData currentPlayerData = GameManager.Instance.GetCurrentPlayerData();
        Debug.Log("Current player gender: " + currentPlayerData.gender);
        if (currentPlayerData.gender != 1 && currentPlayerData.gender != 2) {
            genderPanel.SetActive(true);
            dialogueBox.SetActive(false);
        }
        else {
            FinishSelecting();
        }
    }

    public void SelectPlayerGender(bool isBoy) {
        if (isBoy) GameManager.Instance.SetCurrentPlayerGender(1);
        else GameManager.Instance.SetCurrentPlayerGender(2);

        int startBtnIdx = genderPanel.transform.childCount - 1;
        genderPanel.transform.GetChild(startBtnIdx).GetComponent<Button>().interactable = true;
    }

    public void FinishSelecting() {
        genderPanel.SetActive(false);
        dialogueBox.SetActive(true);

        transform.GetComponent<DialogueSystem>().SetPlayerSprite(GameManager.Instance.GetCurrentPlayerData().gender);
        transform.GetComponent<DialogueSystem>().Display();
        audioSource.Play();
    }

}
