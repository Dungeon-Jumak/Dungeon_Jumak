using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DJ.InventorySystem
{
    public abstract class ItemData : ScriptableObject
    {
        public int ID => _id;
        public string Name => _name;
        public string Tooltip => _tooltip;
        public Sprite IconSprite => _iconSprite;

        [SerializeField] private int      _id; //===아이템 ID===//
        [SerializeField] private string   _name; //===아이템 이름===//
        [Multiline][SerializeField] private string   _tooltip; //===아이템 설명===//
        [SerializeField] private Sprite   _iconSprite; //===아이템 이미지===//

        //===해당 타입에 맞는 새로운 아이템 생성===//
        public abstract Item CreateItem();
    }
}