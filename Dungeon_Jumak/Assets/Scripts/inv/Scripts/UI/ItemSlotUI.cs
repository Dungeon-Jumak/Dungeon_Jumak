using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DJ.InventorySystem
{
    public class ItemSlotUI : MonoBehaviour
    {
        [Header("슬롯 내에서 아이콘과 슬롯 사이의 여백")]
        [SerializeField] private float _padding = 1f;

        [Header("아이템 아이콘 이미지")]
        [SerializeField] private Image _iconImage;

        [Header("아이템 개수 표시 텍스트")]
        [SerializeField] private Text _amountText;

        [Header("슬롯이 포커스될 때 나타나는 하이라이트 이미지")]
        [SerializeField] private Image _highlightImage;

        [Space]
        [Header("하이라이트 이미지 Alpha 값")]
        [SerializeField] private float _highlightAlpha = 0.5f;

        [Header("하이라이트 등장 소요 시간")]
        [SerializeField] private float _highlightFadeDuration = 0.2f;

        //===슬롯 인덱스===//
        public int Index { get; private set; }

        //===슬롯에 아이템이 있는지 여부===//
        public bool HasItem => _iconImage.sprite != null;

        //===접근 가능한 슬롯인지 여부===//
        public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;

        public RectTransform SlotRect => _slotRect;
        public RectTransform IconRect => _iconRect;

        private InventoryUI _inventoryUI;

        private RectTransform _slotRect;
        private RectTransform _iconRect;
        private RectTransform _highlightRect;

        private GameObject _iconGo;
        private GameObject _textGo;
        private GameObject _highlightGo;

        private Image _slotImage;

        //===현재 하이라이트 Alpha 값===//
        private float _currentHLAlpha = 0f;

        //===슬롯 접근 가능 여부===//
        private bool _isAccessibleSlot = true;

        //===아이템 접근 가능 여부===//
        private bool _isAccessibleItem = true; 

        //===비활성화된 슬롯의 색상===//
        private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);

        //===비활성화된 아이콘 색상===//
        private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        private void Awake()
        {
            InitComponents();
            InitValues();
        }

        //===UI 요소 초기화===//
        private void InitComponents()
        {
            _inventoryUI = GetComponentInParent<InventoryUI>();

            //===Rects===//
            _slotRect = GetComponent<RectTransform>();
            _iconRect = _iconImage.rectTransform;
            _highlightRect = _highlightImage.rectTransform;

            //===Game Objects===//
            _iconGo = _iconRect.gameObject;
            _textGo = _amountText.gameObject;
            _highlightGo = _highlightImage.gameObject;

            //===Images===//
            _slotImage = GetComponent<Image>();
        }

        //===UI 요소 초기 값 설정===//
        private void InitValues()
        {
            _iconRect.pivot = new Vector2(0.5f, 0.5f); //===Pivot : Middle===//
            _iconRect.anchorMin = Vector2.zero;        //===Anchor : Top Left===//
            _iconRect.anchorMax = Vector2.one;

            //===Padding 값 조정===//
            _iconRect.offsetMin = Vector2.one * (_padding);
            _iconRect.offsetMax = Vector2.one * (-_padding);

            //===아이콘 & 하이라이트 크기 통일==//
            _highlightRect.pivot = _iconRect.pivot;
            _highlightRect.anchorMin = _iconRect.anchorMin;
            _highlightRect.anchorMax = _iconRect.anchorMax;
            _highlightRect.offsetMin = _iconRect.offsetMin;
            _highlightRect.offsetMax = _iconRect.offsetMax;

            _iconImage.raycastTarget = false;
            _highlightImage.raycastTarget = false;

            HideIcon();
            _highlightGo.SetActive(false);
        }

        private void ShowIcon() => _iconGo.SetActive(true);
        private void HideIcon() => _iconGo.SetActive(false);

        private void ShowText() => _textGo.SetActive(true);
        private void HideText() => _textGo.SetActive(false);

        public void SetSlotIndex(int index) => Index = index;

        //===슬롯 활성화/비활성화 여부 설정===//
        public void SetSlotAccessibleState(bool value)
        {
            //===중복 처리 시===//
            if (_isAccessibleSlot == value) return;

            if (value)
            {
                _slotImage.color = Color.black;
            }
            else
            {
                _slotImage.color = InaccessibleSlotColor;
                HideIcon();
                HideText();
            }

            _isAccessibleSlot = value;
        }

        //===아이템 활성화/비활성화 여부 설정===//
        public void SetItemAccessibleState(bool value)
        {
            //===중복 처리 시===//
            if (_isAccessibleItem == value) return;

            if (value)
            {
                _iconImage.color = Color.white;
                _amountText.color = Color.white;
            }
            else
            {
                _iconImage.color = InaccessibleIconColor;
                _amountText.color = InaccessibleIconColor;
            }

            _isAccessibleItem = value;
        }

        //===다른 슬롯의 아이템 아이콘 교환===//
        public void SwapOrMoveIcon(ItemSlotUI other)
        {
            if (other == null) return;
            if (other == this) return; //===자기 자신과 교환 불가===//
            if (!this.IsAccessible) return;
            if (!other.IsAccessible) return;

            var temp = _iconImage.sprite;

            //===대상에 아이템이 있는 경우 : 교환===//
            if (other.HasItem) SetItem(other._iconImage.sprite);

            //===없는 경우 : 이동===//
            else RemoveItem();

            other.SetItem(temp);
        }

        //===해당 슬롯에 아이템 등록===//
        public void SetItem(Sprite itemSprite)
        {
            if (itemSprite != null)
            {
                _iconImage.sprite = itemSprite;
                ShowIcon();
            }
            else
            {
                RemoveItem();
            }
        }

        //===해당 슬롯에서 아이템 제거===//
        public void RemoveItem()
        {
            _iconImage.sprite = null;
            HideIcon();
            HideText();
        }

        //===아이템 이미지 Alpha 값 설정===//
        public void SetIconAlpha(float alpha)
        {
            _iconImage.color = new Color(
                _iconImage.color.r, _iconImage.color.g, _iconImage.color.b, alpha
            );
        }

        //===아이템 개수 텍스트 설정===//
        public void SetItemAmount(int amount)
        {
            if (HasItem && amount > 1)
                ShowText();

            //===amount가 1 이하일 경우 텍스트 미표시===//
            else
                HideText();

            _amountText.text = amount.ToString();
        }

        //===슬롯에 하이라이트 나타내기===//
        public void Highlight(bool show)
        {
            if (!this.IsAccessible) return;

            if (show)
                StartCoroutine(nameof(HighlightFadeInRoutine));
            else
                StartCoroutine(nameof(HighlightFadeOutRoutine));
        }

        //===하이라이트 이미지를 표시===//
        public void SetHighlightOnTop(bool value)
        {
            if (value)
                _highlightRect.SetAsLastSibling();
            else
                _highlightRect.SetAsFirstSibling();
        }

        //===하이라이트 Alpha 값 증가===//
        private IEnumerator HighlightFadeInRoutine()
        {
            StopCoroutine(nameof(HighlightFadeOutRoutine));
            _highlightGo.SetActive(true);

            float unit = _highlightAlpha / _highlightFadeDuration;

            for (; _currentHLAlpha <= _highlightAlpha; _currentHLAlpha += unit * Time.deltaTime)
            {
                _highlightImage.color = new Color(
                    _highlightImage.color.r,
                    _highlightImage.color.g,
                    _highlightImage.color.b,
                    _currentHLAlpha
                );

                yield return null;
            }
        }

        //===하이라이트 Alpha 값 감소===//
        private IEnumerator HighlightFadeOutRoutine()
        {
            StopCoroutine(nameof(HighlightFadeInRoutine));

            float unit = _highlightAlpha / _highlightFadeDuration;

            for (; _currentHLAlpha >= 0f; _currentHLAlpha -= unit * Time.deltaTime)
            {
                _highlightImage.color = new Color(
                    _highlightImage.color.r,
                    _highlightImage.color.g,
                    _highlightImage.color.b,
                    _currentHLAlpha
                );

                yield return null;
            }

            _highlightGo.SetActive(false);
        }
    }
}