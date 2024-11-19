using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionSTTSet : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    public void SaveSttKeyToGameManager() {
        GameManager.Instance.setSttKey(inputField.text);
    }
}
