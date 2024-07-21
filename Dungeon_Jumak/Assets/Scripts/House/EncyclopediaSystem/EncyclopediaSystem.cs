using DJ.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaSystem : MonoBehaviour
{
    public MonsterData[] monsterDataArray;
    public SpriteRenderer[] monsterImageArray;

    public FoodData[] foodDataArray;
    public SpriteRenderer[] foodImageArray;

    public int MonsterID;
    public int FoodID;

    private Data data;

    void Awake()
    {
        data = DataManager.Instance.data;
    }

    void OnEnable()
    {
        OnMonsterRegister();
        OnFoodRegister();
    }

    public void OnMonsterRegister()
    {
        foreach (MonsterData monsterData in monsterDataArray)
        {
            if (monsterData.unlock == true)
            {
                if (monsterData.monsterId < monsterImageArray.Length && monsterData.monsterId >= 0)
                {
                    monsterImageArray[monsterData.monsterId].color = Color.white;
                }
                break;
            }
            else
            {
                monsterImageArray[monsterData.monsterId].color = Color.black;
            }
        }
    }

    public void OnFoodRegister()
    {
        for (int i = 0; i < data.unlockMenuIndex.Length; i++)
        {
            if (data.unlockMenuIndex[i] == true)
            {
                foodDataArray[i].unlock = true;

                foreach (FoodData foodData in foodDataArray)
                {
                    if (foodData.unlock == true)
                    {
                        if (foodData.foodId < foodImageArray.Length && foodData.foodId >= 0)
                        {
                            foodImageArray[foodData.foodId].color = Color.white;
                        }
                        //break;
                    }
                    else
                    {
                        foodImageArray[foodData.foodId].color = Color.black;
                    }
                }
            }
        }
    }
}
