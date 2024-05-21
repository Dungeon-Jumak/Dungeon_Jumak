using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Jumak;

    protected virtual void Init()
    {
        //�߰��ϰ� ���� �κ� ������ �߰��ص� ��.
    }

    //public abstract void Clear();
}
