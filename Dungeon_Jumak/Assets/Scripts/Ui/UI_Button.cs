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

        //enum들 각 타입에 맞게 Bind
        //Bind() : 각 타입에 맞는 오브젝트 리스트에 추가
        
        //---바인드---//
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //---이벤트 추가---//

        //---Test---//  
        //Buttons의 PointButton에 위치값 = 0 즉, idx가 0인 버튼을 반환 후 OnButtonClicked를 BindEvent함
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GetButton((int)Buttons.TestButton).gameObject.BindEvent(TestShowPopUp);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;                                                  //ItemIcon에 해당하는 이미지 go에 반환
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);  //람다식을 통해 이벤트 연결
    }
    
    //---버튼 이벤트 추가---//
    int _score = 0;

    //버튼 클릭시 점수 증가
    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }

    public void TestShowPopUp(PointerEventData data)
    {
        Debug.Log("실행성공!");
        GameManager.UI.ShowPopupUI<UI_PopUp>("FirePopup");
    }

    public void ClosePopUp(PointerEventData data)
    {
        Debug.Log("상위 팝업을 닫습니다");
        GameManager.UI.ClosePopupUI();
    }
}
