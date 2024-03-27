using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public void OnPointerClick(PointerEventData eventData) //Ŭ�� �̺�Ʈ �������̵�
    {
        if(OnClickHandler != null)
            OnClickHandler.Invoke(eventData); //Ŭ���� ���õ� �׼� ����
    }

    public void OnDrag(PointerEventData eventData) //�巡�� �̺�Ʈ �������̵�
    {
        if(OnDragHandler != null)
            OnDragHandler.Invoke(eventData); //�巡�׿� ���õ� �׼� ����
    }

}
