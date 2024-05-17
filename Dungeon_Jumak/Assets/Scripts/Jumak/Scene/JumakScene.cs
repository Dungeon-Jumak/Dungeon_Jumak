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

    [SerializeField]
    int compareTotalPrice;

    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject[] panelForStart;

    [SerializeField]
    private TextMeshProUGUI coinTMP;

    [SerializeField]
    private TextMeshProUGUI dayTMP;


    private bool playBGM = false;
    private bool endPanel = false;


    private void Start()
    {
        data.gukbapCount = 0;
        data.pajeonCount = 0;
        data.riceJuiceCount = 0;
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

        switch (data.backgroundLevel) 
        {
            case 0:
                GameObject.Find("Level1_Backgr").gameObject.SetActive(false);
                GameObject.Find("Level2_Backgr").gameObject.SetActive(true); 
                break;
            case 1:
                GameObject.Find("Level2_Backgr").gameObject.SetActive(false);
                GameObject.Find("Level3_Backgr").gameObject.SetActive(true);
                break;
        }

        switch (data.chairLevel)
        {
            case 0:
                GameObject.Find("Level1_Chair").gameObject.SetActive(false);
                GameObject.Find("Level2_Chair").gameObject.SetActive(true);
                break;
            case 1:
                GameObject.Find("Level2_Chair").gameObject.SetActive(false);
                GameObject.Find("Level3_Chair").gameObject.SetActive(true);
                break;
        }

        switch (data.tableLevel)
        {
            case 0:
                GameObject.Find("Level1_Table").gameObject.SetActive(false);
                GameObject.Find("Level2_table").gameObject.SetActive(true);
                break;
            case 1:
                GameObject.Find("Level2_Table").gameObject.SetActive(false);
                GameObject.Find("Level3_Table").gameObject.SetActive(true);
                break;
        }
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
        coinTMP.text = data.curCoin.ToString() + "전";
        dayTMP.text = data.days.ToString() + " 일차";

        if (isStart)
            timer += Time.deltaTime;

        if (timer >= duration)
        {
            isStart = false;

            timer -= duration;
            JumakOff();
        }

        int newTime = Mathf.FloorToInt(duration - timer);
        timerTxt.text = newTime.ToString();

        if (endPanel && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)))
        {
            SceneManager.LoadScene("WaitingScene");
        }

    }

    private void JumakOff()
    {
        for (int i = 0; i < JumakSystemObj.Length; i++)
        {
            JumakSystemObj[i].SetActive(false);
        }
        receiptPopup.SetActive(true);

        StartCoroutine(ActivateTextSequentially());

        data.currentTotalPrice = (data.gukbapCount * data.nowGukbapPrice) + (data.riceJuiceCount * data.nowRiceJuicePrice) + (data.pajeonCount * data.nowPajeonPrice);
        audioManager.AllStop();
    }

    private IEnumerator ActivateTextSequentially()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Find("GukBap_Recipt").GetComponent<TextMeshProUGUI>().text = "국밥 x " + data.gukbapCount.ToString() + " = " + data.gukbapCount * data.nowGukbapPrice;
        yield return new WaitForSeconds(1f);

        GameObject.Find("Pajeon_Recipt").GetComponent<TextMeshProUGUI>().text = "파전 x " + data.pajeonCount.ToString() + " = " + data.pajeonCount * data.nowPajeonPrice;
        yield return new WaitForSeconds(1f);

        GameObject.Find("RiceJuice_Recipt").GetComponent<TextMeshProUGUI>().text = "식혜 x " + data.riceJuiceCount.ToString() + " = " + data.riceJuiceCount * data.nowRiceJuicePrice;

        yield return new WaitForSeconds(1f);
        GameObject.Find("Total_Recipt").GetComponent<TextMeshProUGUI>().text = "총 매출 = " + data.currentTotalPrice.ToString() + "전";

        yield return new WaitForSeconds(1f);

        /*
        compareTotalPrice = data.currentTotalPrice - data.yesterdayTotalPrice;
        if (compareTotalPrice > 0)
        {
            GameObject.Find("Compare_Recipt").GetComponent<TextMeshProUGUI>().text = compareTotalPrice.ToString() + "↑";
        }
        else
        {
            ameObject.Find("Compare_Recipt").GetComponent<TextMeshProUGUI>().text = compareTotalPrice.ToString() + "↓";
        }
        data.yesterdayTotalPrice = data.currentTotalPrice;
        */
        endPanel = true;
    }

    public void JumakStart()
    {
        isStart = true;

        data.days++;

        startPanel.SetActive(false);
        for (int i = 0; i < panelForStart.Length; i++)
        {
            panelForStart[i].SetActive(true);
        }
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
        //GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString() + "전";
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

    public void InitialTransfrom(GameObject go)
    {
        go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, go.transform.localPosition.z);
    }
}