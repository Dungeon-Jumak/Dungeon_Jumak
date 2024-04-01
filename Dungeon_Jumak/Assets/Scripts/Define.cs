using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Define : MonoBehaviour
{
    //---SceneManager�� ���� Scene�� Enum Ÿ�� ����---//
    //Scene ���� ����� ���⿡ �̸� �߰����ֱ�. �׷��� SceneManager���� ��� ����

    /// <summary>
    /// Define���� �̺�Ʈ Scene�� ���� ������ ���� ���������� ������ ��!
    /// </summary>

    public enum Scene
    {
        Jumak,
        ComingSoon,
    }

    //---UI Ŭ���� �巡�׸� �����ϱ� ���� Define.cs UIEvent enum�� �߰�---//
    public enum UIEvent
    {
        Click,
        Drag,
    }

    //---���콺 �̺�Ʈ�� �����ϱ� ���� Define.cs MouseEvent enum�� �߰�---//
    public enum MouseEvent
    {
        Press,
        Click,
    }
    public enum CameraMode
    {
        QuaterView,
    }


}
