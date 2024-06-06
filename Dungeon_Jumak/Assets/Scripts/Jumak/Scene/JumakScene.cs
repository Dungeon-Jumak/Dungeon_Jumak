using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class JumakScene : BaseScene
{
    public bool isStart;
    public bool isPause;


    private Data data;

    [SerializeField]
    private FadeController fadeController;

    //---해금 할 단상 배열---//
    [SerializeField]
    private GameObject[] Dansangs;

    [SerializeField]
    private GameObject[] JumakSystemObj;

    [SerializeField]
    private GameObject receiptPopup;

    [SerializeField] private float timer;
    [SerializeField] private float duration;

    [SerializeField]
    private TextMeshProUGUI timerTxt;

    [SerializeField]
    int compareTotalPrice;

    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject[] panelForStart;

    [SerializeField]
    private TextMeshProUGUI coinTMP;

    [SerializeField]
    private TextMeshProUGUI dayTMP;

    [SerializeField]
    private Sprite[] houseSprites;
    [SerializeField]
    private Sprite[] leftFenceSprites;
    [SerializeField]
    private Sprite[] rightFenceSprites;
    [SerializeField]
    private Sprite[] dansangSprites;
    [SerializeField]
    private Sprite[] tableSprites;

    [SerializeField]
    private SpriteRenderer house;
    [SerializeField]
    private SpriteRenderer leftFence;
    [SerializeField]
    private SpriteRenderer rightFence;
    [SerializeField]
    private SpriteRenderer[] dansangs;
    [SerializeField]
    private SpriteRenderer[] tables;

    private bool playBGM = false;
    private bool endPanel = false;


    private void Start()
    {

        playBGM = false;

        data = DataManager.Instance.data;

        data.gukbapCount = 0;
        data.pajeonCount = 0;
        data.riceJuiceCount = 0;

        fadeController = FindObjectOfType<FadeController>();

        for (int i = 0; i < data.onTables.Length; i++)
        {
            data.isAllocated[i] = false;
            data.isCustomer[i] = false;
            data.onTables[i] = false;
            data.isFinEat[i] = false;
        }


        data.customerHeadCount = 0;


        timer = 0;
        isStart = false;
        isPause = false;

      
    }

    public void Update()
    {
        UpdateJumakFurniture();

        coinTMP.text = data.curCoin.ToString() + "전";
        //dayTMP.text = data.days.ToString() + " 일차";

        if (isStart && !isPause)
            timer += Time.deltaTime;

        if (timer >= duration)
        {
            isStart = false;

            timer -= duration;
            JumakOff();
        }

        int newTime = Mathf.FloorToInt(duration - timer);
        timerTxt.text = newTime.ToString();

        if (endPanel && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)))
        {
            SceneManager.LoadScene("WaitingScene");
        }

    }

    private void UpdateJumakFurniture()
    {
        house.sprite = houseSprites[data.houseLV];
        leftFence.sprite = leftFenceSprites[data.houseLV];
        rightFence.sprite = rightFenceSprites[data.houseLV];

        for (int i = 0; i < dansangs.Length; i++)
        {
            dansangs[i].sprite = dansangSprites[data.dansangLV];
            tables[i].sprite = tableSprites[data.tableLV];
        }
    }

    private void JumakOff()
    {
        for (int i = 0; i < JumakSystemObj.Length; i++)
        {
            JumakSystemObj[i].SetActive(false);
        }
        receiptPopup.SetActive(true);

        StartCoroutine(ActivateTextSequentially());

        data.currentTotalPrice = (data.gukbapCount * data.nowGukbapPrice) + (data.riceJuiceCount * data.nowRiceJuicePrice) + (data.pajeonCount * data.nowPajeonPrice);
    }

    private IEnumerator ActivateTextSequentially()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Find("GukBap_Recipt").GetComponent<TextMeshProUGUI>().text = "국밥 x " + data.gukbapCount.ToString() + " = " + data.gukbapCount * data.nowGukbapPrice;
        yield return new WaitForSeconds(1f);

        GameObject.Find("Pajeon_Recipt").GetComponent<TextMeshProUGUI>().text = "파전 x " + data.pajeonCount.ToString() + " = " + data.pajeonCount * data.nowPajeonPrice;
        yield return new WaitForSeconds(1f);

        GameObject.Find("RiceJuice_Recipt").GetComponent<TextMeshProUGUI>().text = "식혜 x " + data.riceJuiceCount.ToString() + " = " + data.riceJuiceCount * data.nowRiceJuicePrice;

        yield return new WaitForSeconds(1f);
        GameObject.Find("Total_Recipt").GetComponent<TextMeshProUGUI>().text = "총 매출 = " + data.currentTotalPrice.ToString() + "전";

        yield return new WaitForSeconds(1f);

        endPanel = true;
    }

    public void JumakStart()
    {
        isStart = true;

        data.days++;
    }

    public void AddRecipe()
    {
        if (data.curMenuUnlockLevel < data.maxMenuUnlockLevel)
        {
            data.curMenuUnlockLevel++;
        }

    }


    public void ConvertScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void InitialTransfrom(GameObject go)
    {
        go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, go.transform.localPosition.z);
    }

    //씬 전환할 때 필요한 기능
    public override void Clear()
    {
        Debug.Log("Jumak Scene changed!");
    }
}