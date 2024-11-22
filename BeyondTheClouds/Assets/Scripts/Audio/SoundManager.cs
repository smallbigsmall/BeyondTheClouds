using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> BGM_List, NightBGM_List;
    [SerializeField] AudioClip Ambience_Bird, Ambience_Cicada;
    [SerializeField] AudioClip SFX_Rain, SFX_Thunder, SFX_Quest;
    [SerializeField] AudioSource BgmAudioSource, SfxAudioSource, AmbienceAudioSource;

    private int currentDay = 0;
    private bool isDayCleared = false;

    void Start()
    {
        if (!isDayCleared)
        {
            Invoke("StartBGM", 6);
        }
        else {
            StartBGM();
        }
    }

    public void StartBGM() {
        if (!isDayCleared)
        {
            if (currentDay == 1)
            {
                AmbienceAudioSource.PlayOneShot(Ambience_Bird);
                Invoke("StartDay1Bgm", 20);
            }
            else
            {
                int value = Random.Range(0, 3);
                BgmAudioSource.clip = BGM_List[value];
                BgmAudioSource.Play();

                int ambienceRandomValue = Random.Range(0, 3);
                if (ambienceRandomValue == 0) AmbienceAudioSource.PlayOneShot(Ambience_Bird);
                else if (ambienceRandomValue == 1) AmbienceAudioSource.PlayOneShot(Ambience_Cicada);
            }
        }
        else {
            int value = Random.Range(0, 3);
            BgmAudioSource.clip = NightBGM_List[value];
            BgmAudioSource.Play();
        }
    }

    public void StartDay1Bgm() {
        BgmAudioSource.clip = BGM_List[0];
        BgmAudioSource.Play();
    }

    public void SetDay(int day, bool cleared) {
        currentDay = day;
        isDayCleared = cleared;
    }

    public void playQuestSound() {
        SfxAudioSource.PlayOneShot(SFX_Quest);
    }
}
