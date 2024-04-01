using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_PopUp : UI_Base
{
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