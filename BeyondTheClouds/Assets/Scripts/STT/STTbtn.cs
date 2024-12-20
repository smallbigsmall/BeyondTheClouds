using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class STTbtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] WitSTT _witSTT;
    [SerializeField] GameObject popup;
    private NPCQuest _npcQuest;

    private void Start()
    {
        if (GameManager.Instance.GetSttKey().Equals(""))
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void AcceptByVoice() {
        _witSTT.SetSTTui(true, _npcQuest);
    }

    public void RejectByVoice()
    {
        _witSTT.SetSTTui(false, _npcQuest);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(!GameManager.Instance.GetSttKey().Equals(""))
            popup.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!GameManager.Instance.GetSttKey().Equals(""))
            popup.SetActive(false);
    }

    public void SetNPCQuest(NPCQuest npcQuest) {
        _npcQuest = npcQuest;
    }
}
