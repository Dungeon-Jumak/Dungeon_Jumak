using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI_Base.cs 기본 UI Base를 가져올 스크립트
/// </summary>
public class UI_Base : MonoBehaviour
{
    //_objects : 상속에 사용할 딕셔너리 변수 
    // Key는 타입, Value는 UnityEngine.Object[]
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();


    // Bind함수는 UI정보를 저장하는 함수임
    // Bind()는 Type을 파라미터로 받는 Generic Method이며, T는 UnityEngine.Object를 상속 받는 클래스여야 한다는 제약조건을 의미 (where 절)
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); //파라미터로 받는 type에 해당하는 enum 리스트를 names 배열에 string으로 저장함
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; //얻은 enum의 갯수만큼 object 배열을 생성
        _objects.Add(typeof(T), objects); // 딕셔너리에 바인드한 objects들 추가

        //enum의 사이즈 만큼 반복
        for (int i = 0; i < names.Length; i++)
        {
            //타입이 GameObject 타입이라면
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true); 
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            //objects가 비어 있는 경우 = type에 해당하는 enum에 아무것도 들어 있지 않다는 의미이므로 Bind실패라는 문구를 띄움
            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    //Get<T>()을 통해 정보를 가져옴
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false) //TryGetValu : key에 해당하는 value 반환 있다면 true 반환
            return null; //해당 딕셔너리에 key값에 연관되는 value가 정의되어 있지 않다면 리턴

        return objects[idx] as T; //해당 타입으로서 idx에 해당하는 object반환
    }

    //idx에 해당하는 해당 타입의 object 반환
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }    //idx에 해당하는 GameObject 반환
    protected Text GetText(int idx) { return Get<Text>(idx); }                  //idx에 해당하는 Text 반환
    protected Button GetButton(int idx) { return Get<Button>(idx); }            //idx에 해당하는 Button 반환
    protected Image GetImage(int idx) { return Get<Image>(idx); }               //idx에 해당하는 Image 반환

    //BindEvent를 통해 UI 이벤트의 구독을 함
    //파라미터는 GameObject, action, Define에 정의 되어 있는 UIEvent의 타입이 들어감 default parameter는 Click!
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go); //UI_EventHandler를 go에 추가한 evt 변수 추가
        //Util.GetOrAddComponent<T>(GameObject go) : 게임 오브젝트에 T를 추가해 반환

        switch (type) //해당 UIEvent에 대해
        {
            case Define.UIEvent.Click: //클릭일 경우 
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag: //드래그일 경우
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
           //아래에 UIEvent를 추가하면 됨
        }
    }

}
