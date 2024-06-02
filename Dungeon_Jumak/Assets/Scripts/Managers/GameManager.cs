using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Data.cs
    [SerializeField]
    private Data data;

    //GameManager.cs instance variable
    private static GameManager instance;

    //Setting GameManager.cs to Singleton system 
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

    //Setting Get function
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

    /// <summary>
    /// Create Manager Script Variable
    /// </summary>
    SceneManagerEx _sceneManager = new SceneManagerEx();//SceneManagerEx 持失
    ResourceManager _resource = new ResourceManager();//ResourceManager 持失
    SoundManager _soundManager = new SoundManager();//SoundManager 持失
    UIManager _ui = new UIManager();//UIManager 持失

    public static SceneManagerEx Scene { get { return Instance._sceneManager; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static UIManager UI { get { return Instance._ui; } }

    void Start()
    {
        data = DataManager.Instance.data;//Data.cs

        _soundManager.Init();//Init the SoundManager.cs
    }

    //Clear function to control scene and sound memory system when convert current scene
    public static void Clear()
    {
        Scene.Clear();
        Sound.Clear();
    }

    //Pause System
    public void Pause()
    {
        if (!data.isPause)
        {
            data.isPause = true;
            Time.timeScale = 0f;
        }
    }

    //Cancel the pause system
    public void Resume()
    {
        if (data.isPause)
        {
            data.isPause = false;
            Time.timeScale = 1f;
        }
    }

    /*public void ConvertScene(string _sceneName)
    {
        bgmManager.Stop();
        audioManager.AllStop();
        SceneManager.LoadScene(_sceneName);
    }*/
}
