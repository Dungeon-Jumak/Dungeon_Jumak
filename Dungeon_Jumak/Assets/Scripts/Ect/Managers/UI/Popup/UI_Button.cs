using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        Start_Button,
        Option_Button,
        Encyclopedia_Button,
    }

    enum Texts
    {
        Start_Text,
    }

    enum GameObjects
    {
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        //Bind<GameObject>(typeof(GameObjects));

        GetText((int)Texts.Start_Text).text = "WOW";
        GetButton((int)Buttons.Option_Button).gameObject.AddUIEvent(OnButtonClicked);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("fdfdf");
    }
}
