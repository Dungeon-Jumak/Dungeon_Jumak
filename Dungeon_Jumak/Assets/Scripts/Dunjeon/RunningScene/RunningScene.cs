using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunningScene : MonoBehaviour
{
    private Data data;

    private BGMManager bgmManager;
    private bool playBGM = false;

    [SerializeField]
    private int bgmSoundTrack;
    [SerializeField]
    private float maxVolume;

    private AudioManager audioManager;



    void Awake()
    {
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

    private void Start()
    {
        Invoke("PlayRunSound", 0.5f);
    }

    void Update()
    {
        BGMPlayer();
        SoundControl();

        if (data.playerHP == 0)
        {
            SceneManager.LoadScene("WaitingScene");
            data.isMonster = false;
        }
    }

    public void PlayRunSound()
    {
        audioManager.Play("runSound");
        audioManager.SetLoop("runSound");
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
