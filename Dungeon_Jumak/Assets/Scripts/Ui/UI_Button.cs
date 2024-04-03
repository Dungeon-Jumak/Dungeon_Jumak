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
        OptionButton
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

        //---�ɼ� �˾��� ���� ��ư�� �ҷ���---//
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(ShowOptionPopUp);

        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;                                                  //ItemIcon�� �ش��ϴ� �̹��� go�� ��ȯ
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);  //���ٽ��� ���� �̺�Ʈ ����
    }
  
    //---�ɼ� �˾� ���---//
    public void ShowOptionPopUp(PointerEventData data)
    {
        Debug.Log("�ɼ� �˾�");
        GameManager.UI.ShowPopupUI<UI_PopUp>("OptionPopup");

    }

}
