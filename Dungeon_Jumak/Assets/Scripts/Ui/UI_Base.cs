using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI_Base.cs �⺻ UI Base�� ������ ��ũ��Ʈ
/// </summary>
public class UI_Base : MonoBehaviour
{
    //_objects : ��ӿ� ����� ��ųʸ� ���� 
    // Key�� Ÿ��, Value�� UnityEngine.Object[]
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();


    // Bind�Լ��� UI������ �����ϴ� �Լ���
    // Bind()�� Type�� �Ķ���ͷ� �޴� Generic Method�̸�, T�� UnityEngine.Object�� ��� �޴� Ŭ�������� �Ѵٴ� ���������� �ǹ� (where ��)
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); //�Ķ���ͷ� �޴� type�� �ش��ϴ� enum ����Ʈ�� names �迭�� string���� ������
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; //���� enum�� ������ŭ object �迭�� ����
        _objects.Add(typeof(T), objects); // ��ųʸ��� ���ε��� objects�� �߰�

        //enum�� ������ ��ŭ �ݺ�
        for (int i = 0; i < names.Length; i++)
        {
            //Ÿ���� GameObject Ÿ���̶��
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true); 
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            //objects�� ��� �ִ� ��� = type�� �ش��ϴ� enum�� �ƹ��͵� ��� ���� �ʴٴ� �ǹ��̹Ƿ� Bind���ж�� ������ ���
            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    //Get<T>()�� ���� ������ ������
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false) //TryGetValu : key�� �ش��ϴ� value ��ȯ �ִٸ� true ��ȯ
            return null; //�ش� ��ųʸ��� key���� �����Ǵ� value�� ���ǵǾ� ���� �ʴٸ� ����

        return objects[idx] as T; //�ش� Ÿ�����μ� idx�� �ش��ϴ� object��ȯ
    }

    //idx�� �ش��ϴ� �ش� Ÿ���� object ��ȯ
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }    //idx�� �ش��ϴ� GameObject ��ȯ
    protected Text GetText(int idx) { return Get<Text>(idx); }                  //idx�� �ش��ϴ� Text ��ȯ
    protected Button GetButton(int idx) { return Get<Button>(idx); }            //idx�� �ش��ϴ� Button ��ȯ
    protected Image GetImage(int idx) { return Get<Image>(idx); }               //idx�� �ش��ϴ� Image ��ȯ

    //BindEvent�� ���� UI �̺�Ʈ�� ������ ��
    //�Ķ���ʹ� GameObject, action, Define�� ���� �Ǿ� �ִ� UIEvent�� Ÿ���� �� default parameter�� Click!
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go); //UI_EventHandler�� go�� �߰��� evt ���� �߰�
        //Util.GetOrAddComponent<T>(GameObject go) : ���� ������Ʈ�� T�� �߰��� ��ȯ

        switch (type) //�ش� UIEvent�� ����
        {
            case Define.UIEvent.Click: //Ŭ���� ��� 
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag: //�巡���� ���
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
           //�Ʒ��� UIEvent�� �߰��ϸ� ��
        }
    }

}
