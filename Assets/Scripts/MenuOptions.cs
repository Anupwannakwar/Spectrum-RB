using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    public Slider VolumeBar;
    public Toggle Mute;

    private void Update()
    {
        if (Mute.isOn)
        {
            PlayerPrefs.SetFloat("MusicVolume", 0);
            PlayerPrefs.SetFloat("EffectsVolume", 0);
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", VolumeBar.value);
            PlayerPrefs.SetFloat("EffectsVolume", VolumeBar.value);
        }

        SoundManager.instance.effectVolumeChanged();
        SoundManager.instance.musicVolumeChanged();
    }
}
