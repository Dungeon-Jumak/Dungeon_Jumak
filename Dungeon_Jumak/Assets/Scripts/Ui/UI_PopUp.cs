using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PopUp : UI_Base
{

    private void Start()
    {
    }





    //Virtual Method (���� �Լ�) : �Ļ� Ŭ�������� �ʿ信 ���� ������ �� �� ����
    public virtual void Init()
    {
        GameManager.UI.SetCanvas(gameObject, true);
    }
    public virtual void ClosePopupUI()
    {
        GameManager.UI.ClosePopupUI(this);
    }
}