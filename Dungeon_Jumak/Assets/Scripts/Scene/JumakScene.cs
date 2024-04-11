using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JumakScene : BaseScene
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
    private FadeController fadeController;

    //---해금 할 단상 배열---//
    [SerializeField]
    private GameObject[] Dansangs;

    private void Start()
    {
        playBGM = false;

        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();
        audioManager = FindObjectOfType<AudioManager>();
        fadeController = FindObjectOfType<FadeController>();

        //---기본 BGM 실행---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();
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
        UpdateLevel();
        unlockTable();

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
    public void BGMON()
    {
        data.isPlayBGM = true;
    }

    public void BGMOFF()
    {
        data.isPlayBGM = false;
    }

    public override void Clear()
    {

    }

    // --- 레벨 변경 함수 --- //
    public void UpdateLevel()
    {
        GameObject.Find("UI_LevelText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curPlayerLV.ToString();
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

    //---게임 로드시 데이터 값에 따라 해금---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            if (Dansangs[i] != null) // 해당 게임 오브젝트가 파괴되지 않았는지 확인
            {
                Dansangs[i].SetActive(true);
            }
        }
    }
}
