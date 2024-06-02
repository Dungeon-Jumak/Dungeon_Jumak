using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Scene
    {
        Jumak,
        StartScene,
        WaitingScene,
        Map,
        Dunjeon,
        Market,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
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
