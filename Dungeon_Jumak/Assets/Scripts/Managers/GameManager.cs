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

    [Header("경험치 슬라이더 오브젝트")]
    [SerializeField] private GameObject xpObj;

    private float lastXp;

    [Header("공통헤더 밤 배경")]
    [SerializeField] private GameObject nightBackGround;

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
        lastXp = data.curXP;

        _soundManager.Init();//Init the SoundManager.cs
    }

    private void Update()
    {
        //Level System
        LevelSystem();

        //Coin System
        CoinSystem();

        XPSystem();
    }

    #region Level System

    private void LevelSystem()
    {
        if (SceneManager.GetActiveScene().name != "StartScene" && SceneManager.GetActiveScene().name != "Map" && SceneManager.GetActiveScene().name != "WaitingScene")
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
        if (SceneManager.GetActiveScene().name != "StartScene" && SceneManager.GetActiveScene().name != "Map" && SceneManager.GetActiveScene().name != "WaitingScene")
        {
            if (coinObj == null)
            {
                coinObj = GameObject.Find("[Text] Coin");

                if (coinObj != null)
                    coinObj.GetComponent<TextMeshProUGUI>().text = data.curCoin.ToString() + "전";
            }

            if (lastCoin != data.curCoin)
            {
                lastCoin = data.curCoin;
                coinObj.GetComponent<TextMeshProUGUI>().text = lastCoin.ToString() + "전";
            }
        }
    }

    #endregion

    #region XP System

    private void XPSystem()
    {
        if (SceneManager.GetActiveScene().name != "StartScene" && SceneManager.GetActiveScene().name != "Map" && SceneManager.GetActiveScene().name != "WaitingScene")
        {
            if (xpObj == null)
            {
                xpObj = GameObject.Find("[Slider] Xp");

                if (xpObj != null)
                    xpObj.GetComponent<Slider>().value = data.curXP / data.maxXP;
            }

            if (lastXp != data.curXP)
            {
                lastXp = data.curXP;
                xpObj.GetComponent<Slider>().value = data.curXP / data.maxXP;
            }
        }
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