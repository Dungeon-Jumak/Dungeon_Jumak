using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class JumakScene : BaseScene
{
    public bool isStart;

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

    [SerializeField]
    private FadeController fadeController;

    //---해금 할 단상 배열---//
    [SerializeField]
    private GameObject[] Dansangs;

    [SerializeField]
    private string pauseSound;

    [SerializeField]
    private GameObject bgmOn;
    [SerializeField]
    private GameObject bgmOff;

    [SerializeField]
    private GameObject soundOn;
    [SerializeField]
    private GameObject soundOff;

    [SerializeField]
    private GameObject[] JumakSystemObj;

    [SerializeField]
    private GameObject receiptPopup;

    [SerializeField] private float timer;
    [SerializeField] private float duration;

    [SerializeField]
    private TextMeshProUGUI timerTxt;

    private bool playBGM = false;


    private void Start()
    {
        playBGM = false;

        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();
        audioManager = FindObjectOfType<AudioManager>();
        fadeController = FindObjectOfType<FadeController>();

        for (int i = 0; i < data.onTables.Length; i++)
        {
            data.isAllocated[i] = false;
            data.isCustomer[i] = false;
            data.onTables[i] = false;
            data.isFinEat[i] = false;
        }

        bgmManager.Stop();

        data.curSeatSize = 0;

        //---기본 BGM 실행---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();

        pauseSound = "pauseSound";

        timer = 0;
        isStart = false;
    }
    protected override void Init()
    {
        base.Init();

        //---Define.cs enum 통한 현재 Scene 설정---//
        SceneType = Define.Scene.Jumak;

        //---씬 다시 로드 되면 바로 Data의 coin 로드---//
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString();
    }

    public void Update()
    {
        UpdateCoin();
        BGMControl();
        SoundControl();

        BGMPlayer();

        if(isStart)
            timer += Time.deltaTime;

        if(timer >= duration)
        {
            isStart = false;

            timer -= duration;
            JumakOff();
        }

        int newTime = Mathf.FloorToInt(duration - timer);
        timerTxt.text = newTime.ToString();
    }

    private void JumakOff()
    {
        for (int i = 0; i < JumakSystemObj.Length; i++)
        {
            JumakSystemObj[i].SetActive(false);
        }
        receiptPopup.SetActive(true);

        audioManager.AllStop();
    }

    public void JumakStart()
    {
        isStart = true;
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

    public void AddRecipe()
    {
        if (data.curMenuUnlockLevel < data.maxMenuUnlockLevel)
        {
            audioManager.Play(pauseSound);
            data.curMenuUnlockLevel++;
        }

    }
    public override void Clear()
    {

    }

    // --- 코인 변경 --- //
    public void UpdateCoin()
    {
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString() + "전";
    }

    public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        audioManager.AllStop();
        SceneManager.LoadScene(_sceneName);
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

            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = 1f;
                audioManager.sounds[i].Setvolume();
            }
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);

            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = 0f;
                audioManager.sounds[i].Setvolume();
            }
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
