using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    //Dictionary ����
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    //�߻� Ŭ����
    public abstract void Init();

    //UI_Button�� UI_Bae�� ��� �ް� ĵ���� UI �����յ鿡 ���� ��ũ��Ʈ���� ��� UI_Base�� ��� �����Ƿ� ���� ���ε��� _objects���� �ڽŵ��� �����ϴ� ������Ʈ�� ���ε��Ͽ� ��Ե�
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; //_objects Dictionary�� Value�� ��� ���� �迭
        _objects.Add(typeof(T), objects); //Dictionary�� �߰�

        for(int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"{names[i]} ���ε� ����!");
        }
    }

    protected T Get<T>(int idx) where T: UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); } //������Ʈ�μ� ��������
    protected Text GetText(int idx) { return Get<Text>(idx); }  //�ؽ�Ʈ�μ� ��������
    protected Button GetButton(int idx) { return Get<Button>(idx); }  //��ư���μ� ��������
    protected Image GetImage(int idx) { return Get<Image>(idx); } //�̹����μ� ��������

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;

            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

}
