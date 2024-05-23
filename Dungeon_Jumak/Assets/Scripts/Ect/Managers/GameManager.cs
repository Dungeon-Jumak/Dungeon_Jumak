using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Data data;

    //---효과음---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private string pauseSound;

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
    SoundManager _soundManager = new SoundManager();//SoundManager 생성

    public static SceneManagerEx Scene { get { return Instance._sceneManager; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }

    void Start()
    {
        data = DataManager.Instance.data;

        audioManager = FindObjectOfType<AudioManager>();
        bgmManager = FindObjectOfType<BGMManager>();

        pauseSound = "pauseSound";
        _soundManager.Init();
    }

    public static void Clear()
    {
        Scene.Clear();
        Sound.Clear();
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

    public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        audioManager.AllStop();
        SceneManager.LoadScene(_sceneName);
    }
}
