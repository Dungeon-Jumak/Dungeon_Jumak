//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

//TMP
using TMPro;
using Unity.VisualScripting;

[DisallowMultipleComponent]
public class SetMenu : MonoBehaviour
{
    //Button Number
    [Header("��ư ��ȣ")]
    public int number;

    //Food Image
    [Header("���� �̹���")]
    public Image foodImage;

    //Count of Need Ingredient
    [Header("�ʿ��� ��� ����")]
    public int needIngredientNum;

    //Count Each of Need Ingredients
    [Header("�ʿ��� �� ����� ����")]
    public int[] needIngredients = new int[5];

    //Max Can Cook Count
    [Header("�ִ� �丮 ���� ����")]
    public int maxCookCount = 0;

    //TMP of Having Ingredient
    [Header("���� �ִ� ��� Text")]
    [SerializeField] private TextMeshProUGUI[] haveIngredientTMPs;

    //Max ingredient to show on screen 
    [Header("�ִ�� ���� �� �ִ� �丮 ����")]
    [SerializeField] private int maxSignHaveIngredient;

    //Check Click
    [Header("��ư Ŭ�� ����")]
    public bool onClick = false;

    //Check Can Add
    [Header("�߰� ���� ����")]
    public bool canAdd; 

    //Check Free Menu
    [Header("Free Menu ����")]
    public bool freeMenu;

    //Data
    private Data data;

    //Color
    private Color color;

    //Check Count
    private int checkNum;

    private void Start()
    {
        //Get Data
        data = DataManager.Instance.data;

        //max Sign Have Ingredient is '99'
        maxSignHaveIngredient = 99;
    }

    private void Update()
    {
        if (!freeMenu)
        {
            CheckIngredient();
        }
        else canAdd = true;
    }

    //Coroutine for Checking Ingredient
    private void CheckIngredient()
    {
        //Init Check Num, Check Num is variable to ingredient's count
        checkNum = 0;

        //Chect Ingredient
        for (int i = 0; i < data.ingredient.Length; i++)
        {
            //if need ingredient
            if (needIngredients[i] > 0)
            {
                //if current number of ingredient greater than to need ingredient
                if (data.ingredient[i] >= needIngredients[i])
                {
                    //Increase CheckNum
                    checkNum++;

                    //temp variable that can cook max count
                    int tempmaxCookCount = data.ingredient[i] / needIngredients[i];

                    //if first checking or tempmaxcount less than last max cook count, update max cook count
                    if (i == 0 || tempmaxCookCount > maxCookCount)
                    {
                        if (tempmaxCookCount > 99) maxCookCount = 99;
                        else maxCookCount = tempmaxCookCount;
                    }

                    //change text color => if enough ingredient, color is black
                    color = Color.black;
                    haveIngredientTMPs[i].color = color;

                    //display text (greater than max)
                    if (data.ingredient[i] > maxSignHaveIngredient)
                        haveIngredientTMPs[i].text = maxSignHaveIngredient.ToString();
                    //display tex (less than max)
                    else
                        haveIngredientTMPs[i].text = data.ingredient[i].ToString();
                }
                //lack ingredient
                else
                {
                    //change text color => if lack ingredient, color is red
                    color = Color.red;
                    haveIngredientTMPs[i].color = color;

                    //display text
                    haveIngredientTMPs[i].text = data.ingredient[i].ToString();
                }
            }
        }

        //if all ingredient is enough, can add
        if (checkNum == needIngredientNum) canAdd = true;
        //if all ingredient is not enough, can't add
        else canAdd = false;
    }

    //OnClick
    public void OnClick()
    {
        //If click Button and can add
        if (canAdd)
        {
            //Convert onclick sign
            onClick = true;
        }
    }

    //OffClick
    public void OffClick()
    {
        //Convert onclck sign. if is not click button
        onClick = false;
    }
}