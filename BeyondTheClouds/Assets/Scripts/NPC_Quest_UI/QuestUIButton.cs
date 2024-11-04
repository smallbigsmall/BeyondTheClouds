using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIButton : MonoBehaviour
{
    [SerializeField] Animator questUIAnimator;
    private bool isOpen = true;

    public void MoveQuestUI() {
        if (isOpen)
        {
            isOpen = !isOpen;
            questUIAnimator.SetTrigger("closeQuest");
        }
        else {
            isOpen = !isOpen;
            questUIAnimator.SetTrigger("openQuest");
        }
        
    }
}
