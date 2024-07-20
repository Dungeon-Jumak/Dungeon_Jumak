using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace DJ.InventorySystem
{
    public class ItemSlotUI : MonoBehaviour
    {
        [Header("슬롯 내에서 아이콘과 슬롯 사이의 여백")]
        [SerializeField] private float padding = 1f;

        [Header("아이템 아이콘 이미지")]
        [SerializeField] private Image iconImage;

        [Header("아이템 개수 표시 텍스트")]
        [SerializeField] private TextMeshProUGUI amountText;

        [Space]

        [Header("슬롯이 포커스될 때 나타나는 하이라이트 이미지")]
        [SerializeField] private Image highlightImage;

        [Header("하이라이트 이미지 Alpha 값")]
        [SerializeField] private float highlightAlpha = 0.5f;

        [Header("하이라이트 등장 소요 시간")]
        [SerializeField] private float highlightFadeDuration = 0.2f;

        //===슬롯 인덱스===//
        public int Index { get; private set; }

        //===슬롯에 아이템이 있는지 여부===//
        public bool HasItem => iconImage.sprite != null;

        //===접근 가능한 슬롯인지 여부===//
        public bool IsAccessible => isAccessibleSlot && isAccessibleItem;

        public RectTransform SlotRect => slotRect;
        public RectTransform IconRect => iconRect;

        private InventoryUI inventoryUI;

        private RectTransform slotRect;
        private RectTransform iconRect;
        private RectTransform highlightRect;

        private GameObject iconGo;
        private GameObject textGo;
        private GameObject highlightGo;

        private Image slotImage;

        //===현재 하이라이트 Alpha 값===//
        private float currentHLAlpha = 0f;

        //===슬롯 접근 가능 여부===//
        private bool isAccessibleSlot = true;

        //===아이템 접근 가능 여부===//
        private bool isAccessibleItem = true; 

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
            inventoryUI = GetComponentInParent<InventoryUI>();

            //===Rects===//
            slotRect = GetComponent<RectTransform>();
            iconRect = iconImage.rectTransform;
            highlightRect = highlightImage.rectTransform;

            //===Game Objects===//
            iconGo = iconRect.gameObject;
            textGo = amountText.gameObject;
            highlightGo = highlightImage.gameObject;

            //===Images===//
            slotImage = GetComponent<Image>();
        }

        //===UI 요소 초기 값 설정===//
        private void InitValues()
        {
            iconRect.pivot = new Vector2(0.5f, 0.5f); //===Pivot : Middle===//
            iconRect.anchorMin = Vector2.zero; //===Anchor : Top Left===//
            iconRect.anchorMax = Vector2.one;

            //===Padding 값 조정===//
            iconRect.offsetMin = Vector2.one * (padding);
            iconRect.offsetMax = Vector2.one * (-padding);

            //===아이콘 & 하이라이트 크기 통일==//
            highlightRect.pivot = iconRect.pivot;
            highlightRect.anchorMin = iconRect.anchorMin;
            highlightRect.anchorMax = iconRect.anchorMax;
            highlightRect.offsetMin = iconRect.offsetMin;
            highlightRect.offsetMax = iconRect.offsetMax;

            iconImage.raycastTarget = false;
            highlightImage.raycastTarget = false;

            HideIcon();
            highlightGo.SetActive(false);
        }

        private void ShowIcon() => iconGo.SetActive(true);
        private void HideIcon() => iconGo.SetActive(false);

        private void ShowText() => textGo.SetActive(true);
        private void HideText() => textGo.SetActive(false);

        public void SetSlotIndex(int index) => Index = index;

        //===슬롯 활성화/비활성화 여부 설정===//
        public void SetSlotAccessibleState(bool value)
        {
            //===중복 처리 시===//
            if (isAccessibleSlot == value) return;

            if (value)
            {
                slotImage.color = Color.black;
            }
            else
            {
                slotImage.color = InaccessibleSlotColor;
                HideIcon();
                HideText();
            }

            isAccessibleSlot = value;
        }

        //===아이템 활성화/비활성화 여부 설정===//
        public void SetItemAccessibleState(bool value)
        {
            //===중복 처리 시===//
            if (isAccessibleItem == value) return;

            if (value)
            {
                iconImage.color = Color.white;
                amountText.color = Color.white;
            }
            else
            {
                iconImage.color = InaccessibleIconColor;
                amountText.color = InaccessibleIconColor;
            }

            isAccessibleItem = value;
        }

        //===아이템 아이콘 교환===//
        public void SwapOrMoveIcon(ItemSlotUI other)
        {
            if (other == null) return;
            if (other == this) return; //===자기 자신과 교환 불가===//
            if (!this.IsAccessible) return;
            if (!other.IsAccessible) return;

            var temp = iconImage.sprite;

            //===대상에 아이템이 있는 경우 : 교환===//
            if (other.HasItem) SetItem(other.iconImage.sprite);

            //===없는 경우 : 이동===//
            else RemoveItem();

            other.SetItem(temp);
        }

        //===해당 슬롯에 아이템 등록===//
        public void SetItem(Sprite itemSprite)
        {
            if (itemSprite != null)
            {
                iconImage.sprite = itemSprite;
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
            iconImage.sprite = null;
            HideIcon();
            HideText();
        }

        //===아이템 이미지 Alpha 값 설정===//
        public void SetIconAlpha(float alpha)
        {
            iconImage.color = new Color(
                iconImage.color.r, iconImage.color.g, iconImage.color.b, alpha
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

            amountText.text = amount.ToString();
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
                highlightRect.SetAsLastSibling();
            else
                highlightRect.SetAsFirstSibling();
        }

        //===하이라이트 Alpha 값 증가===//
        private IEnumerator HighlightFadeInRoutine()
        {
            StopCoroutine(nameof(HighlightFadeOutRoutine));
            highlightGo.SetActive(true);

            float unit = highlightAlpha / highlightFadeDuration;

            for (; currentHLAlpha <= highlightAlpha; currentHLAlpha += unit * Time.deltaTime)
            {
                highlightImage.color = new Color(
                    highlightImage.color.r,
                    highlightImage.color.g,
                    highlightImage.color.b,
                    currentHLAlpha
                );

                yield return null;
            }
        }

        //===하이라이트 Alpha 값 감소===//
        private IEnumerator HighlightFadeOutRoutine()
        {
            StopCoroutine(nameof(HighlightFadeInRoutine));

            float unit = highlightAlpha / highlightFadeDuration;

            for (; currentHLAlpha >= 0f; currentHLAlpha -= unit * Time.deltaTime)
            {
                highlightImage.color = new Color(
                    highlightImage.color.r,
                    highlightImage.color.g,
                    highlightImage.color.b,
                    currentHLAlpha
                );

                yield return null;
            }

            highlightGo.SetActive(false);
        }
    }
}