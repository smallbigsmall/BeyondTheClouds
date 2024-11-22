using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionSettingWithQuest : MonoBehaviour
{
    public GameObject QuestUI;
    public GameObject mapQuestMark; //�ٸ� ������Ʈ�κ��� �Ҵ�����Ŷ� �ν����Ϳ��� �Ű澵�ʿ�X
    private Image questMainImg;

    public void CompleteQuestUI() {
        Debug.Log("Mission complete!!! change the UI");
        if (QuestUI == null) return;
        questMainImg = QuestUI.transform.GetChild(0).GetComponent<Image>();
        questMainImg.color = new Color(questMainImg.color.r, questMainImg.color.g, questMainImg.color.b, 0.5f);
        QuestUI.transform.GetChild(1).gameObject.SetActive(true);
        mapQuestMark.SetActive(false);
        GameObject.FindWithTag("Sound").GetComponent<SoundManager>().playQuestSound();
    }
}
