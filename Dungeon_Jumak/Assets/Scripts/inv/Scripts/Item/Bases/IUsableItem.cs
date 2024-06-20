using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    //===사용 가능한 아이템 스크립트===//
    public interface IUsableItem
    {
        //===아이템 사용하기(사용 성공 여부 값 반환)===//
        bool Use();
    }
}