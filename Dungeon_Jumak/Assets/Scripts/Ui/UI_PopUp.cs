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
    enum Buttons
    {
        CloseButton
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(ClosePopUp);
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

    //---��� �˾��� �����ϴ� CLosePopUp---//
    public void ClosePopUp(PointerEventData data)
    {
        Debug.Log("���� �˾��� �ݽ��ϴ�");
        GameManager.UI.ClosePopupUI();
    }
}