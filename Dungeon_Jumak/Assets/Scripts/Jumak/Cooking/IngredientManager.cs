using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 재료를 추가하고 관리하기 위한 스크립트
/// </summary>
public class IngredientManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro[] gukbabTMPs;
    [SerializeField]
    private TextMeshPro[] pajeonTMPs;
    [SerializeField]
    private TextMeshPro[] riceJuiceTMPs;

    private Data data;
    private CookingFood cookingFood;

    private void Start()
    {
        data = DataManager.Instance.data;

        cookingFood = GetComponent<CookingFood>();
    }

    private void Update()
    {
        switch (cookingFood.category)
        {
            case "Gukbab":
                break;

            default:
                break;
        }
    }

    //국밥 팝업 리스트 업
    private void ListUpGukbab()
    {
        for (int idx = 0; idx < gukbabTMPs.Length; idx++)
        {
            switch (idx)
            {
                case 0: //국밥 
                    gukbabTMPs[idx].text = "";
                    break;

                case 1:
                    gukbabTMPs[idx].text = data.ingredient[0].ToString() + " / 2";
                    break;
            }
        }

    }

    

}
