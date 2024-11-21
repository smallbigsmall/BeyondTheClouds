using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcClickTest : MonoBehaviour
{
    [SerializeField] GameObject DialogueCanvas, ChoiceUI;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null && (hit.collider.CompareTag("NPC"))) {
                hit.collider.GetComponent<NPCQuest>().SetDialogueUI(DialogueCanvas, ChoiceUI);
                hit.collider.GetComponent<NPCQuest>().StartConversation();
            }
        }
    }

    public void playerButtonClick() {
        NPCQuest _npcQuest = GameObject.FindWithTag("Player").transform.GetChild(1).GetComponent<NPCQuest>();
        _npcQuest.SetIsPlayerTrue();
        _npcQuest.SetDialogueUI(DialogueCanvas, ChoiceUI);
        _npcQuest.StartConversation();
    }
}
