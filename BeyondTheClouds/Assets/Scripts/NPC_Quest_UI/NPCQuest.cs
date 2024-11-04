using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    [SerializeField] GameObject QuestUI;
    private GameObject QuestUIScrollView;
    private bool isQuestNPC = false;
    [SerializeField] GameObject QuestPopup;
    private GameObject newQuest;

    DialogueList dialogueList;
    [SerializeField] TextAsset jsonDialogue;

    void Start()
    {

        QuestUIScrollView = GameObject.FindGameObjectWithTag("QuestUIScrollView");
 
        dialogueList = JsonUtility.FromJson<DialogueList>(jsonDialogue.text);
        Debug.Log(dialogueList.Dialogues[0].Lines[0].line);
        Debug.Log(dialogueList.Dialogues[0].Lines[1].line);
    }

    public void AddQuestUI() {
        newQuest = Instantiate(QuestUI);
        newQuest.transform.SetParent(QuestUIScrollView.transform);
        GetComponentInParent<MissionSettingWithQuest>().QuestUI = newQuest;
    }

    public void MakeNPCQuest(string Mission) {
        isQuestNPC = true;
        QuestPopup.SetActive(true);
        AddQuestUI(); //For test. 나중에 이 부분 지우기.
    }

    public void StartConversation() {
        if (isQuestNPC)
        {
            //대화
            //선택지
            AddQuestUI();
        }
        else { 
            //일반 대화
        }
    }

    /*
     * start하면 미리 파싱은 끝내놓기.
     * NPC 클릭되면 StartConversation() 실행됨.
     * 엔딩 보기전까지는 Day 기준으로 대사 출력.
     * 엔딩 후 확장 버전에서는 랜덤하게 미션이 정해지므로 선택된 Mission 참조하고 
     * index 랜덤으로 돌려서 나온 대사 출력
     * 플레이어가 퀘스트 받아들이면 퀘스트 UI의 텍스트 설정해주고 Add 하기
     */

    /*
     * 1. 미션 매니저가 각 구역에 미션 셋팅함
     * 2. 이때 각 NPC의 상태도 미션있음/없음 상태로 바꿈
     * 3. 퀘스트 대화 끝나면 NPC는 QuestUI를 만듦.
     * 4. 퀘스트의 내용은 Mission를 기준으로 json 텍스트 가져옴. Index의 경우 랜덤 아니면
     * 수동으로 설정
     * 영역별로 json을 따로 만들어줄거기 때문에 씬에 배치된 npc에 할당된 json에 따라
     * 영역은 알아서 구분될 예정
     */
}
