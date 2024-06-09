//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.VisualScripting;

//TMPro
using TMPro;

[DisallowMultipleComponent]
public class JumakScene : BaseScene
{
    #region Variables

    //For Start
    [Header("시작 여부를 판단하기 위한 Bool 변수 (Don't touch)")]
    public bool start;

    //For Pause
    [Header("게임 일시 정지 여부를 판단하기 위한 변수")]
    public bool pause;

    //For End
    private bool end = false;

    //Receipt Popup
    [Header("전표 팝업 오브젝트")]
    [SerializeField] private GameObject receiptPopup;

    //Timer
    [Header("타이머 시간 초")]
    [SerializeField] private float timer;

    //Duration of Timer
    [Header("주막의 총 운영 시간")]
    [SerializeField] private float duration;

    //Timer TMP
    [Header("타이머 TMP")]
    [SerializeField] private TextMeshProUGUI timerTMP;

    //Coin TMP
    [Header("코인 TMP")]
    [SerializeField] private TextMeshProUGUI coinTMP;

    //House Sprites
    [Header("주막 : 하우스 스프라이트 배열")]
    [SerializeField] private Sprite[] houseSprites;

    //Left Fence Sprites
    [Header("주막 : 왼쪽 펜스 스프라이트 배열")]
    [SerializeField] private Sprite[] leftFenceSprites;

    //Right Fence Sprites
    [Header("주막 : 오른쪽 펜스 스프라이트 배열")]
    [SerializeField] private Sprite[] rightFenceSprites;

    //Dansang Sprites
    [Header("주막 : 단상 스프라이트 배열")]
    [SerializeField] private Sprite[] dansangSprites;

    //Table Sprites
    [Header("주막 : 테이블 스프라이트 배열")]
    [SerializeField] private Sprite[] tableSprites;

    //House Sprite Renderer
    [Header("하우스 스프라이트 렌더러")]
    [SerializeField] private SpriteRenderer house;

    //Left Fence Sprite Renderer
    [Header("왼쪽 펜스 스프라이트 렌더러")]
    [SerializeField] private SpriteRenderer leftFence;

    //Right Fence Sprite Renderer
    [Header("오른쪽 펜스 스프라이트 렌더러")]
    [SerializeField] private SpriteRenderer rightFence;

    //Dansang Sprite Renderers
    [Header("단상 스프라이트 렌더러 배열")]
    [SerializeField] private SpriteRenderer[] dansangs;

    //Tables Sprite Renderers
    [Header("테이블 스프라이트 렌더러 배열")]
    [SerializeField] private SpriteRenderer[] tables;

    //Objects related Jumak System
    [Header("주막 시스템과 관련된 오브젝트 배열")]
    [SerializeField] private GameObject[] JumakSystemObj;

    //Gukbab Receipt TMP
    [Header("국밥 전표 텍스트")]
    [SerializeField] private TextMeshProUGUI gukbabReceiptTMP;

    //Pajeon Receipt TMP
    [Header("파전 전표 텍스트")]
    [SerializeField] private TextMeshProUGUI pajeonReceiptTMP;

    //RiceJuice Receipt TMP
    [Header("식혜 전표 텍스트")]
    [SerializeField] private TextMeshProUGUI riceJuiceReceiptTMP;

    //Total Receipt TMP
    [Header("토탈 가격 전표 텍스트")]
    [SerializeField] private TextMeshProUGUI totalReceiptTMP;

    //Data
    private Data data;

    //Past Coin
    private int pastCoin;

    #endregion

    private void Start()
    {
        //Get Data
        data = DataManager.Instance.data;

        //Initialize Past Coin
        pastCoin = data.curCoin;

        //Initialze Food Count
        data.gukbapCount = 0;
        data.pajeonCount = 0;
        data.riceJuiceCount = 0;

        //Initialize Jumak System's Boolean Variables
        for (int i = 0; i < data.onTables.Length; i++)
        {
            data.isAllocated[i] = false;
            data.isCustomer[i] = false;
            data.onTables[i] = false;
            data.isFinEat[i] = false;
        }

        //Initialize Customer HeadCount
        data.customerHeadCount = 0;

        //Initialize Timer Value
        timer = 0;

        //Initialize Jumak Start, Pause, End Boolean Value
        start = false;
        pause = false;
        end = false;

        //Update Jumak Furniture
        UpdateJumakFurniture();
    }

