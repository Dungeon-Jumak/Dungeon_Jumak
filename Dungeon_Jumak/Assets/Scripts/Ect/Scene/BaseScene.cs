using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Jumak;

    protected virtual void Init()
    {
        //추가하고 싶은 부분 있으면 추가해도 됨.
    }

    //public abstract void Clear();
}
