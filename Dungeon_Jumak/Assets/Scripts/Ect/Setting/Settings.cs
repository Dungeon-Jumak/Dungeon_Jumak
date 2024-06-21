//System
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//Unity
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Settings : MonoBehaviour
{
    [Header("����� ON ���� ��� ������Ʈ")]
    [SerializeField] private GameObject musicOnToggle;
    [Header("����� OFF ���� ��� ������Ʈ")]
    [SerializeField] private GameObject musicOffToggle;

    [Header("ȿ���� ON ���� ��� ������Ʈ")]
    [SerializeField] private GameObject effectOnToggle;
    [Header("ȿ���� ON ���� ��� ������Ʈ")]
    [SerializeField] private GameObject effectOffToggle;

    [Header("BGM �ͼ�")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("�Ҹ� ũ�� �����̴�")]
    [SerializeField] private Slider audioSlider;

    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;

        audioMixer.GetFloat("Master", out data.pitch);

        audioSlider.value = data.pitch;
    }

    private void Update()
    {

        data.pitch = audioSlider.value;

        audioMixer.SetFloat("Master", audioSlider.value);

        if(audioSlider.value == -80f)
        {
            MusicOFF();
            EffectOFF();
        }

        if (data.isPlayBGM) 
        {
            musicOnToggle.SetActive(true);
            musicOffToggle.SetActive(false);
        }
        else
        {
            musicOnToggle.SetActive(false);
            musicOffToggle.SetActive(true);
        }

        if (data.isSound)
        {
            effectOnToggle.SetActive(true);
            effectOffToggle.SetActive(false);
        }
        else
        {
            effectOnToggle.SetActive(false);
            effectOffToggle.SetActive(true);
        }
    }

    //Music On
    public void MusicON()
    {
        data.isPlayBGM = true;

        //On Toggle Active
        musicOnToggle.SetActive(true);

        //Off Toggle DeActive
        musicOffToggle.SetActive(false);

        audioMixer.SetFloat("BGM", 0f);
    }

    //Music Off
    public void MusicOFF()
    {
        data.isPlayBGM = false;

        //Off Toggle Active
        musicOffToggle.SetActive(true);

        //On Toggle DeActive
        musicOnToggle.SetActive(false);

        audioMixer.SetFloat("BGM", -80f);
    }

    //Effect On
    public void EffectON()
    {
        data.isSound = true;

        effectOnToggle.SetActive(true);

        //Off Toggle DeActive
        effectOffToggle.SetActive(false);

        audioMixer.SetFloat("SFX", 0f);
    }

    public void EffectOFF()
    {
        data.isSound = false;

        //Off Toggle Active
        effectOffToggle.SetActive(true);

        //On Toggle DeActive
        effectOnToggle.SetActive(false);

        audioMixer.SetFloat("SFX", -80f);
    }


}
