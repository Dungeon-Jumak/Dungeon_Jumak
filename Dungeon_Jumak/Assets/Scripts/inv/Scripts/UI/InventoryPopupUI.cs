using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DJ  
{
    //===인벤토리 UI 위에 띄울 버리기 기능 팝업 관리===//
    public class InventoryPopupUI : MonoBehaviour
    {
        //===아이템 버릴 시 뜨는 팝업 요소===//
        [Header("Confirmation Popup")]
        [SerializeField] private GameObject _confirmationPopupObject;
        [SerializeField] private Text   _confirmationItemNameText;
        [SerializeField] private Text   _confirmationText;
        [SerializeField] private Button _confirmationOkButton;  
        [SerializeField] private Button _confirmationCancelButton;


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
            if (_confirmationPopupObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    _confirmationOkButton.onClick?.Invoke();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _confirmationCancelButton.onClick?.Invoke();
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
            _confirmationOkButton.onClick.AddListener(HidePanel);
            _confirmationOkButton.onClick.AddListener(HideConfirmationPopup);
            _confirmationOkButton.onClick.AddListener(() => OnConfirmationOK?.Invoke());

            _confirmationCancelButton.onClick.AddListener(HidePanel);
            _confirmationCancelButton.onClick.AddListener(HideConfirmationPopup);
        }

        private void ShowPanel() => gameObject.SetActive(true);
        private void HidePanel() => gameObject.SetActive(false);

        private void ShowConfirmationPopup(string itemName)
        {
            _confirmationItemNameText.text = itemName;
            _confirmationPopupObject.SetActive(true);
        }
        private void HideConfirmationPopup() => _confirmationPopupObject.SetActive(false);
        private void SetConfirmationOKEvent(Action handler) => OnConfirmationOK = handler;
    }
}