using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    //---SceneManager를 위한 Scene들 Enum 타입 선언---//
    //Scene 새로 생기면 여기에 이름 추가해주기
    //그래야 SceneManager에서 사용 가능
    public enum Scene
    {
        Jumak,
        ComingSoon,
    }
}
