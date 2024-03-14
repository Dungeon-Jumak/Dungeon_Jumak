using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    
    public AudioMixer audioMixer;

    public Slider BGMSlider;
    public Slider SFXSlider;

    public void BGMControl()
    {
        float sound = BGMSlider.value;

        if (sound == -40f) audioMixer.SetFloat("BGM", -80);
        else audioMixer.SetFloat("BGM", sound);
    }

    public void SFXControl()
    {
        float sound = SFXSlider.value;

        if (sound == -40f) audioMixer.SetFloat("SFX", -80);
        else audioMixer.SetFloat("SFX", sound);
    }

    public void BGMToggleVolume()
    {
        float sound = BGMSlider.value;
        sound = sound == 0 ? 1 : 0;

        audioMixer.SetFloat("BGM", sound);
    }

    public void SFXToggleVolume()
    {
        float sound = SFXSlider.value;
        sound = sound == 0 ? 1 : 0;

        audioMixer.SetFloat("SFX", sound);
    }


}
