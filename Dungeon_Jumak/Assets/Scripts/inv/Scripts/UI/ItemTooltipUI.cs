using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DJ.InventorySystem
{
    public class ItemTooltipUI : MonoBehaviour
    {
        [Header("아이템 이름")]
        [SerializeField]
        private Text _titleText;

        [Header("아이템 설명")]
        [SerializeField]
        private Text _contentText;

        private RectTransform rt;
        private CanvasScaler canvasScaler;

        private static readonly Vector2 LeftTop = new Vector2(0f, 1f);
        private static readonly Vector2 LeftBottom = new Vector2(0f, 0f);
        private static readonly Vector2 RightTop = new Vector2(1f, 1f);
        private static readonly Vector2 RightBottom = new Vector2(1f, 0f);

        private void Awake()
        {
            Init();
            Hide();
        }

        private void Init()
        {
            TryGetComponent(out rt);
            rt.pivot = LeftTop;
            canvasScaler = GetComponentInParent<CanvasScaler>();
            DisableAllChildrenRaycastTarget(transform);
        }

        private void DisableAllChildrenRaycastTarget(Transform tr)
        {
            tr.TryGetComponent(out Graphic gr);
            if(gr != null)
                gr.raycastTarget = false;

            int childCount = tr.childCount;
            if (childCount == 0) return;

            for (int i = 0; i < childCount; i++)
            {
                DisableAllChildrenRaycastTarget(tr.GetChild(i));
            }
        }

        //===툴팁 UI에 아이템 데이터 등록===//
        public void SetItemInfo(ItemData data)
        {
            _titleText.text = data.Name;
            _contentText.text = data.Tooltip;
        }

        //===툴팁 위치 조정===//
        public void SetRectPosition(RectTransform slotRect)
        {
            //===해상도 대응===//
            float wRatio = Screen.width / canvasScaler.referenceResolution.x;
            float hRatio = Screen.height / canvasScaler.referenceResolution.y;
            float ratio = 
                wRatio * (1f - canvasScaler.matchWidthOrHeight) +
                hRatio * (canvasScaler.matchWidthOrHeight);

            float slotWidth = slotRect.rect.width * ratio;
            float slotHeight = slotRect.rect.height * ratio;

            //===툴팁 초기 위치 설정 : 우하단===//
            rt.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
            Vector2 pos = rt.position;

            //===툴팁 크기===//
            float width = rt.rect.width * ratio;
            float height = rt.rect.height * ratio;

            //===우측, 하단이 잘렸는지 여부===//
            bool rightTruncated = pos.x + width > Screen.width;
            bool bottomTruncated = pos.y - height < 0f;

            ref bool R = ref rightTruncated;
            ref bool B = ref bottomTruncated;

            //===오른쪽만 잘림 => 슬롯의 Left Bottom 방향으로 표시===//
            if (R && !B)
            {
                rt.position = new Vector2(pos.x - width - slotWidth, pos.y);
            }
            //===아래쪽만 잘림 => 슬롯의 Right Top 방향으로 표시===//
            else if (!R && B)
            {
                rt.position = new Vector2(pos.x, pos.y + height + slotHeight);
            }
            //===모두 잘림 => 슬롯의 Left Top 방향으로 표시===//
            else if (R && B)
            {
                rt.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
            }
            //===잘리지 않음 => 슬롯의 Right Bottom(초기) 방향으로 표시===//
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}