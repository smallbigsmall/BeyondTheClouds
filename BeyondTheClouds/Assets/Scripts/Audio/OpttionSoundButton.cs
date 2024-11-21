using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OpttionSoundButton : MonoBehaviour
{
    [SerializeField] Sprite SwitchWithCircle, SwitchWithoutCircle;
    [SerializeField] Toggle toggle;
    [SerializeField] Image backgroundImage;
    [SerializeField] AudioMixer audioMixer;
    public bool isBgmToggle = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMap"))
        {
            float value;
            if (isBgmToggle)
            {
                audioMixer.GetFloat("BGM", out value);
                if (value == 0) toggle.isOn = true;
                else toggle.isOn = false;
            }
            else
            {
                audioMixer.GetFloat("SFX", out value);
                if (value == 0) toggle.isOn = true;
                else toggle.isOn = false;
            }
        }
        toggleBackgroundImageChange();
    }

    public void toggleBackgroundImageChange() {
        if (toggle.isOn)
        {
            backgroundImage.sprite = SwitchWithoutCircle;
            backgroundImage.SetNativeSize();
            if (isBgmToggle)
            {
                audioMixer.SetFloat("BGM", 0);
            }
            else
            {
                audioMixer.SetFloat("SFX", 0);
            }
        }
        else
        {
            backgroundImage.sprite = SwitchWithCircle;
            backgroundImage.SetNativeSize();
            if (isBgmToggle)
            {
                audioMixer.SetFloat("BGM", -80);
            }
            else
            {
                audioMixer.SetFloat("SFX", -80);
            }
        }
    }
}
