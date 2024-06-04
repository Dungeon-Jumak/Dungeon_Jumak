//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

//TMP
using TMPro;

//본 스크립트는 View Port안 Contents들 버튼에 각 부착한 후 각 버튼에 메뉴에 대한 정보를 사전 Setting하도록 한다
[DisallowMultipleComponent]
public class SetMenu : MonoBehaviour
{
    //Button Number
    [Header("버튼의 번호(0부터 시작)")]
    public int number;

    //Count of Need Ingredient
    [Header("필요한 재료 종류의 총 갯수")]
    public int needIngredientNum;

    //Count Each of Need Ingredients
    [Header("필요한 각 재료의 갯수")]
    public int[] needIngredients = new int[5];

    //Max Can Cook Count
    [Header("음식을 만들 수 있는 최대 갯수")]
    public int maxCookCount;

    //TMP of Having Ingredient
    [Header("갖고 있는 재료의 갯수")]
    [SerializeField] TextMeshProUGUI[] haveIngredientTMPs;

    //Max ingredient to show on screen 
    [Header("최대로 표시할 재료의 갯수")]
    [SerializeField] int maxSignHaveIngredient;
    
    //Check Click
    [Header("버튼 클릭 여부")]
    public bool onClick = false;

    //Check Can Add
    [Header("음식을 추가할 수 있는지 여부")]
    public bool canAdd; // 재료가 충분할 시 음식을 추가 여부를 판단하기 위한 변수

    //Check Free Menu
    [Header("재료 필요 없이 사용할 수 있는 기본 메뉴인지 확인")]
    public bool freeMenu; // 재료 없이 사용할 수 있는 메뉴 인지

    //Data
    private Data data;

    //Color
    Color color;

    //Check Count
    [SerializeField] private int checkNum;

    private void Start()
    {
        //Get Data
        data = DataManager.Instance.data;

        //If is not base menu, start coroutine
        if(!freeMenu)
            StartCoroutine(CheckIngredient());
        //If is base menu, can add = true
        else canAdd = true;

        //max Sign Have Ingredient is '99'
        maxSignHaveIngredient = 99;
    }

    //Coroutine for Checking Ingredient
    IEnumerator CheckIngredient()
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
                    if(i == 0 || tempmaxCookCount < maxCookCount)
                        maxCookCount = tempmaxCookCount;

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

        //yield retrun
        yield return null;

        //recursion coroutine
        StartCoroutine(CheckIngredient());
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
