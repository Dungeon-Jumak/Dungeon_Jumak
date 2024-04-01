using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// FindChild()�� ���� Bind()�� UI�� ã�� �� �ֵ��� ��
/// GameObject�� ã�� ��쿡�� GameObject�� ������Ʈ�� ������ ���� �� �����Ƿ� �����ε��� ���� ���׸��� ���� �ʵ��� �ϴ� �Լ� ���� �ۼ�
/// 
/// GetOrAddComponet()�� ���׸� �Լ��� ������ ������Ʈ�� �߰��� �� �ֵ��� �ۼ�
/// 
/// �ڵ�ȭ UI�� ��ɼ� �Լ����� ��� ���� ��ũ��Ʈ
/// </summary>
public class Util
{
    //GetOrAddComponet : ���� ������Ʈ�� ���ϴ� ������Ʈ�� �߰��ϱ� ���� Method
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null) component = go.AddComponent<T>(); //������Ʈ�� ����ִٸ� ������Ʈ �߰�
        return component; //������Ʈ ��ȯ
    }

    //GameObject FindChild : GameObject Ÿ���� �ڽ� ������Ʈ�� ã�� ���� Method
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) return null;
        return transform.gameObject;
    }

    //FindChild (Generic Method) : T�� �ش��ϴ� Ÿ���� �ڽ� ������Ʈ�� ã�� ���� ���׸� �޼ҵ� (GameObject FindChild�� �⺻�� ��)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) //���� ���� ������Ʈ�� ���ٸ� ��� ���� - �Ʒ� �ڵ带 ������ �ʿ䰡 ����
            return null;

        //���� argument recursive�� ���� false���
        if (recursive == false)
        {
            //���� ������Ʈ�� �ڽ� ���� ��ŭ �ݺ�
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i); //�ش� �ε����� �ش��ϴ� �ڽ��� �ҷ���
                if (string.IsNullOrEmpty(name) || transform.name == name) //���� �̸��� ���ų� �ҷ��� �ڽ��� �̸��� ���ٸ�
                {
                    T componet = transform.GetComponent<T>();   // T ������Ʈ �߰�
                    if (componet != null)                       // ���� ������Ʈ�� ��� ���� �ʴٸ�
                        return componet;                        // ������Ʈ�� ����� T ������Ʈ ����
                }
            }
        }
        else //recursive�� ���� true���
        {
            foreach (T component in go.GetComponentsInChildren<T>()) //GetComponetsInChildren<T> : T�� �ش��ϴ� ������Ʈ�� ���� �ִ� �ڽ� ������Ʈ�� �ҷ���
            {
                if (string.IsNullOrEmpty(name) || component.name == name) //���� �̸��� ���ų� �ҷ��� �ڽ��� �̸��� ���ٸ�
                {
                    return component; //�ش� ������Ʈ ����
                }
            }
        }
        return null;
    }
}
