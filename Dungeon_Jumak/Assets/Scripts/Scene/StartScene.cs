using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private int bgmSoundTrack;
    [SerializeField]
    private float maxVolume;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private Data data;

    private bool playBGM = false;

    [SerializeField]
    private GameObject bgmOn;
    [SerializeField]
    private GameObject bgmOff;

    [SerializeField]
    private GameObject soundOn;
    [SerializeField]
    private GameObject soundOff;

    // Start is called before the first frame update
    void Start()
    {
        playBGM = false;

        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();
        audioManager = FindObjectOfType<AudioManager>();

        //---BGM 사운드 트랙 설정---//
        bgmSoundTrack = 0;

        //---기본 BGM 실행---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();
    }

    // Update is called once per frame
    void Update()
    {
        BGMControl();
        SoundControl();
        BGMPlayer();


    }

    public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        audioManager.AllStop();
        SceneManager.LoadScene(_sceneName);
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

    public void BGMControl()
    {
        if (data.isPlayBGM)
        {
            bgmOn.SetActive(true);
            bgmOff.SetActive(false);
        }
        else
        {
            bgmOn.SetActive(false);
            bgmOff.SetActive(true);
        }
    }

    public void SoundControl()
    {
        if (data.isSound)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }

    public void BGMON()
    {
        data.isPlayBGM = true;
    }

    public void BGMOFF()
    {
        data.isPlayBGM = false;
    }

    public void SoundON()
    {
        data.isSound = true;
    }

    public void SoundOFF()
    {
        data.isSound = false;
    }

}
