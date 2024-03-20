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
    public float increaseDuration = 1f; // ���� ���� �ð�

    private int water = 0; 
    private int rice = 0; 
    private bool alreadyReachedMax = false; // �̹� �ִ밪�� �����ߴ��� ����
    private float lastWaterIncreaseTime; // ������ water ���� �ð�
    private float lastRiceIncreaseTime; // ������ rice ���� �ð�

    public GameObject RiceJuicePopup;
    public GameObject RiceJuiceBar;

    void OnEnable()
    {
        ResetValues();
    }

    void Update()
    {
        //--�̹� �ִ밪�� �����ߴٸ� �� �̻� ������ ������Ű�� ����--/
        if (alreadyReachedMax)
            return;

        //--�� 100�� ���� �������� �� �̴ϰ��� ����--/
        if (water + rice >= 100)
        {
            alreadyReachedMax = true;
            RiceJuiceBar.SetActive(false);
            RiceJuicePopup.SetActive(false);
            return;
        }

        //--Ball ������Ʈ�� x ��ǥ--//
        float ballX = ballObject.transform.position.x;
        //--�׸� ������Ʈ�� x ��ǥ ������ ���۰�--//
        float squareXStart = squareObject.transform.position.x - squareObject.transform.localScale.x / 2;
        float squareXEnd = squareObject.transform.position.x + squareObject.transform.localScale.x / 2;

        //--Ball ������Ʈ�� �׸� ������Ʈ �ȿ� �ִ��� Ȯ��--//
        bool isInside = ballX >= squareXStart && ballX <= squareXEnd;

        //--Ball ������Ʈ�� �׸� ������Ʈ �ȿ� �ִ� ���--/
        if (isInside)
        {
            // 2�ʿ� 1�� water ���� ����
            if (Time.time - lastWaterIncreaseTime >= increaseDuration)
            {
                water += waterIncreaseAmount;
                waterText.text = "Water: " + water.ToString();
                lastWaterIncreaseTime = Time.time;
            }
        }
        //--Ball ������Ʈ�� �׸� ������Ʈ �ȿ� ���� ���--//
        else
        {
            //--2�ʿ� 1�� rice ���� ����--/
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
