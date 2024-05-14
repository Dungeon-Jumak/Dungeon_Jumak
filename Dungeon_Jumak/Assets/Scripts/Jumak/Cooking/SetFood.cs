using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ ������ Confirmâ�� �̹����� �����ֱ� ���� ��ũ��Ʈ
public class SetFood : MonoBehaviour
{
    //���� Ŭ���� �� ������ ����� ������ �̹���
    public Image gukbabImage;
    public Image pajeonImage;
    public Image riceJuiceImage;

    public int gukbabMaxNum;
    public int pajeonMaxNum;
    public int riceJuiceMaxNum;

    //�� ����� ����
    public int[] subGukbabIngredient;
    public int[] subPajeonIngredient;
    public int[] subRiceJuiceIngredient;

    //���� ����Ʈ�� �� ��������Ʈ�� ��ư �迭
    [SerializeField]
    private Sprite[] gukbabSprites;
    [SerializeField]
    private SetMenu[] gukbabs;

    [SerializeField]
    private Sprite[] pajeonSprites;
    [SerializeField]
    private SetMenu[] pajeons;

    [SerializeField]
    private Sprite[] riceJuiceSprites;
    [SerializeField]
    private SetMenu[] riceJuices;

    //�ѹ��� ������ ����
    [SerializeField]
    private int[] foodNums;

    private SelectFood selectFood;

    Data data;

    private void Start()
    {
        selectFood = GetComponent<SelectFood>();
        foodNums = new int[3];

        subGukbabIngredient = new int[5];
        subPajeonIngredient = new int[5];
        subRiceJuiceIngredient = new int[5];

        data = DataManager.Instance.data;
    }

    private void Update()
    {
        //�� ī�װ��� �ش��ϴ� �˾��� ���� �� �ݺ����� ������ üũ�ϵ��� ����
        switch (selectFood.category)
        {
            case "Gukbab":
                for (int i = 0; i < gukbabs.Length; i++)
                {
                    if (gukbabs[i].onClick)
                    { 
                        gukbabs[i].OffClick();

                        gukbabImage.sprite = gukbabSprites[i];

                        gukbabMaxNum = gukbabs[i].maxCookNum;

                        for (int k = 0; k < data.ingredient.Length; k++)
                        {
                            if (gukbabs[i].isIngredients[k])
                            {
                                subGukbabIngredient[k] = gukbabs[i].needIngredients[k] * gukbabMaxNum;
                            }
                        }

                    }
                }
                break;

            case "Pajeon":
                for (int i = 0; i < pajeons.Length; i++)
                {
                    if (pajeons[i].onClick)
                    {

                        pajeons[i].OffClick();
                        pajeonImage.sprite = pajeonSprites[i];

                    }
                }
                break;

            case "RiceJuice":
                for (int i = 0; i < riceJuices.Length; i++)
                {
                    if (riceJuices[i].onClick)
                    {

                        riceJuices[i].OffClick();
                        riceJuiceImage.sprite = riceJuiceSprites[i];

                    }
                }
                break;

            default:
                break;
        }
    }

}
