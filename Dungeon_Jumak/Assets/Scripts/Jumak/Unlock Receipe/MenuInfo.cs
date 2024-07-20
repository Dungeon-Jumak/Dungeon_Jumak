//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MenuInfo : MonoBehaviour
{
    [Header("��ư ��ȣ �ε���")]
    [SerializeField] private int buttonIndex;

    [Header("�޴� �̸�")]
    [SerializeField] private string menuName;

    [Header("�޴� �ر� ����")]
    [SerializeField] private int unlockPrice;

    [Header("���� �Ұ� �˾�")]
    [SerializeField] private GameObject canNotBuyPopUp;

    [Header("������ ���� Ȯ�� �˾�")]
    [SerializeField] private GameObject confirmBuyPopUp;

    [Header("������ ���� Ȯ�� �˾� Ȯ�� ��ư")]
    [SerializeField] private Button buttonOfConfirmPopUp;

    [Header("Black Front�� �г�")]
    [SerializeField] private GameObject blackFrontPanel;

    [Header("��ư �̹��� �迭")]
    [SerializeField] private GameObject[] objs;

    [Header("Lock �̹���")]
    [SerializeField] private GameObject lockImage;

    [Header("Lcok ��ư")]
    [SerializeField] private GameObject lockButton;


    private Data data;

    private Button chooseButton;

    private void Start()
    {
        chooseButton = GetComponent<Button>();

        data = DataManager.Instance.data;


        //Active Button Component
        if (data.unlockMenuIndex[buttonIndex])
        {
            for (int i = 0; i < objs.Length; i++)
                objs[i].SetActive(true);

            lockImage.SetActive(false);

            chooseButton.interactable = true;

            lockButton.SetActive(false);
        }   
    }


    //Confirm Buy Receipe
    public void ConfrimUnlcokMenu()
    {
        data.curCoin -= unlockPrice;
        data.unlockMenuIndex[buttonIndex] = true;

        for (int i = 0; i < objs.Length; i++)
            objs[i].SetActive(true);

        lockImage.SetActive(false);

        data.unlockMenuIndex[buttonIndex] = true;

        chooseButton.interactable = true;

        lockButton.SetActive(false);
    }

    public void CheckPriceAndOnPopUp()
    {
        InitOnClick();

        if (data.curCoin >= unlockPrice)
        {
            blackFrontPanel.SetActive(true);

            confirmBuyPopUp.SetActive(true);

            buttonOfConfirmPopUp.onClick.AddListener(ConfrimUnlcokMenu);
        }
        else
        {
            blackFrontPanel.SetActive(true);

            canNotBuyPopUp.SetActive(true);
        }
    }


    public void InitOnClick()
    {
        buttonOfConfirmPopUp.onClick.RemoveAllListeners();
    }

}
