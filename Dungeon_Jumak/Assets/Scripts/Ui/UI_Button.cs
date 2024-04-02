using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI ��ư�� ���� ��ũ��Ʈ
/// Ȯ�� �޼ҵ� ����� ���� PointButton�� Ŭ���� �� OnButtonCliked() �Լ��� �����ϵ��� ��
/// ���� ���ٽ��� ���� ItemIcon �̹����� �巡���� �� �ֵ��� ��   
/// </summary>
public class UI_Button : UI_PopUp   
{
    // ��ư�� ������ ��ҵ� enum�� �߰�
    enum Buttons
    {
        PointButton,
        TestButton
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    private void Start()
    {

        //enum�� �� Ÿ�Կ� �°� Bind
        //Bind() : �� Ÿ�Կ� �´� ������Ʈ ����Ʈ�� �߰�
        
        //---���ε�---//
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //---�̺�Ʈ �߰�---//

        //---Test---//  
        //Buttons�� PointButton�� ��ġ�� = 0 ��, idx�� 0�� ��ư�� ��ȯ �� OnButtonClicked�� BindEvent��
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GetButton((int)Buttons.TestButton).gameObject.BindEvent(TestShowPopUp);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;                                                  //ItemIcon�� �ش��ϴ� �̹��� go�� ��ȯ
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);  //���ٽ��� ���� �̺�Ʈ ����
    }
    
    //---��ư �̺�Ʈ �߰�---//
    int _score = 0;

    //��ư Ŭ���� ���� ����
    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)Texts.ScoreText).text = $"���� : {_score}";
    }

    public void TestShowPopUp(PointerEventData data)
    {
        Debug.Log("���༺��!");
        GameManager.UI.ShowPopupUI<UI_PopUp>("FirePopup");
    }

    public void ClosePopUp(PointerEventData data)
    {
        Debug.Log("���� �˾��� �ݽ��ϴ�");
        GameManager.UI.ClosePopupUI();
    }
}
