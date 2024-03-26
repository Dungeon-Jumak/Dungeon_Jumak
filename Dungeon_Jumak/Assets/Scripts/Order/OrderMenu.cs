using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    public int menuNum;
    public bool isRun = true;

    [Header("�޴� ��ǳ��")]
    [SerializeField]
    private GameObject[] speechBox;

    [Header("�޴� ���� Ȯ�� ����")]
    [SerializeField]
    private int percentOfMenu;
    [SerializeField]
    private int gukBabMax = 50;
    [SerializeField]
    private int riceJuiceMax = 70;
    [SerializeField]
    private int paJeonMax = 100;
    [SerializeField]
    private int min = 0;
    [SerializeField]
    private int max = 100;

    [Header("�޴� �ѹ���")]
    [SerializeField]
    private int gukBabNum = 1;
    [SerializeField]
    private int riceJuiceNum = 2;
    [SerializeField]
    private int paJeonNum = 3;

    //---������---//
    [SerializeField]
    Data data;

    [SerializeField]
    private CustomerMovement customerMovement;

    void Start()
    {
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
                max = 70; //����
                break;
            case 3:
                max = 100; //����
                break;
        }

        if (isRun)
        {
            //���� �ڸ��� ������ �ö󰬴ٸ�
            if (data.onTables[customerMovement.seatIndex])
            {
                isRun = false;
                EatFood(data.menuNums[customerMovement.seatIndex] - 1);
            }
        }
    }

    //---�޴� �ֹ� ���� �Լ�---//
    public void OrderNewMenu()
    {
        int randNum = Random.Range(min, max);

        if (randNum >= min && randNum < gukBabMax)
            data.menuNums[customerMovement.seatIndex] = gukBabNum;
        else if (randNum >= gukBabMax && randNum < riceJuiceMax)
            data.menuNums[customerMovement.seatIndex] = riceJuiceNum;
        else if (randNum >= riceJuiceMax && randNum < paJeonMax)
            data.menuNums[customerMovement.seatIndex] = paJeonNum;

        OrederSelectMenu(data.menuNums[customerMovement.seatIndex] - 1);

    }

    //�ֹ� �׸� ���̰� �ϴ� �Լ�
    void OrederSelectMenu(int nums)
    {
        transform.GetChild(nums).gameObject.SetActive(true);
    }

    //��ǳ�� ����� �Լ�
    void EatFood(int nums)
    {
        transform.GetChild(nums).gameObject.SetActive(false);
        customerMovement.EatFood();
    }

    public void TimeOut()
    {
        transform.GetChild(data.menuNums[customerMovement.seatIndex] - 1).gameObject.SetActive(false);
        customerMovement.TimeOut();
    }

}
