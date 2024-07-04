using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace DJ  
{
    public class InventoryPopupUI : MonoBehaviour
    {
        //===아이템 버릴 시 뜨는 팝업 요소===//
        [Header("버릴 시 확인 Popup")]
        [SerializeField] private GameObject confirmationPopupObject;
        [SerializeField] private TextMeshProUGUI confirmationItemNameText;
        [SerializeField] private TextMeshProUGUI confirmationText;
        [SerializeField] private Button confirmationOkButton;  
        [SerializeField] private Button confirmationCancelButton;

        //===확인 버튼 눌렀을 때 동작할 이벤트===//
        private event Action OnConfirmationOK;

        private void Awake()
        {
            InitUIEvents();
            HidePanel();
            HideConfirmationPopup();
        }

        private void Update()
        {
            if (confirmationPopupObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    confirmationOkButton.onClick?.Invoke();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    confirmationCancelButton.onClick?.Invoke();
                }
            }
        }

        //===팝업 띄우기===//
        public void OpenConfirmationPopup(Action okCallback, string itemName)
        {
            ShowPanel();
            ShowConfirmationPopup(itemName);
            SetConfirmationOKEvent(okCallback);
        }

        private void InitUIEvents()
        {
            //===확인/취소 팝업===//
            confirmationOkButton.onClick.AddListener(HidePanel);
            confirmationOkButton.onClick.AddListener(HideConfirmationPopup);
            confirmationOkButton.onClick.AddListener(() => OnConfirmationOK?.Invoke());

            confirmationCancelButton.onClick.AddListener(HidePanel);
            confirmationCancelButton.onClick.AddListener(HideConfirmationPopup);
        }

        private void ShowPanel() => gameObject.SetActive(true);
        private void HidePanel() => gameObject.SetActive(false);

        private void ShowConfirmationPopup(string itemName)
        {
            confirmationItemNameText.text = itemName;
            confirmationPopupObject.SetActive(true);
        }
        private void HideConfirmationPopup() => confirmationPopupObject.SetActive(false);
        private void SetConfirmationOKEvent(Action handler) => OnConfirmationOK = handler;
    }
}