    public void Update()
    {
        //Jumak System
        JumakSystem();
    }

    #region Methods

    //Update Jumak Furniture : According to Jumak Furniture's Level
    private void UpdateJumakFurniture()
    {
        //Update House Sprite
        house.sprite = houseSprites[data.houseLV];

        //Update Left Fence Sprite
        leftFence.sprite = leftFenceSprites[data.houseLV];

        //Update Right Fence Sprite 
        rightFence.sprite = rightFenceSprites[data.houseLV];

        //Update Dansangs and Tables Sprite
        for (int i = 0; i < dansangs.Length; i++)
        {
            dansangs[i].sprite = dansangSprites[data.dansangLV];
            tables[i].sprite = tableSprites[data.tableLV];
        }
    }

    //Jumak System
    private void JumakSystem()
    {
        //If cur coin greater than past coin, update coin text
        if (data.curCoin > pastCoin)
        {
            //update past coin
            pastCoin = data.curCoin;

            //update text
            coinTMP.text = data.curCoin.ToString() + "전";
        }

        //Call Timer Method
        Timer();

        //If end and mousebutton down, load waiting scene
        if (end && Input.GetMouseButtonDown(0))
            GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }

    //Timer
    private void Timer()
    {
        //If game is started and is not pausing, activate timer
        if (start && !pause)
        {
            timer += Time.deltaTime;

            //If Time Out
            if (timer >= duration)
            {
                //Change Start Sign
                start = false;

                //Call Clost Jumak Method
                CloseJumak();
            }

            //Compute time for display
            int newTime = Mathf.FloorToInt(duration - timer);

            //Update Timer Text
            timerTMP.text = newTime.ToString();
        }
    }

    //Method to Close Jumak
    private void CloseJumak()
    {
        //InActivate Object related Jumak System
        for (int i = 0; i < JumakSystemObj.Length; i++)
        {
            JumakSystemObj[i].SetActive(false);
        }

        //Active Receipt Popup
        receiptPopup.SetActive(true);

        //Activate Text Sequentially
        StartCoroutine(ActivateTextSequentially());

        //Compute Current Total Price
        data.currentTotalPrice = (data.gukbapCount * data.nowGukbapPrice) + (data.riceJuiceCount * data.nowRiceJuicePrice) + (data.pajeonCount * data.nowPajeonPrice);
    }

    //Method to Activate Text 'Sequentially'
    private IEnumerator ActivateTextSequentially()
    {
        //WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);

        //Gukbab
        gukbabReceiptTMP.text = "국밥 x " + data.gukbapCount.ToString() + " = " + data.gukbapCount * data.nowGukbapPrice;

        //WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);

        //Pajeon
        pajeonReceiptTMP.text = "파전 x " + data.pajeonCount.ToString() + " = " + data.pajeonCount * data.nowPajeonPrice;

        //WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);

        //RiceJuice
        riceJuiceReceiptTMP.text = "식혜 x " + data.riceJuiceCount.ToString() + " = " + data.riceJuiceCount * data.nowRiceJuicePrice;

        //WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);

        //Total
        totalReceiptTMP.text = "총 매출 = " + data.currentTotalPrice.ToString() + "전";

        //WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);

        //Change end sign
        end = true;
    }

    //Method to Jumak Start
    public void JumakStart()
    {
        //Change start sign
        start = true;

        //Increase day
        data.days++;
    }

    //Method to Add Recipe
    public void AddRecipe()
    {
        //Increase Current Menu Unlock Level
        if (data.curMenuUnlockLevel < data.maxMenuUnlockLevel)
            data.curMenuUnlockLevel++;
    }

    //For Convert Scene
    public override void Clear()
    {
        //Debug.Log
        Debug.Log("Jumak Scene changed!");
    }

    #endregion
}