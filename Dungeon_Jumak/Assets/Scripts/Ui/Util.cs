using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// FindChild()를 통해 Bind()가 UI를 찾을 수 있도록 함
/// GameObject를 찾는 경우에는 GameObject는 컴포넌트의 형식을 가질 수 없으므로 오버로딩을 통해 제네릭을 갖지 않도록 하는 함수 또한 작성
/// 
/// GetOrAddComponet()는 제네릭 함수로 선언해 컴포넌트를 추가할 수 있도록 작성
/// 
/// 자동화 UI의 기능성 함수들을 모아 놓는 스크립트
/// </summary>
public class Util
{
    //GetOrAddComponet : 게임 오브젝트에 원하는 컴포넌트를 추가하기 위한 Method
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null) component = go.AddComponent<T>(); //컴포넌트가 비어있다면 컴포넌트 추가
        return component; //컴포넌트 반환
    }

    //GameObject FindChild : GameObject 타입의 자식 오브젝트를 찾기 위한 Method
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) return null;
        return transform.gameObject;
    }

    //FindChild (Generic Method) : T에 해당하는 타입의 자식 오브젝트를 찾기 위한 제네릭 메소드 (GameObject FindChild의 기본이 됨)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) //만약 게임 오브젝트가 없다면 즉시 리턴 - 아래 코드를 실행할 필요가 없음
            return null;

        //만약 argument recursive의 값이 false라면
        if (recursive == false)
        {
            //게임 오브젝트의 자식 갯수 만큼 반복
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i); //해당 인덱스에 해당하는 자식을 불러옴
                if (string.IsNullOrEmpty(name) || transform.name == name) //만약 이름이 없거나 불러온 자식의 이름과 같다면
                {
                    T componet = transform.GetComponent<T>();   // T 컴포넌트 추가
                    if (componet != null)                       // 만약 컴포넌트가 비어 있지 않다면
                        return componet;                        // 컴포넌트가 적용된 T 오브젝트 리턴
                }
            }
        }
        else //recursive의 값이 true라면
        {
            foreach (T component in go.GetComponentsInChildren<T>()) //GetComponetsInChildren<T> : T에 해당하는 컴포넌트를 갖고 있는 자식 오브젝트를 불러옴
            {
                if (string.IsNullOrEmpty(name) || component.name == name) //만약 이름이 없거나 불러온 자식의 이름과 같다면
                {
                    return component; //해당 컴포넌트 리턴
                }
            }
        }
        return null;
    }
}
