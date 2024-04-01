using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI Manager�� �̱����� Gamemanager���� �����Ѵ�.
/// UI Manager : �⺻���� UI�˾��� ���� �ϱ� ���� ��ũ��Ʈ
/// </summary>
public class UIManager
{
    int _order = 10; //sorting�� ���� ����

    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();    //������ ���� �˾�â ����
    UI_Scene _sceneUI = null;                               // �� UI

    public GameObject Root //GameObject Ÿ���� ��Ʈ ��ȯ
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");      //UI root�� ��ȯ
            if (root == null)                                   //���� root�� ����ִٸ�
                root = new GameObject { name = "@UI_Root" };    //���ο� UI root�� ���� ��ȯ
            return root;
        }
    }

    //SetCanvas() : ĵ������ �����ϱ� ���� Method
    public void SetCanvas(GameObject go, bool sort = true) //*Parameter sort : sorting ���θ� �Ǵ��ϱ� ���� �Ķ����
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go); //ĵ���� ������Ʈ �߰� �� ��ȯ
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;  //ĵ������ ������ ��� ����
        canvas.overrideSorting = true;                      //ĵ������ sorting�� �������̵� �� �� �ֵ��� ����

        if (sort)   //argument sort�� ���� true���
        {
            canvas.sortingOrder = _order;   //sorting order ����
            _order++;                       //���� sorting�� �� ������ �� �ֵ��� _order++ ����
        }
        else
        {
            canvas.sortingOrder = 0;        //false��� sorting oreder�� 0���� ����
        }
    }

    //ShowSceneUI<T>() : �� UI�� �����ֱ� ���� Method
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name)) //���� argument�� �̸��� ����ִٸ�
            name = typeof(T).Name;      //�̸��� T�� �̸����� ����

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}"); //UI/Scene/Name ��ο� ������ ����

        T SceneUI = Util.GetOrAddComponent<T>(go);  //�� UI�� ������Ʈ �߰�
        _sceneUI = SceneUI;                         //�� UI �Ҵ�

        go.transform.SetParent(Root.transform);     //�������� �θ� ����

        return SceneUI;                             //�� UI ��ȯ
    }

    //ShowPopupUI<T>() : �˾� UI�� �����ֱ� ���� Method
    public T ShowPopupUI<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;  

        GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Util.GetOrAddComponent<T>(go);    //������Ʈ �߰�
        _popupStack.Push(popup);                    //�˾��� ���ÿ� Ǫ��

        go.transform.SetParent(Root.transform);

        return popup;
    }

    //�Ķ���Ϳ� �ش��ϴ� �˾� UI�� �ݱ� ���� Method
    public void ClosePopupUI(UI_PopUp popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup) //������ �ֻ�ܿ� �ִ� �˾� UI �� argument�� ���� �ٸ��ٸ� ���� ���� ����
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI(); //�� �ΰ����� ��찡 �ƴ϶�� �˾� �ݱ� ����
    }

    //�˾� UI�� �ݱ� ���� Method
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popup = _popupStack.Pop();                 //�˾� ���� ��忡 �ִ� �˾��� pop
        GameManager.Resource.Destroy(popup.gameObject);     //�˾� ���ӿ�����Ʈ �ı�
        popup = null;                                       //���� �ʱ�ȭ

        _order--;                                           //sorting ���� ����
    }

    //��� �˾� UI�� �Ѳ����� �ݱ� ���� Method
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0) //���� ����� 0�� �ɶ����� �ݺ�
            ClosePopupUI();
    }
}
