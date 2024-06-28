using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    public abstract class EquipmentItemData : ItemData
    {
        //===최대 내구도 설정===//
        public int MaxDurability => _maxDurability;

        [SerializeField] private int _maxDurability = 100;
    }
}