using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShowNPCDialogue : MonoBehaviour
{
    DialogueList dialogueList;
    [SerializeField] TextAsset jsonDialogue;

    private void Start()
    {
        dialogueList = JsonUtility.FromJson<DialogueList>(jsonDialogue.text);
        Debug.Log(dialogueList.Dialogues[0].Lines[0].line);
        Debug.Log(dialogueList.Dialogues[0].Lines[1].line);
    }

    public void ShowDialogue() { 
    
    }

    
}
