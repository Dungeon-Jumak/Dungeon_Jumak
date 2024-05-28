using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetMenu : MonoBehaviour
{
    public int number;
    public int[] needIngredients = new int[5];
    public bool[] isIngredients = new bool[5]; //��� �ʿ� ���ο� ����
    public int needIngredientNum; //��� ����

    public int maxCookNum; // �ִ� �丮 ���� ����

    public TextMeshProUGUI[] haveIngredientTMPs;

    public int maxSignHaveIngredient; //ȭ�鿡 ������ �ִ� ��� ����

    public bool onClick = false;

    public bool canAdd; // ��ᰡ ����� �� ������ �߰� ���θ� �Ǵ��ϱ� ���� ����

    public bool freeMenu; // ��� ���� ����� �� �ִ� �޴� ����

    Data data;
    Color color;

    [SerializeField]
    private int checkNum; //��� ������ ���ϱ� ���� ����

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
            //�ش� ����� �ʿ� ����� ��
            if (isIngredients[i])
            {
                //���� ���� �ִ� ��ᰡ �ʿ��� ��Ẹ�� ���ٸ�
                if (data.ingredient[i] >= needIngredients[i])
                {
                    checkNum++;

                    int tempMaxCookNum = data.ingredient[i] / needIngredients[i];
                    
                    if(i == 0 || tempMaxCookNum < maxCookNum)
                        maxCookNum = tempMaxCookNum;



                    //�ؽ�Ʈ ����
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

        //���࿡ ��� ������ ��� �ִٸ� canAdd = true
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
