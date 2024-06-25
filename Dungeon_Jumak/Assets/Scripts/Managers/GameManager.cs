//System
using System.Collections;
using System.Collections.Generic;
using System.Resources;

//TMPro
using TMPro;

//Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //Data.cs
    [SerializeField]
    private Data data;

    //GameManager.cs instance variable
    private static GameManager instance;

    //Coin System
    [Header("코인 텍스트를 불러오기 위한 오브젝트")]
    [SerializeField] private GameObject coinObj;

    private int lastCoin;

    //Level System
    [Header("레벨 텍스트를 불러오기 위한 오브젝트")]
    [SerializeField] private GameObject levelObj;

    private int lastLevel;

    //Timer System
    [Header("타이머 텍스트를 불러오기 위한 오브젝트")]
    [SerializeField] private GameObject timerObj;

    [Header("타이머 오브젝트")]
    [SerializeField] private GameObject timer;

    private float gameSecondsPerRealSecond = 3 * 60f; //3 minutes per second
    private float secondsInADay = 24 * 60 * 60;
    private bool IsDay = true;

    public string timerText;

    //Setting GameManager.cs to Singleton system 
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            data = DataManager.Instance.data;
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

    #region Create Manger Script Variable

    /// <summary>
    /// Create Manager Script Variable
    /// </summary>
    SceneManagerEx _sceneManager = new SceneManagerEx();//SceneManagerEx 
    ResourceManager _resource = new ResourceManager();//ResourceManager 
    SoundManager _soundManager = new SoundManager();//SoundManager 
    UIManager _ui = new UIManager();//UIManager 

    public static SceneManagerEx Scene { get { return Instance._sceneManager; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static UIManager UI { get { return Instance._ui; } }

    #endregion

    public void Start()
    {
        data = DataManager.Instance.data;//Data.cs

        lastCoin = data.curCoin;
        lastLevel = data.curPlayerLV;

        _soundManager.Init();//Init the SoundManager.cs
    }

    private void Update()
    {
        //Level System
        LevelSystem();

        //Coin System
        CoinSystem();

        //Game Timer System
        GameTimerSystem();
    }

    #region Level System

    private void LevelSystem()
    {
        if (data.timerStart)
        {
            if (levelObj == null)
            {
                levelObj = GameObject.Find("[Text] Level");

                if (levelObj != null)
                    levelObj.GetComponent<TextMeshProUGUI>().text = "LV." + data.curPlayerLV.ToString();
            }

            if (lastLevel != data.curPlayerLV)
            {
                lastLevel = data.curPlayerLV;
                levelObj.GetComponent<TextMeshProUGUI>().text = "LV." + lastLevel.ToString();
            }

        }
    }

    #endregion


    #region Coin System

    private void CoinSystem()
    {
        if (data.timerStart)
        {
            if (coinObj == null)
            {
                coinObj = GameObject.Find("[Text] Coin");

                if (coinObj != null)
                    coinObj.GetComponent<TextMeshProUGUI>().text = data.curCoin.ToString();
            }

            if (lastCoin != data.curCoin)
            {
                lastCoin = data.curCoin;
                coinObj.GetComponent<TextMeshProUGUI>().text = lastCoin.ToString();
            }
        }
    }

    #endregion

    #region Game Timer System

    private void TimerSlider()
    {
        if (data.timerStart)
        {
            if (timer == null)
            {
                timer = GameObject.Find("[Timer] Timer");
            }

            if (timer != null)
            {
                timer.GetComponent<Timer>().timeRemaining = 480 - (data.gameTime / gameSecondsPerRealSecond);
            }
        }
    }

    private void GameTimerSystem()
    {
        if (data.timerStart)
        {
            if (timerObj == null)
            {
                timerObj = GameObject.Find("[Text] Main Timer");
            }

            if (timerObj != null)
            {
                data.gameTime += Time.deltaTime * gameSecondsPerRealSecond;

                //24 hours (86400 seconds) are exceeded, the timer returns to 0.
                if (data.gameTime >= secondsInADay)
                {
                    data.gameTime -= secondsInADay;
                }

                DisplayTime();

                timerObj.GetComponent<TextMeshProUGUI>().text = timerText;
            }

            TimerSlider();
        }

        if (data.IsMorning)
        {
            data.IsMorning = false;
            ResetTimer();
        }
    }

    private void DisplayTime()
    {
        int totalMinutes = Mathf.FloorToInt(data.gameTime / 60f);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        timerText = string.Format("{0:D2} : {1:D2}", hours, minutes);

        if (hours >= 0 && hours < 6)
        {
            Debug.Log("얼렁 자라!!");
        }

        if (hours == 6 && minutes == 1 && IsDay)
        {
            IsDay = false;
            data.dayCount = true;
        }
        else if(hours == 6 && minutes == 5 && IsDay == false)
        {
            IsDay = true;
        }

    }

    public void ResetTimer()
    {
        data.gameTime = 6 * 60 * 60;
    }

    #endregion

    #region Ect.

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

    #endregion
}