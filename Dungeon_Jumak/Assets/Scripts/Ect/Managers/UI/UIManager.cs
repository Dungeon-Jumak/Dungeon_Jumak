using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UIManager 
{
    int _order = 0; //�ֱٿ� ����� sort order ��ȣ

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
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Canvas�� ȭ�� ���� ���� �������Ǵ� ���
        canvas.overrideSorting = true; // Canvas�� ���� �켱������ �������� ������

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // popup�� ���þ��� screen UI
        {
            canvas.sortingOrder = 0;
        }
    }


}
