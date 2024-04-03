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
    UIManager _uiManager = new UIManager();

    public static SceneManagerEx Scene { get { return instance._sceneManager; } }
    public static ResourceManager Resource { get { return instance._resource; } }
    
    public static UIManager UI { get { return instance._uiManager; } }

    [SerializeField] 
    private Data data;

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
