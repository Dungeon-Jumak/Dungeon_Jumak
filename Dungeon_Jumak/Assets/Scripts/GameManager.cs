using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private Data data;

    //---해금 할 단상 배열---//
    [SerializeField]
    private GameObject[] Dansangs;

    //---배경 음악---//
    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private int baseBGMTrackNum;

    //---효과음---//
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
            if(data.curUnlockLevel < data.maxUnlockLevel) 
            {
                data.curUnlockLevel++;
                data.maxSeatSize += 2;
            }

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(data.curMenuUnlockLevel < data.maxMenuUnlockLevel)
                data.curMenuUnlockLevel++;
        }

    }

    //---게임 로드시 데이터 값에 따라 해금---//
    void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            Dansangs[i].SetActive(true);
        }
    }

    //---일시 정지 기능---//
    public void Pause()
    {
        if (!data.isPause)
        {
            data.isPause = true;
            audioManager.Play(pauseSound);
            Time.timeScale = 0f;
        }
    }

    //---일시 정지 취소 기능---//
    public void Resume()
    {
        if (data.isPause)
        {
            data.isPause = false;
            audioManager.Play(pauseSound);
            Time.timeScale = 1f;
        }
    }

}
