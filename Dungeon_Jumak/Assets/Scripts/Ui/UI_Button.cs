using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); //UI_Button�� �θ���  UI_Popup�� Init ȣ��

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

       

    }
    enum Buttons
    {
        TestButton
    }

    enum Texts
    {
        TestTex
    }

    enum GameObjects
    {
        TestObject
    }

    enum Images
    {
        TestImage
    }
}
