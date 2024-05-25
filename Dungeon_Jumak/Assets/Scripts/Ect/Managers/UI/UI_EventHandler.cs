using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler
{
    public Action<PointerEventData> onClickHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClickHandler != null) onClickHandler.Invoke(eventData);
    }
}
