using UnityEngine;
using UnityEngine.UI;

public class CollisionController : MonoBehaviour
{
    public GameObject squareObject; 
    public GameObject ballObject; 
    public Text waterText; 
    public Text riceText;
    public int waterIncreaseAmount = 5; 
    public int riceIncreaseAmount = 5; 
    public float increaseDuration = 1f; // 변수 증가 시간

    private int water = 0; 
    private int rice = 0; 
    private bool alreadyReachedMax = false; // 이미 최대값에 도달했는지 여부
    private float lastWaterIncreaseTime; // 마지막 water 증가 시간
    private float lastRiceIncreaseTime; // 마지막 rice 증가 시간

    public GameObject RiceJuicePopup;
    public GameObject RiceJuiceBar;

    void OnEnable()
    {
        ResetValues();
    }

    void Update()
    {
        //--이미 최대값에 도달했다면 더 이상 변수를 증가시키지 않음--/
        if (alreadyReachedMax)
            return;

        //--총 100의 값에 도달했을 때 미니게임 종료--/
        if (water + rice >= 100)
        {
            alreadyReachedMax = true;
            RiceJuiceBar.SetActive(false);
            RiceJuicePopup.SetActive(false);
            return;
        }

        //--Ball 오브젝트의 x 좌표--//
        float ballX = ballObject.transform.position.x;
        //--네모 오브젝트의 x 좌표 끝값과 시작값--//
        float squareXStart = squareObject.transform.position.x - squareObject.transform.localScale.x / 2;
        float squareXEnd = squareObject.transform.position.x + squareObject.transform.localScale.x / 2;

        //--Ball 오브젝트가 네모 오브젝트 안에 있는지 확인--//
        bool isInside = ballX >= squareXStart && ballX <= squareXEnd;

        //--Ball 오브젝트가 네모 오브젝트 안에 있는 경우--/
        if (isInside)
        {
            // 2초에 1씩 water 변수 증가
            if (Time.time - lastWaterIncreaseTime >= increaseDuration)
            {
                water += waterIncreaseAmount;
                waterText.text = "Water: " + water.ToString();
                lastWaterIncreaseTime = Time.time;
            }
        }
        //--Ball 오브젝트가 네모 오브젝트 안에 없는 경우--//
        else
        {
            //--2초에 1씩 rice 변수 증가--/
            if (Time.time - lastRiceIncreaseTime >= increaseDuration)
            {
                rice += riceIncreaseAmount;
                riceText.text = "Rice: " + rice.ToString();
                lastRiceIncreaseTime = Time.time;
            }
        }
    }

    void ResetValues()
    {
        water = 0;
        rice = 0;
        alreadyReachedMax = false;
        lastWaterIncreaseTime = 0f;
        lastRiceIncreaseTime = 0f;
        waterText.text = "Water: " + water.ToString();
        riceText.text = "Rice: " + rice.ToString();
    }
}
