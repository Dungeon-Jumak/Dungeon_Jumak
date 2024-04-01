using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Define : MonoBehaviour
{
    //---SceneManager를 위한 Scene들 Enum 타입 선언---//
    //Scene 새로 생기면 여기에 이름 추가해주기. 그래야 SceneManager에서 사용 가능

    /// <summary>
    /// Define에는 이벤트 Scene등 따로 정의할 것을 열거형으로 선언할 것!
    /// </summary>

    public enum Scene
    {
        Jumak,
        ComingSoon,
    }

    //---UI 클릭과 드래그를 구분하기 위해 Define.cs UIEvent enum을 추가---//
    public enum UIEvent
    {
        Click,
        Drag,
    }

    //---마우스 이벤트를 구분하기 위해 Define.cs MouseEvent enum을 추가---//
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
