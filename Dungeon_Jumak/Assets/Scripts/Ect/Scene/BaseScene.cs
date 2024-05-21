using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Jumak;

    /*protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if(obj != null) {
            GameManager.Resource.Instantiate("EventSystem").name = "@EventSystem";
        }
    }*/

    //public abstract void Clear();
}
