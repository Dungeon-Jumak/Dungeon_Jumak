using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    //===장비 <- 무기===//
    public class WeaponItem : EquipmentItem
    {
        public WeaponItem(WeaponItemData data) : base(data) { }
    }
}