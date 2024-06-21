using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Data.cs
    [SerializeField]
    private Data data;

    //GameManager.cs instance variable
    private static GameManager instance;

    private TextMeshProUGUI timeDisplayTmp;

    private float timer;
    private float gameSecondsPerRealSecond = 3 * 60f; //3 minutes per second
    private float secondsInADay = 24 * 60 * 60;

    //Setting GameManager.cs to Singleton system 
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            timer = 6 * 60 * 60;
            data = DataManager.Instance.data;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameObject dayTimerTextObject = GameObject.Find("DayTimer_Text");
        if (dayTimerTextObject != null)
        {
            timeDisplayTmp = dayTimerTextObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("");
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
    SceneManagerEx _sceneManager = new SceneManagerEx();//SceneManagerEx »ý¼º
    ResourceManager _resource = new ResourceManager();//ResourceManager »ý¼º
    SoundManager _soundManager = new SoundManager();//SoundManager »ý¼º
    UIManager _ui = new UIManager();//UIManager »ý¼º

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

    private void Update()
    {
        if (data.timerStart)
        {
            timer += Time.deltaTime * gameSecondsPerRealSecond;

            //24 hours (86400 seconds) are exceeded, the timer returns to 0.
            if (timer >= secondsInADay)
            {
                timer -= secondsInADay;
            }

            DisplayTime();
        }
    }

    private void DisplayTime()
    {
        int totalMinutes = Mathf.FloorToInt(timer / 60f);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        string timeText = string.Format("{0:D2}:{1:D2}", hours, minutes);
        timeDisplayTmp.text = timeText;

        if (hours >= 0 && hours < 6)
        {
            Debug.Log("얼렁 자라!!");
        }

        if (hours >= 6 && minutes > 0 && minutes < 4)
        {
            data.dayCount = true;
        }
    }

    public void ResetTimer()
    {
        timer = 6 * 60 * 61;
    }
}