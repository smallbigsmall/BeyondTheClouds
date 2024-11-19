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

    private void Start()
    {
        //���� Ű�� ������ ��ư ��Ȱ��ȭ ��Ű��
        if (true)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void AcceptByVoice() {
        _witSTT.SetSTTui(true);
    }

    public void RejectByVoice()
    {
        _witSTT.SetSTTui(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        popup.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        popup.SetActive(false);
    }
}
