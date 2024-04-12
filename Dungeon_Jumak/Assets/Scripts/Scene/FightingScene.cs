using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightingScene : MonoBehaviour
{
    private Data data;

    private BGMManager bgmManager;
    private bool playBGM = false;

    [SerializeField]
    private int bgmSoundTrack;
    [SerializeField]
    private float maxVolume;

    private AudioManager audioManager;

    [SerializeField]
    private Animator winAnimator;
    [SerializeField]
    private Animator defeatAnimator;

    private bool isCoroutine;

    void Awake()
    {
        isCoroutine = false;
        playBGM = false;
        data = DataManager.Instance.data;

        bgmManager = FindObjectOfType<BGMManager>();
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.AllStop();
        bgmManager.Stop();

        //---기본 BGM 실행---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();
    }

    void Update()
    {
        BGMPlayer();
        SoundControl();

        if (data.monsterHP == 0)
        {
            for (int i = 0; i < data.monsterSpawn.Length; i++)
            {
                data.monsterSpawn[i] = false;
            }

            if(!isCoroutine) StartCoroutine(WinCoroutine());
        }
        else if(data.playerHP == 0)
        {
            if(!isCoroutine) StartCoroutine(DefeatCoroutine());
        }
    }

    IEnumerator WinCoroutine()
    {
        isCoroutine = true;

        if (data.isSound)
            audioManager.Play("battleWinSound");

        winAnimator.SetTrigger("notice");

        yield return new WaitForSeconds(4f);

        audioManager.Stop("battleWinSound");

        if (data.isThirdMonster == true)
        {
            SceneManager.LoadScene("WaitingScene");
        }
        else
        {
            SceneManager.LoadScene("RunningScene");
        }
    }

    IEnumerator DefeatCoroutine()
    {
        isCoroutine = true;

        if(data.isSound)
            audioManager.Play("battleDefeatSound");

        defeatAnimator.SetTrigger("notice");

        yield return new WaitForSeconds(4f);

        audioManager.Stop("battleDefeatSound");

        SceneManager.LoadScene("WaitingScene");
    }

    public void BGMPlayer()
    {
        if (!data.isPlayBGM)
        {
            playBGM = false;

            bgmManager.CancelLoop();
            bgmManager.Stop();
        }

        if (!playBGM && data.isPlayBGM)
        {
            playBGM = true;

            bgmManager.Play(bgmSoundTrack);
            bgmManager.FadeInMusic(maxVolume);
            bgmManager.SetLoop();
        }
    }

    public void SoundControl()
    {
        if (data.isSound)
        {
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = 1f;
                audioManager.sounds[i].Setvolume();
            }
        }
        else
        {
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = 0f;
                audioManager.sounds[i].Setvolume();
            }
        }
    }
}
