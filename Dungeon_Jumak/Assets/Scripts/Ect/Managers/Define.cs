using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Define : MonoBehaviour
{
    //---summary---//

    // Define에는 이벤트 Scene등 따로 정의할 것을 열거형으로 선언할 것!
    //Scene 새로 생기면 여기에 이름 추가해주기. 그래야 SceneManager에서 사용 가능
    // 씬 전환할 때 GameManager.Scene.LoadScene(Define.Scene.원하는씬이름); 코드 추가하면 됨 -> SceneManagement 안불러와도 됨


    public enum Scene
    {
        Jumak,
        StartScene,
        WaitingScene,
        Map,
        MainDunjeon,
        Market,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,//브금과 효과음들의 모든 개수
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
