using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UIManager 
{
    int _order = 0; //최근에 사용한 sort order 번호

    Stack<UI_Popup> _popupStacks = new Stack<UI_Popup> ();
    UI_Scene _sceneUI = null;

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
        GameObject canvas = GameObject.Find("Canvas");
        go.transform.SetParent(canvas.transform, false);
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;
        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T: UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");
        GameObject canvas = GameObject.Find("Canvas");
        go.transform.SetParent(canvas.transform, false);
        T popup = Util.GetOrAddComponent<T> (go);
        _popupStacks.Push(popup);
        return popup;
    }

    public void ClosePopupUI()
    {
        if (_popupStacks.Count == 0) return;

        UI_Popup popup = _popupStacks.Pop();
        GameManager.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStacks.Count == 0) return;

        if(_popupStacks.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Canvas는 화면 위에 직접 렌더링되는 모드
        canvas.overrideSorting = true; // Canvas의 정렬 우선순위를 수동으로 관리함

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // popup과 관련없는 screen UI
        {
            canvas.sortingOrder = 0;
        }
    }


}
