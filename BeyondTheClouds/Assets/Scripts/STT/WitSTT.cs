using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class WitSTT : MonoBehaviour
{
    private string Key = "";

    //bool isAccept = true;
    [SerializeField] GameObject AcceptButton, RejectButton, STTUI;
    private string textOfButton, resultText;
    private AudioClip recordedClip;
    private bool isRecording = false;
    [SerializeField] TextMeshProUGUI textToRead;
    [SerializeField] GameObject recordingText, recordingEndText;
    private Coroutine sttCoroutine = null;

    private string recordingEnd = "문장을 다 읽었다면 마이크 버튼을 다시 눌러주세요.";
    private string success = "상대방에게 말이 제대로 전달되었어요! 이제 녹음 창을 닫아주세요.";
    private string fail = "상대방에게 말이 제대로 전달되지 않은 것 같아요. 다시 시도해주세요.";


    public void SetSTTui(bool isAccept) {
        //this.isAccept = isAccept;

        if (isAccept)
        {
            textOfButton = AcceptButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }
        else {
            textOfButton = RejectButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }

        textToRead.text = textOfButton;
        recordingEndText.GetComponent<TextMeshProUGUI>().text = recordingEnd;
        STTUI.SetActive(true);
    }

    public void RecordingButton() {
        if (isRecording)
        {
            Microphone.End(null);
            recordingText.SetActive(false);
            byte[] WavClip = ConvertToWAV.AudioClipToWav(recordedClip);
            isRecording = false;
            if (sttCoroutine != null) {
                StopCoroutine(sttCoroutine);
                sttCoroutine = StartCoroutine(RunSTT(WavClip));
            }
        }
        else {
            isRecording = true;
            recordedClip = Microphone.Start(null, false, 60, 16000); //null이면 기본 마이크 사용
            Invoke("StopRecordingAuto", 60);
            recordingEndText.GetComponent<TextMeshProUGUI>().text = recordingEnd;
            recordingText.SetActive(true);
        }
    }

    public void StopRecordingAuto() {
        Microphone.End(null);
        recordingText.SetActive(false);
        byte[] WavClip = ConvertToWAV.AudioClipToWav(recordedClip);
        isRecording = false;
        if (sttCoroutine != null)
        {
            StopCoroutine(sttCoroutine);
            sttCoroutine = StartCoroutine(RunSTT(WavClip));
        }
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
            if (compareResult == 0)
            {
                recordingEndText.GetComponent<TextMeshProUGUI>().text = success;
            }
            else {
                recordingEndText.GetComponent<TextMeshProUGUI>().text = fail;
            }
        }
        else {
            Debug.LogError("STT 오류");
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

        return d[textOfButton.Length, resultText.Length]; //만약 결과에 띄어쓰기나 문장부호가 영향을 주는 경우 저거 전부 없애기
    }

    public void CloseSTTui() {
        STTUI.SetActive(false);
    }
}
