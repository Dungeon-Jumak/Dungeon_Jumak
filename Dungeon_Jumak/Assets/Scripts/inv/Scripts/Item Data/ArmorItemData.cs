using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
namespace DJ.InventorySystem
{
    //===방어구 아이템 데이터===//
    [CreateAssetMenu(fileName = "Item_Armor_", menuName = "Inventory System/Item Data/Armor", order = 2)]
    public class ArmorItemData : EquipmentItemData
    {
        //===방어력 값 설정===//
        public int Defence => _defence;

        [SerializeField] private int _defence = 1;

        public override Item CreateItem()
        {
            return new ArmorItem(this);
        }
    }
}