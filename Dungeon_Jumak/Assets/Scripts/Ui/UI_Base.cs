using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    //Dictionary 정의
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    //추상 클래스
    public abstract void Init();

    //UI_Button은 UI_Bae를 상속 받고 캔버스 UI 프리팹들에 붙을 스크립트들은 모두 UI_Base를 상속 받으므로 각각 본인들의 _objects에서 자신들을 구성하는 오브젝트를 바인딩하여 담게됨
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; //_objects Dictionary에 Value로 담기 위한 배열
        _objects.Add(typeof(T), objects); //Dictionary에 추가

        for(int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"{names[i]} 바인딩 실패!");
        }
    }

    protected T Get<T>(int idx) where T: UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); } //오브젝트로서 가져오기
    protected Text GetText(int idx) { return Get<Text>(idx); }  //텍스트로서 가져오기
    protected Button GetButton(int idx) { return Get<Button>(idx); }  //버튼으로서 가져오기
    protected Image GetImage(int idx) { return Get<Image>(idx); } //이미지로서 가져오기

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
