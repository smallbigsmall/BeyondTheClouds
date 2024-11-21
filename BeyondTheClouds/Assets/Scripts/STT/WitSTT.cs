using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class WitSTT : MonoBehaviour
{
    private string Key = "";

    [SerializeField] GameObject AcceptButton, RejectButton, STTUI;
    private string textOfButton, resultText;
    private AudioClip recordedClip;
    private bool isRecording = false, conversationSuccess = false, isAcceptConversation = false;
    [SerializeField] TextMeshProUGUI textToRead;
    [SerializeField] GameObject recordingText, recordingEndText;
    private Coroutine sttCoroutine = null, timerCoroutine = null;
    [SerializeField] Button recordingButton;

    private string recordingEnd = "������ �� �о��ٸ� ����ũ ��ư�� �ٽ� �����ּ���.";
    private string success = "���濡�� ���� ����� ���޵Ǿ����!\n���� ���� â�� �ݾ��ּ���.";
    private string fail = "���濡�� ���� ����� ���޵��� ���� �� ���ƿ�.\n�ٽ� �õ����ּ���.";
    private string transcripting = "������ �Ϸ�Ǿ����ϴ�. �����ν���...";
    private string warning = "�����ν� ����. �Է��� key�� ��ȿ���� Ȯ�����ּ���.";

    public void SetSTTui(bool isAccept) {

        Key = GameManager.Instance.GetSttKey();
        isAcceptConversation = isAccept;

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
        recordingButton.interactable = true;
    }

    public void RecordingButton() {
        if (isRecording)
        {
            StopCoroutine(timerCoroutine);
            Microphone.End(null);
            recordingText.SetActive(false);
            byte[] WavClip = ConvertToWAV.AudioClipToWav(recordedClip);
            isRecording = false;
            if (sttCoroutine != null) {
                StopCoroutine(sttCoroutine);
            }
            sttCoroutine = StartCoroutine(RunSTT(WavClip));
        }
        else {
            isRecording = true;
            recordedClip = Microphone.Start(null, false, 60, 16000); //null�̸� �⺻ ����ũ ���
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(countRecording());
            recordingEndText.GetComponent<TextMeshProUGUI>().text = recordingEnd;
            recordingText.SetActive(true);
        }
    }

    IEnumerator countRecording() {
        yield return new WaitForSeconds(60);
        StopRecordingAuto();
    }

    public void StopRecordingAuto() {
        Microphone.End(null);
        recordingText.SetActive(false);
        byte[] WavClip = ConvertToWAV.AudioClipToWav(recordedClip);
        isRecording = false;
        if (sttCoroutine != null)
        {
            StopCoroutine(sttCoroutine);
        }
        sttCoroutine = StartCoroutine(RunSTT(WavClip));
    }

    private IEnumerator RunSTT(byte[] wavData) {
        recordingEndText.GetComponent<TextMeshProUGUI>().text = transcripting;

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", wavData);

        UnityWebRequest www = new UnityWebRequest("https://api.wit.ai/dictation", "POST");
        www.uploadHandler = new UploadHandlerRaw(wavData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Authorization", "Bearer " + Key);
        www.SetRequestHeader("Content-Type", "audio/wav");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string result = www.downloadHandler.text;
            resultText = ParsingJson(result);
            int compareResult = CompareSentence();
            if (compareResult < textOfButton.Length/3)
            {
                recordingEndText.GetComponent<TextMeshProUGUI>().text = success;
                recordingButton.interactable = false;
                conversationSuccess = true;
            }
            else {
                recordingEndText.GetComponent<TextMeshProUGUI>().text = fail;
                conversationSuccess = false;
            }
        }
        else {
            Debug.LogError(www.error);
            recordingEndText.GetComponent<TextMeshProUGUI>().text = warning;
        }
    }

    public int CompareSentence() { //Levenshtein

        textOfButton = Regex.Replace(textOfButton, @"[\s\p{P}]", "");
        resultText = Regex.Replace(resultText, @"[\s\p{P}]", "");
        Debug.Log(textOfButton + "        " + resultText);

        int[,] d = new int[textOfButton.Length + 1, resultText.Length + 1];

        for (int i = 0; i <= textOfButton.Length; i++)
            d[i, 0] = i;

        for (int j = 0; j <= resultText.Length; j++)
            d[0, j] = j;

        for (int i = 1; i <= textOfButton.Length; i++) {
            for (int j = 1; j <= resultText.Length; j++) {
                int cost = textOfButton[i - 1] == resultText[j - 1] ? 0 : 1; //���� ��ü�� ��
                d[i, j] = Mathf.Min(Mathf.Min(d[i-1,j]+1, d[i, j-1]+1), d[i-1, j-1] + cost); //����, ����, ���� �� �ּ�
            }
        }

        return d[textOfButton.Length, resultText.Length]; //���� ����� ���⳪ �����ȣ�� ������ �ִ� ��� ���� ���� ���ֱ�
    }

    public void CloseSTTui() {

        if (conversationSuccess)
        {
            if (isAcceptConversation)
            {
                GameObject.FindWithTag("Player").transform.GetChild(1).GetComponent<NPCQuest>().AcceptQuest();
            }
            else {
                GameObject.FindWithTag("Player").transform.GetChild(1).GetComponent<NPCQuest>().RejectQuest();
            }
            
        }
        STTUI.SetActive(false);
    }

    private string ParsingJson(string resultData) {

        string[] jsonArray = resultData.Split(new[] { "}\n{", "}\r{", "}\r\n{" }, System.StringSplitOptions.None);

        jsonArray[jsonArray.Length - 1] = "{" + jsonArray[jsonArray.Length - 1];
        string json = jsonArray[jsonArray.Length - 1];
        SttResult.Transcription transcription = JsonUtility.FromJson<SttResult.Transcription>(json);
        Debug.Log(transcription.text);
        return transcription.text;
    }
}
