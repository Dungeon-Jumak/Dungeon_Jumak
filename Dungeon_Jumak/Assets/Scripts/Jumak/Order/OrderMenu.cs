using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    //--- ���� ������ �����ϴ� bool ���� ---//
    public bool isEat;

    //--- �޴� ���� Ȯ�� ���� ---//
    [SerializeField]
    private int gukBabMax;          //--- ���� �ִ� Ȯ�� ---//
    [SerializeField]
    private int paJeonMax;          //--- ���� �ִ� Ȯ�� ---//
    [SerializeField]
    private int riceJuiceMax;       //--- ���� �ִ� Ȯ�� ---//

    //--- �ּ� Ȯ�� ���� �ִ� Ȯ�� �� ���� ---//
    private int min = 0;
    private int max = 100;

    //--- ������ ---//
    Data data;

    //--- CustomerMovement Componet ---//
    private CustomerMovement customerMovement;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void Start()
    {
        //--- Initialize Variables ---//
        isEat = false;

        gukBabMax = 50;
        paJeonMax = 80;
        riceJuiceMax = 100;

        //--- Get Component ---//
        data = DataManager.Instance.data;
        customerMovement = GetComponent<CustomerMovement>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        //--- ���� �޴� �ر� ������ ���� max���� �ٲ� ---//
        //*** �ر��� ��� �߰� �� �������� ���� Ȯ���� ��ȹ�� ������ �ڵ� ���� ***//
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

        //--- ������ �԰� ���� ������ ---//
        if (!isEat)
        {
            //--- ���� �ڸ��� ������ �ö󰬴ٸ� ---//
            if (data.onTables[customerMovement.seatIndex])
            {
                //--- �ߺ� ���� ���� bool ���� ��ȯ ---//
                isEat = true;
                //--- EatFood �Լ� ���� ---//
                EatFood(data.menuCategories[customerMovement.seatIndex]);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- �޴��� ī�װ��� �����ϱ� ���� �Լ� ---//
    public void OrderNewMenu()
    {
        //--- ������ ���� Ȯ�� ������ ����� ---//
        int randNum = Random.Range(min, max);

        //--- Ȯ���� ���� ī�װ� ���� ---//
        if (randNum >= min && randNum < gukBabMax)
            data.menuCategories[customerMovement.seatIndex] = "Gukbab";
        else if (randNum >= gukBabMax && randNum < paJeonMax)
            data.menuCategories[customerMovement.seatIndex] = "Pajeon";
        else if (randNum >= paJeonMax && randNum < riceJuiceMax)
            data.menuCategories[customerMovement.seatIndex] = "RiceJuice";

        //--- �޴��� �����Ǹ� �ش�Ǵ� �޴��� ��ǳ���� �մ����� ��쵵�� �� ---//
        OrederSelectMenu(data.menuCategories[customerMovement.seatIndex]);

    }

    //--- ������ �޴��� ��ǳ���� ���� ���� �Լ� ---//
    private void OrederSelectMenu(string _menuCategories)
    {
        //--- �޴� ī�װ��� ���� Switch ������ �б⸦ ���� ---//
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

    //--- ���� ���� ��ȣ�� �����ϰ� ��ǳ���� ����� �Լ� ---//
    private void EatFood(string _menuCategories)
    {
        //--- ī�װ��� ��ǳ���� SetActive�� ��ȯ�ϴ� �б⹮ ---//
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

        //--- CustomerMovement -> EatFood �޼ҵ� ���� ---//
        customerMovement.EatFood();
    }

    //--- �ð��� �� �Ǿ��� �� ��ǳ���� ���� �մ��� ���ư��� �ϴ� �Լ� ---//
    public void TimeOut()
    {
        //--- ī�װ��� ���� Switch �б⹮ ---//
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

        //--- CustomerMovement -> TimeOut �޼ҵ� ���� ---//
        customerMovement.TimeOut();
    }

}
