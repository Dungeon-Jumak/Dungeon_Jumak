using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    //===장비 아이템 데이터 (무기, 방어구)===//
    public abstract class EquipmentItemData : ItemData
    {
        //===최대 내구도 설정===//
        public int MaxDurability => _maxDurability;

        [SerializeField] private int _maxDurability = 100;
    }
}