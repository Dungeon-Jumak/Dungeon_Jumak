using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Jumak;

    //---만약 씬에서 EventSystem이 없는 경우에 prefabs 폴더에 만들어놓은 EventSystem 생성---//
    //이거 다시 확인해볼게
    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if(obj != null) {
            GameManager.Resource.Instantiate("EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();
}
