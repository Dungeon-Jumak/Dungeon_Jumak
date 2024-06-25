//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

//TMP
using TMPro;

[DisallowMultipleComponent]
public class ConfirmFood : MonoBehaviour
{
    //Want Coocking Food Count
    [Header("원하는 음식 조리 갯수")]
    public int wantCookingFood;

    //
    [Header("국밥을 생성하는 Gukbab Generator 스크립트")]
    [SerializeField] private GukbabGenerator gukbapSetting;

    //
    [Header("확정창으로 넘길 때 정보를 불러오기 위한 SetFood 스크립트")]
    [SerializeField] private SetFood setFood;

    //
    [Header("확정 팝업 오브젝트")]
    [SerializeField] private GameObject confirmPopUp;

    //
    [Header("음식 선택 패널 오브젝트")]
    [SerializeField] private GameObject chooseFoodPanel;
    
    //
    [Header("게임 시작 패널 오브젝트")]
    [SerializeField] private GameObject startPanel;

    //
    [Header("확정을 위한 버튼 배열")]
    [SerializeField] private Button[] confirmButtons;

    //
    [Header("현재 클릭한 확정 버튼")]
    [SerializeField] private Button curConfirmButton;

    //
    [Header("음식의 조리 갯수를 표시하기 위한 TMP")]
    [SerializeField] private TextMeshProUGUI countTMP;

    //
    [Header("팝업 가운데 들어가는 음식 이미지")]
    [SerializeField] private Image foodImage;

    //Check Base Food
    [SerializeField] private bool freeFood;

    //Check Start Game
    private bool gameStart;

    private void Start()
    {
        for (int i = 0; i < confirmButtons.Length; i++)
            confirmButtons[i].interactable = true;

        freeFood = true;

        gameStart = false;
    }

    private void Update()
    {
       if(!confirmButtons[0].interactable && !confirmButtons[1].interactable && !confirmButtons[2].interactable && !gameStart)
        {
            gameStart = true;
            startPanel.SetActive(true);
            chooseFoodPanel.SetActive(false);
        }

        if (freeFood) countTMP.text = "무한";
        else countTMP.text = wantCookingFood.ToString();

    }
    

}
