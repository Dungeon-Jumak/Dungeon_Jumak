using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    //===소비 아이템 데이터===//
    [CreateAssetMenu(fileName = "Item_Ingredients_", menuName = "Inventory System/Item Data/Ingredients", order = 3)]
    public class IngredientsItemData : CountableItemData
    {
        public float Value => _value;
        [SerializeField] private float _value;
        public override Item CreateItem()
        {
            return new IngredientsItem(this);
        }
    }
}