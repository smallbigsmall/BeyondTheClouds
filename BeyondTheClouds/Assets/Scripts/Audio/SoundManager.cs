using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> BGM_List;
    [SerializeField] AudioClip Ambience_Bird, Ambience_Cicada, Ambience_Fire;
    [SerializeField] AudioClip SFX_Rain, SFX_Thunder;
    [SerializeField] AudioSource BgmAudioSource, SfxAudioSource;

    private int currentDay = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartBGM() {
        if (currentDay == 1) {
            BgmAudioSource.PlayOneShot(Ambience_Bird);
        }
    }

    public void SetDay(int day) {
        currentDay = day;
    }
}
