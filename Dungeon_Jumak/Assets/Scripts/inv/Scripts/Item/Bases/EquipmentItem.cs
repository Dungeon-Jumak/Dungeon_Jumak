using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    //===장비 아이템 스크립트===//
    public abstract class EquipmentItem : Item
    {
        public EquipmentItemData EquipmentData { get; private set; }

        //===현재 내구도===//
        public int Durability
        {
            get => _durability;
            set
            {
                if(value < 0) value = 0;
                if(value > EquipmentData.MaxDurability)
                    value = EquipmentData.MaxDurability;

                _durability = value;
            }
        }
        private int _durability;

        public EquipmentItem(EquipmentItemData data) : base(data)
        {
            EquipmentData = data;
            Durability = data.MaxDurability;
        }
    }
}