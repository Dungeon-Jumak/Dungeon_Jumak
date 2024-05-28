using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetMenu : MonoBehaviour
{
    public int number;
    public int[] needIngredients = new int[5];
    public bool[] isIngredients = new bool[5]; //재료 필요 여부에 대한
    public int needIngredientNum; //재료 갯수

    public int maxCookNum; // 최대 요리 가능 갯수

    public TextMeshProUGUI[] haveIngredientTMPs;

    public int maxSignHaveIngredient; //화면에 보여줄 최대 재료 갯수

    public bool onClick = false;

    public bool canAdd; // 재료가 충분할 시 음식을 추가 여부를 판단하기 위한 변수

    public bool freeMenu; // 재료 없이 사용할 수 있는 메뉴 인지

    Data data;
    Color color;

    [SerializeField]
    private int checkNum; //재료 갯수와 비교하기 위한 변수

    private void Start()
    {
        data = DataManager.Instance.data;

        if(!freeMenu)
            StartCoroutine(CheckIngredient());
        else canAdd = true;

        maxSignHaveIngredient = 99;
    }

    IEnumerator CheckIngredient()
    {
        checkNum = 0;

        for (int i = 0; i < data.ingredient.Length; i++)
        {
            //해당 블록의 필요 재료일 때
            if (isIngredients[i])
            {
                //현재 갖고 있는 재료가 필요한 재료보다 많다면
                if (data.ingredient[i] >= needIngredients[i])
                {
                    checkNum++;

                    int tempMaxCookNum = data.ingredient[i] / needIngredients[i];
                    
                    if(i == 0 || tempMaxCookNum < maxCookNum)
                        maxCookNum = tempMaxCookNum;



                    //텍스트 변경
                    color = Color.black;
                    haveIngredientTMPs[i].color = color;

                    haveIngredientTMPs[i].text = data.ingredient[i].ToString();

                    if (data.ingredient[i] > maxSignHaveIngredient) 
                        haveIngredientTMPs[i].text = maxSignHaveIngredient.ToString();
                }
                else
                {
                    color = Color.red;
                    haveIngredientTMPs[i].color = color;

                    haveIngredientTMPs[i].text = data.ingredient[i].ToString();
                }
            }
        }

        //만약에 재료 갯수가 모두 있다면 canAdd = true
        if (checkNum == needIngredientNum) canAdd = true;
        else canAdd = false;

        yield return null;

        StartCoroutine(CheckIngredient());
    }

    public void OnClick()
    {
        if (canAdd)
        {
            onClick = true;
        }
    }

    public void OffClick()
    {
        onClick = false;
    }
}
