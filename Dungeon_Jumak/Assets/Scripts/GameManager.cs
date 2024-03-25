using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private Data data;

    //---�ر� �� �ܻ� �迭---//
    [SerializeField]
    private GameObject[] Dansangs;

    //---��� ����---//
    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private int baseBGMTrackNum;

    //---ȿ����---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string pauseSound;

    // Start is called before the first frame update
    void Start()
    {
        data = DataManager.Instance.data;
        bgmManager = FindObjectOfType<BGMManager>();

        audioManager = FindObjectOfType<AudioManager>();
        pauseSound = "pauseSound";
    }

    // Update is called once per frame
    void Update()
    {
        unlockTable();

        if (!data.isPlayBGM)
        {
            data.isPlayBGM = true;
            bgmManager.Play(baseBGMTrackNum);
            bgmManager.FadeInMusic();
            bgmManager.SetLoop();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddTable();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddRecipe();
        }

        UpdateCoin();
        UpdateLevel();

    }

    //---���� �ε�� ������ ���� ���� �ر�---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            Dansangs[i].SetActive(true);
        }
    }

    //---�Ͻ� ���� ���---//
    public void Pause()
    {
        if (!data.isPause)
        {
            data.isPause = true;
            audioManager.Play(pauseSound);
            Time.timeScale = 0f;
        }
    }

    //---�Ͻ� ���� ��� ���---//
    public void Resume()
    {
        if (data.isPause)
        {
            data.isPause = false;
            audioManager.Play(pauseSound);
            Time.timeScale = 1f;
        }
    }

    public void AddTable()
    {
        if (data.curUnlockLevel < data.maxUnlockLevel)
        {
            audioManager.Play(pauseSound);
            data.curUnlockLevel++;
            data.maxSeatSize += 2;
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

    // --- ���� ���� �Լ� --- //
    public void UpdateLevel()
    {
        GameObject.Find("UI_LevelText").GetComponent<TextMeshProUGUI>().text = data.curPlayerLV.ToString();
    }

    // --- ���� ���� --- //
    public void UpdateCoin()
    {
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = data.curCoin.ToString();
    }
}
