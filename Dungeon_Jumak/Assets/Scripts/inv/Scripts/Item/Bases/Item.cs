using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DJ.InventorySystem
{
    public abstract class Item
    {
        public ItemData Data { get; private set; }

        public Item(ItemData data) => Data = data;
    }
}