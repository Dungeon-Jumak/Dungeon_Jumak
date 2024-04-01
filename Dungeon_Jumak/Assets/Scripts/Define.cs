using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Define : MonoBehaviour
{
    //---SceneManager�� ���� Scene�� Enum Ÿ�� ����---//
    //Scene ���� ����� ���⿡ �̸� �߰����ֱ�. �׷��� SceneManager���� ��� ����

    public enum Scene
    {
        Jumak,
        ComingSoon,
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
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
