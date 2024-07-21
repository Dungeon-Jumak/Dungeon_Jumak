//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MenuInfo : MonoBehaviour
{
    [Header("버튼 번호 인덱스")]
    [SerializeField] private int buttonIndex;

    [Header("메뉴 이름")]
    [SerializeField] private string menuName;

    [Header("메뉴 해금 가격")]
    [SerializeField] private int unlockPrice;

    [Header("구매 불가 팝업")]
    [SerializeField] private GameObject canNotBuyPopUp;

    [Header("레시피 구매 확정 팝업")]
    [SerializeField] private GameObject confirmBuyPopUp;

    [Header("레시피 구매 확정 팝업 확인 버튼")]
    [SerializeField] private Button buttonOfConfirmPopUp;

    [Header("Black Front형 패널")]
    [SerializeField] private GameObject blackFrontPanel;

    [Header("버튼 이미지 배열")]
    [SerializeField] private GameObject[] objs;

    [Header("Lock 이미지")]
    [SerializeField] private GameObject lockImage;

    [Header("Lcok 버튼")]
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