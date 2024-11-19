using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class WitSTT : MonoBehaviour
{
    //bool isAccept = true;
    [SerializeField] GameObject AcceptButton, RejectButton, STTUI;
    private string textOfButton, resultText;
    private AudioClip recordedClip;
    private string Key = "";
    private bool isRecording = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSTTui(bool isAccept) {
        //this.isAccept = isAccept;

        if (isAccept)
        {
            textOfButton = AcceptButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }
        else {
            textOfButton = RejectButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }

        STTUI.SetActive(true);
    }

    public void StartRecording() { 
    
    }

    public void StopRecording() {
        if (!isRecording) return;
        byte[] WavClip = ConvertToWAV.AudioClipToWav(recordedClip);
        StartCoroutine(RunSTT(WavClip));
    }

    private IEnumerator RunSTT(byte[] wavData) {

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", wavData);

        UnityWebRequest www = UnityWebRequest.Post("https://api.wit.ai/dictation", form);
        www.SetRequestHeader("Authorization", "Bearer" + Key);
        www.SetRequestHeader("Content-Type", "audio/wav");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string result = www.downloadHandler.text;
            int compareResult = CompareSentence();
            if (compareResult == 0) { 
            
            }
        }
        else { 
            //다시 하라고 하기? 오류가 발생했습니다.
        }
    }

    public int CompareSentence() { //Levenshtein
        int[,] d = new int[textOfButton.Length + 1, resultText.Length + 1];

        for (int i = 0; i <= textOfButton.Length; i++)
            d[i, 0] = i;

        for (int j = 0; j <= resultText.Length; j++)
            d[0, j] = j;

        for (int i = 1; i <= textOfButton.Length; i++) {
            for (int j = 1; j <= resultText.Length; j++) {
                int cost = textOfButton[i - 1] == resultText[j - 1] ? 0 : 1; //글자 자체를 비교
                d[i, j] = Mathf.Min(Mathf.Min(d[i-1,j]+1, d[i, j-1]+1), d[i-1, j-1] + cost); //삽입, 수정, 삭제 중 최소
            }
        }

        return d[textOfButton.Length, resultText.Length];
    }

    public void CloseSTTui() { 
    
    }
}
