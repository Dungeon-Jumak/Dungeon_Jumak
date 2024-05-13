using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 시작 전 제조할 음식을 선택하고 제조하기 위한 스크립트
/// </summary>
public class CookingFood : MonoBehaviour
{
    public string category;    // 선택한 메뉴 카테고리

    //메뉴 팝업
    [SerializeField]
    private GameObject gukbabPopup;
    [SerializeField]
    private GameObject pajeonPopup;
    [SerializeField]
    private GameObject riceJuicePopup;

    private void Start()
    {
        category = "";
    }

    public void ChooseGukbab()
    {
        category = "Gukbab";

        //지정 팝업 제외 나머지 Setactive = false
        gukbabPopup.SetActive(true);
        pajeonPopup.SetActive(false);
        riceJuicePopup.SetActive(false);
    }

    public void ChoosePajeon()
    {
        category = "Pajeon";

        //지정 팝업 제외 나머지 Setactive = false
        gukbabPopup.SetActive(false);
        pajeonPopup.SetActive(true);
        riceJuicePopup.SetActive(false);
    }

    public void ChooseRiceJuice()
    {
        category = "RiceJuice";

        //지정 팝업 제외 나머지 Setactive = false
        gukbabPopup.SetActive(false);
        pajeonPopup.SetActive(false);
        riceJuicePopup.SetActive(true);
    }

    
}
