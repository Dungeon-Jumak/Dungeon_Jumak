using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    public bool isEat;

    [Header("�޴� ���� Ȯ�� ����")]
    [SerializeField]
    private int percentOfMenu;
    [SerializeField]
    private int gukBabMax;
    [SerializeField]
    private int paJeonMax;
    [SerializeField]
    private int riceJuiceMax;
    [SerializeField]
    private int min = 0;
    [SerializeField]
    private int max = 100;

    //---������---//
    [SerializeField]
    Data data;

    [SerializeField]
    private CustomerMovement customerMovement;

    void Start()
    {
        isEat = false;

        gukBabMax = 50;
        paJeonMax = 80;
        riceJuiceMax = 100;

        data = DataManager.Instance.data;
        customerMovement = GetComponent<CustomerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //���� �޴� �ر� ������ ���� max���� �ٲ�
        switch (data.curMenuUnlockLevel)
        {
            case 1:
                max = 50; //����
                break;
            case 2:
                max = 80; //����
                break;
            case 3:
                max = 100; //����
                break;
        }

        if (!isEat)
        {
            //���� �ڸ��� ������ �ö󰬴ٸ�
            if (data.onTables[customerMovement.seatIndex])
            {
                isEat = true;
                EatFood(data.menuCategories[customerMovement.seatIndex]);
            }
        }
    }

    //---�޴� �ֹ� ���� �Լ�---//
    public void OrderNewMenu()
    {
        int randNum = Random.Range(min, max); //���� ��

        //Ȯ���� ���� ī�װ� ����
        if (randNum >= min && randNum < gukBabMax)
            data.menuCategories[customerMovement.seatIndex] = "Gukbab";
        else if (randNum >= gukBabMax && randNum < paJeonMax)
            data.menuCategories[customerMovement.seatIndex] = "Pajeon";
        else if (randNum >= paJeonMax && randNum < riceJuiceMax)
            data.menuCategories[customerMovement.seatIndex] = "RiceJuice";

        OrederSelectMenu(data.menuCategories[customerMovement.seatIndex]);

    }

    //�ֹ� �׸� ���̰� �ϴ� �Լ�
    void OrederSelectMenu(string _menuCategories)
    {
        switch (_menuCategories)
        {
            case "Gukbab":
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Pajeon":
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "RiceJuice":
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                Debug.Log("���� : ���� �޴� ī�װ��� �����߽��ϴ�");
                break;
        }

    }

    //��ǳ�� ����� �Լ�
    void EatFood(string _menuCategories)
    {

        switch (_menuCategories)
        {
            case "Gukbab":
                BubbleShadowController bsc0 = transform.GetChild(0).GetChild(1).GetComponent<BubbleShadowController>();
                bsc0.Initialize();
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Pajeon":
                BubbleShadowController bsc1 = transform.GetChild(1).GetChild(1).GetComponent<BubbleShadowController>();
                bsc1.Initialize();
                transform.GetChild(1).gameObject.SetActive(false);
                break;
            case "RiceJuice":
                BubbleShadowController bsc2 = transform.GetChild(2).GetChild(1).GetComponent<BubbleShadowController>();
                bsc2.Initialize();
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            default:
                Debug.Log("���� : EatFood(In OrderMenu)");
                break;
        }
        customerMovement.EatFood();
    }

    public void TimeOut()
    {
        switch (data.menuCategories[customerMovement.seatIndex])
        {
            case "Gukbab":
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Pajeon":
                transform.GetChild(1).gameObject.SetActive(false);
                break;
            case "RiceJuice":
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            default:
                Debug.Log("���� : Time Out(In OrderMenu)");
                break;
        }
        customerMovement.TimeOut();
    }

}
