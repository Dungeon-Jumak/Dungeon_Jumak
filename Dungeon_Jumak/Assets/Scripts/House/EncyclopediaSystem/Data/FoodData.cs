using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food_", menuName = "Encyclopedia/Data/Food", order = 2)]
public class FoodData : ScriptableObject
{
    public int foodId;
    public string foodName;
    public bool unlock = false;
}
