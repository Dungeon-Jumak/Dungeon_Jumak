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

    public bool freeGukbab;
    public bool freePajeon;
    public bool freeRiceJuice;

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

    [SerializeField]
    private GameObject[] gukbabPrefabs;
    [SerializeField]
    private GameObject[] pajeonsPrefabs;
    [SerializeField]
    private GameObject[] riceJuicePrefabs;

    [SerializeField]
    private GukbabGenerator gukbapSetting;

    private SelectFood selectFood;

    private int gukbabIdx;
    private int pajeonIdx;
    private int riceJuiceIdx;

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
                        //Ȯ�� ��ư �̹��� ����
                        gukbabImage.sprite = gukbabSprites[i];

                        gukbabMaxNum = gukbabs[i].maxCookNum;

                        if(gukbabs[i].freeMenu) freeGukbab = true;
                        else freeGukbab = false;

                        gukbabIdx = i;
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

                        pajeonMaxNum = pajeons[i].maxCookNum;

                        if (pajeons[i].freeMenu) freePajeon = true;
                        else freePajeon = false;

                        pajeonIdx = i;
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

                        riceJuiceMaxNum = riceJuices[i].maxCookNum;

                        if(riceJuices[i].freeMenu) freeRiceJuice = true;
                        else freeRiceJuice = false;

                        riceJuiceIdx = i;
                    }
                }
                break;

            default:
                break;
        }
    }

    //��Ḧ ���� ���� ��ũ��Ʈ
    public void subIngredient(string category, int count)
    {
        switch (category)
        {
            case "Gukbab":
                for (int i = 0; i < data.ingredient.Length; i++)
                {
                    if (gukbabs[gukbabIdx].isIngredients[i])
                        data.ingredient[i] -= gukbabs[gukbabIdx].needIngredients[i] * count;
                }
                gukbapSetting.gukbabPrefab = gukbabPrefabs[gukbabIdx];
                break;

            case "Pajeon":
                for (int i = 0; i < data.ingredient.Length; i++)
                {
                    if (pajeons[pajeonIdx].isIngredients[i])
                        data.ingredient[i] -= pajeons[pajeonIdx].needIngredients[i] * count;
                }
                break;

            case "RiceJuice":
                for (int i = 0; i < data.ingredient.Length; i++)
                {
                    if (riceJuices[riceJuiceIdx].isIngredients[i])
                        data.ingredient[i] -= riceJuices[riceJuiceIdx].needIngredients[i] * count;
                }
                break;
        }

    }

    public void ButtonBloker(string category)
    {
        switch (category)
        {
            case "Gukbab":
                for (int j = 0; j < gukbabs.Length; j++)
                {
                    gukbabs[j].gameObject.GetComponent<Button>().interactable = false;
                }
                break;

            case "Pajeon":
                for (int j = 0; j < pajeons.Length; j++)
                {
                    pajeons[j].gameObject.GetComponent<Button>().interactable = false;
                }
                break;

            case "RiceJuice":
                for (int j = 0; j < riceJuices.Length; j++)
                {
                    riceJuices[j].gameObject.GetComponent<Button>().interactable = false;
                }
                break;
        }
    }
}
