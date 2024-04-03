using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 버튼에 관한 스크립트
/// 확장 메소드 기능을 통해 PointButton을 클릭할 시 OnButtonCliked() 함수를 실행하도록 함
/// 또한 람다식을 통해 ItemIcon 이미지를 드래그할 수 있도록 함   
/// </summary>
public class UI_Button : UI_PopUp   
{
    // 버튼과 관련한 요소들 enum에 추가
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

        //enum들 각 타입에 맞게 Bind
        //Bind() : 각 타입에 맞는 오브젝트 리스트에 추가
        
        //---바인드---//
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //---이벤트 추가---//

        //---옵션 팝업을 띄우는 버튼을 불러옴---//
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(ShowOptionPopUp);

        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;                                                  //ItemIcon에 해당하는 이미지 go에 반환
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);  //람다식을 통해 이벤트 연결
    }
  
    //---옵션 팝업 띄움---//
    public void ShowOptionPopUp(PointerEventData data)
    {
        Debug.Log("옵션 팝업");
        GameManager.UI.ShowPopupUI<UI_PopUp>("OptionPopup");

    }

}
