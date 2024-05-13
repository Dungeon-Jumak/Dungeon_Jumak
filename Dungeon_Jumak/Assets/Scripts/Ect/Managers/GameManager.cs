using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //---GameManager.cs �̱��� ����---//
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

    SceneManagerEx _sceneManager = new SceneManagerEx();//SceneManagerEx ����
    ResourceManager _resource = new ResourceManager();//ResourceManager ����

    public static SceneManagerEx Scene { get { return instance._sceneManager; } }
    public static ResourceManager Resource { get { return instance._resource; } }
   

    [SerializeField] 
    private Data data;

    //---ȿ����---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private BGMManager bgmManager;
    [SerializeField]
    private string pauseSound;

    // Start is called before the first frame update
    void Start()
    {
        data = DataManager.Instance.data;

        audioManager = FindObjectOfType<AudioManager>();
        bgmManager = FindObjectOfType<BGMManager>();

        pauseSound = "pauseSound";

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

    public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        audioManager.AllStop();
        SceneManager.LoadScene(_sceneName);
    }
}