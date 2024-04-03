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

    //Virtual Method (가상 함수) : 파생 클래스에서 필요에 따라 재정의 할 수 있음
    public virtual void Init()
    {
        GameManager.UI.SetCanvas(gameObject, true);
    }
    public virtual void ClosePopupUI()
    {
        GameManager.UI.ClosePopupUI(this);
    }

    //---모든 팝업에 존재하는 CLosePopUp---//
    public void ClosePopUp(PointerEventData data)
    {
        Debug.Log("상위 팝업을 닫습니다");
        GameManager.UI.ClosePopupUI();
    }
}