using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //---GameManager.cs 싱글톤 선언---//
    private static GameManager instance;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    SceneManagerEx _sceneManager = new SceneManagerEx();//SceneManagerEx 생성
    ResourceManager _resource = new ResourceManager();//ResourceManager 생성

    public static SceneManagerEx Scene { get { return instance._sceneManager; } }
    public static ResourceManager Resource { get { return instance._resource; } }

    [SerializeField] 
    private Data data;

    //---해금 할 단상 배열---//
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
        //unlockTable();

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
    }

    //---게임 로드시 데이터 값에 따라 해금---//
   /* void unlockTable()
    {
        for (int i = 0; i < data.curUnlockLevel; i++)
        {
            if (Dansangs[i] != null) // 해당 게임 오브젝트가 파괴되지 않았는지 확인
            {
                Dansangs[i].SetActive(true);
            }
        }
    }*/

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

}
