//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class MenuInfo : MonoBehaviour
{
    [Header("��ư ��ȣ �ε���")]
    [SerializeField] private int buttonIndex;

    [Header("�޴� �ر� ����")]
    [SerializeField] private int unlockPrice;

    [Header("���� �Ұ� �˾�")]
    [SerializeField] private GameObject canNotBuyPopUp;

    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    //Confirm Buy Receipe
    public void ConfrimUnlcokMenu()
    {
        if (data.curCoin >= unlockPrice)
        {
            data.curCoin -= unlockPrice;
            data.unlockMenuIndex[buttonIndex] = true;
        }
        else canNotBuyPopUp.SetActive(true);

    }

}
