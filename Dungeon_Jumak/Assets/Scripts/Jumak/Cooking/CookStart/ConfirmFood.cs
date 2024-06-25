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
    [Header("���ϴ� ���� ���� ����")]
    public int wantCookingFood;

    //
    [Header("������ �����ϴ� Gukbab Generator ��ũ��Ʈ")]
    [SerializeField] private GukbabGenerator gukbapSetting;

    //
    [Header("Ȯ��â���� �ѱ� �� ������ �ҷ����� ���� SetFood ��ũ��Ʈ")]
    [SerializeField] private SetFood setFood;

    //
    [Header("Ȯ�� �˾� ������Ʈ")]
    [SerializeField] private GameObject confirmPopUp;

    //
    [Header("���� ���� �г� ������Ʈ")]
    [SerializeField] private GameObject chooseFoodPanel;
    
    //
    [Header("���� ���� �г� ������Ʈ")]
    [SerializeField] private GameObject startPanel;

    //
    [Header("Ȯ���� ���� ��ư �迭")]
    [SerializeField] private Button[] confirmButtons;

    //
    [Header("���� Ŭ���� Ȯ�� ��ư")]
    [SerializeField] private Button curConfirmButton;

    //
    [Header("������ ���� ������ ǥ���ϱ� ���� TMP")]
    [SerializeField] private TextMeshProUGUI countTMP;

    //
    [Header("�˾� ��� ���� ���� �̹���")]
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

        if (freeFood) countTMP.text = "����";
        else countTMP.text = wantCookingFood.ToString();

    }
    

}
