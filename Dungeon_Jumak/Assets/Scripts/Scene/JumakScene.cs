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

    //---�ر� �� �ܻ� �迭---//
    [SerializeField]
    private GameObject[] Dansangs;

    private void Start()
    {
        playBGM = false;

        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();
        audioManager = FindObjectOfType<AudioManager>();
        fadeController = FindObjectOfType<FadeController>();

        //---�⺻ BGM ����---//
        bgmManager.Play(bgmSoundTrack);
        bgmManager.FadeInMusic(maxVolume);
        bgmManager.SetLoop();
    }
    protected override void Init()
    {
        base.Init();

        //---Define.cs enum ���� ���� Scene ����---//
        SceneType = Define.Scene.Jumak;

        //---�� �ٽ� �ε� �Ǹ� �ٷ� Data�� coin �ε�---//
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

    // --- ���� ���� �Լ� --- //
    public void UpdateLevel()
    {
        GameObject.Find("UI_LevelText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curPlayerLV.ToString();
    }

    // --- ���� ���� --- //
    public void UpdateCoin()
    {
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.data.curCoin.ToString() + "��";
    }

    public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        audioManager.AllStop();
        SceneManager.LoadScene(_sceneName);
    }

    //---���� �ε�� ������ ���� ���� �ر�---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            if (Dansangs[i] != null) // �ش� ���� ������Ʈ�� �ı����� �ʾҴ��� Ȯ��
            {
                Dansangs[i].SetActive(true);
            }
        }
    }
}
