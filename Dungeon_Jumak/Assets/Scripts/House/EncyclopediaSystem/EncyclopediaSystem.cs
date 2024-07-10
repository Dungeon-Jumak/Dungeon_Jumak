using DJ.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaSystem : MonoBehaviour
{
    public MonsterData[] monsterDataArray;
    public SpriteRenderer[] monsterImageArray;
    private Data data;

    void Start()
    {
        data = DataManager.Instance.data;
        OnMonsterEncountered();
    }

    public void OnMonsterEncountered()
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
        }
    }
}
