using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI Manager는 싱글톤인 Gamemanager에서 관리한다.
/// UI Manager : 기본적이 UI팝업을 관리 하기 위한 스크립트
/// </summary>
public class UIManager
{
    int _order = 10; //sorting을 위한 변수

    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();    //스택을 통해 팝업창 관리
    UI_Scene _sceneUI = null;                               // 씬 UI

    public GameObject Root //GameObject 타입의 루트 반환
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");      //UI root를 반환
            if (root == null)                                   //만약 root이 비어있다면
                root = new GameObject { name = "@UI_Root" };    //새로운 UI root을 만들어서 반환
            return root;
        }
    }

    //SetCanvas() : 캔버스를 설정하기 위한 Method
    public void SetCanvas(GameObject go, bool sort = true) //*Parameter sort : sorting 여부를 판단하기 위한 파라미터
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go); //캔버스 컴포넌트 추가 후 반환
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;  //캔버스의 렌더러 모드 설정
        canvas.overrideSorting = true;                      //캔버스의 sorting을 오버라이딩 할 수 있도록 설정

        if (sort)   //argument sort의 값이 true라면
        {
            canvas.sortingOrder = _order;   //sorting order 변경
            _order++;                       //다음 sorting이 더 높아질 수 있도록 _order++ 실행
        }
        else
        {
            canvas.sortingOrder = 0;        //false라면 sorting oreder를 0으로 설정
        }
    }

    //ShowSceneUI<T>() : 씬 UI를 보여주기 위한 Method
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name)) //만약 argument의 이름이 비어있다면
            name = typeof(T).Name;      //이름을 T의 이름으로 설정

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}"); //UI/Scene/Name 경로에 프리팹 생성

        T SceneUI = Util.GetOrAddComponent<T>(go);  //씬 UI에 컴포넌트 추가
        _sceneUI = SceneUI;                         //씬 UI 할당

        go.transform.SetParent(Root.transform);     //프리팹의 부모 설정

        return SceneUI;                             //씬 UI 반환
    }

    //ShowPopupUI<T>() : 팝업 UI를 보여주기 위한 Method
    public T ShowPopupUI<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;  

        GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Util.GetOrAddComponent<T>(go);    //컴포넌트 추가
        _popupStack.Push(popup);                    //팝업을 스택에 푸쉬

        go.transform.SetParent(Root.transform);

        return popup;
    }

    //파라미터에 해당하는 팝업 UI를 닫기 위한 Method
    public void ClosePopupUI(UI_PopUp popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup) //스택의 최상단에 있는 팝업 UI 가 argument의 값과 다르다면 오류 문구 실행
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI(); //위 두가지의 경우가 아니라면 팝업 닫기 실행
    }

    //팝업 UI를 닫기 위한 Method
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popup = _popupStack.Pop();                 //팝업 스택 헤드에 있는 팝업을 pop
        GameManager.Resource.Destroy(popup.gameObject);     //팝업 게임오브젝트 파괴
        popup = null;                                       //변수 초기화

        _order--;                                           //sorting 변수 감소
    }

    //모든 팝업 UI를 한꺼번에 닫기 위한 Method
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0) //스택 사이즈가 0이 될때까지 반복
            ClosePopupUI();
    }
}
