using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    public class IngredientsItem : CountableItem, IUsableItem
    {
        public IngredientsItem(IngredientsItemData data, int amount = 1) : base(data, amount) { }

        //===사용 로직===//
        public bool Use() // 여기 로직 수정해서 주막에서 쓴 값만큼 빼면...
        {
            Amount--;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new IngredientsItem(CountableData as IngredientsItemData, amount);
        }
    }
}