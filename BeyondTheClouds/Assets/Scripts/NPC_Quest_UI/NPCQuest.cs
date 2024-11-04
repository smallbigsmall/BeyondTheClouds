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
        AddQuestUI(); //For test. ���߿� �� �κ� �����.
    }

    public void StartConversation() {
        if (isQuestNPC)
        {
            //��ȭ
            //������
            AddQuestUI();
        }
        else { 
            //�Ϲ� ��ȭ
        }
    }

    /*
     * start�ϸ� �̸� �Ľ��� ��������.
     * NPC Ŭ���Ǹ� StartConversation() �����.
     * ���� ������������ Day �������� ��� ���.
     * ���� �� Ȯ�� ���������� �����ϰ� �̼��� �������Ƿ� ���õ� Mission �����ϰ� 
     * index �������� ������ ���� ��� ���
     * �÷��̾ ����Ʈ �޾Ƶ��̸� ����Ʈ UI�� �ؽ�Ʈ �������ְ� Add �ϱ�
     */

    /*
     * 1. �̼� �Ŵ����� �� ������ �̼� ������
     * 2. �̶� �� NPC�� ���µ� �̼�����/���� ���·� �ٲ�
     * 3. ����Ʈ ��ȭ ������ NPC�� QuestUI�� ����.
     * 4. ����Ʈ�� ������ Mission�� �������� json �ؽ�Ʈ ������. Index�� ��� ���� �ƴϸ�
     * �������� ����
     * �������� json�� ���� ������ٰű� ������ ���� ��ġ�� npc�� �Ҵ�� json�� ����
     * ������ �˾Ƽ� ���е� ����
     */
}
