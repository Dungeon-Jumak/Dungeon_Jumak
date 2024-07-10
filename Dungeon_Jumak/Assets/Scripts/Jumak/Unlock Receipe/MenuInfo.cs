//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class MenuInfo : MonoBehaviour
{
    [Header("버튼 번호 인덱스")]
    [SerializeField] private int buttonIndex;

    [Header("메뉴 해금 가격")]
    [SerializeField] private int unlockPrice;

    [Header("구매 불가 팝업")]
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
