//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MenuUnlockManager : MonoBehaviour
{
    [Header("해금 시 Interactable할 버튼 배열")]
    [SerializeField] private Button[] menuButton;

    [Header("버튼 잠금 이미지 배열")]
    [SerializeField] private Image[] lockSprites;

    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < data.unlockMenuIndex.Length; i++)
        {
            if (!data.unlockMenuIndex[i]) continue;

            menuButton[i].interactable = true;
            lockSprites[i].sprite = null;
        }

    }
}
