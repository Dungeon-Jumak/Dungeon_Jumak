using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DJ.InventorySystem
{
    public class InventoryUI : MonoBehaviour
    { 
        [Header("슬롯 가로 개수")]
        [Range(0, 10)]
        [SerializeField] private int horizontalSlotCount = 8;

        [Header("슬롯 세로 개수")]
        [Range(0, 10)]
        [SerializeField] private int verticalSlotCount = 8;

        [Header("한 슬롯의 상하좌우 여백")]
        [SerializeField] private float slotMargin = 8f;

        [Header("인벤토리 영역의 내부 여백")]
        [SerializeField] private float contentAreaPadding = 20f;

        [Header("각 슬롯의 크기")]
        [Range(32, 64)]
        [SerializeField] private float slotSize = 64f;

        [Space]
        [SerializeField] private bool showTooltip = true;
        [SerializeField] private bool showHighlight = true;
        [SerializeField] private bool showRemovingPopup = true;

        [Header("슬롯들이 위치할 영역")]
        [SerializeField] private RectTransform contentAreaRT;

        [Header("슬롯 프리팹")]
        [SerializeField] private GameObject slotUiPrefab;

        [Header("아이템 정보 툴팁 UI")]
        [SerializeField] private ItemTooltipUI itemTooltip;

        [Header("팝업 UI 오브젝트")]
        [SerializeField] private InventoryPopupUI popup;

        [Header("버튼")]
        [SerializeField] private Button trimButton;
        [SerializeField] private Button sortButton;

        [Header("카테고리")]
        [SerializeField] private Toggle toggleFilterAll;
        [SerializeField] private Toggle toggleFilterArmors;
        [SerializeField] private Toggle toggleFilterWeapons;
        [SerializeField] private Toggle toggleFilterIngredients;

        [Space]
        [SerializeField] private bool mouseReversed = false;

        //===인벤토리===//
        private Inventory inventory;

        private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();
        private GraphicRaycaster gr;
        private PointerEventData ped;
        private List<RaycastResult> raycastList;

        //===현재 포인터가 위치한 곳의 슬롯===//
        private ItemSlotUI pointerOverSlot;

        //===현재 드래그를 시작한 슬롯===//
        private ItemSlotUI beginDragSlot;

        //===해당 슬롯의 아이콘 Transform===//
        private Transform beginDragIconTransform; 

        private int leftClick = 0;
        private int rightClick = 1;

        //===드래그 시작 시 슬롯의 위치===//
        private Vector3 beginDragIconPoint;

        //===드래그 시작 시 커서의 위치===//
        private Vector3 beginDragCursorPoint; 

        private int beginDragSlotSiblingIndex;
        
        //===인벤토리 UI 내 아이템 카테고리 옵션===//
        private enum FilterOption
        {
            All, Armor, Weapon, Ingredients
        }

        private FilterOption currentFilterOption = FilterOption.All;

        private void Awake()
        {
            Init();
            InitSlots();
            InitButtonEvents();
            InitToggleEvents();
        }

        private void Update()
        {
            ped.position = Input.mousePosition;

            OnPointerEnterAndExit();
            if(showTooltip) ShowOrHideItemTooltip();
            OnPointerDown();
            OnPointerDrag();
            OnPointerUp();
        }

        //===초기 설정===//
        private void Init()
        {
            TryGetComponent(out gr);
            if (gr == null)
                gr = gameObject.AddComponent<GraphicRaycaster>();

            ped = new PointerEventData(EventSystem.current);
            raycastList = new List<RaycastResult>(10);

            if (itemTooltip == null)
            {
                itemTooltip = GetComponentInChildren<ItemTooltipUI>();
            }
        }

        //===슬롯 영역 내에 슬롯들 동적 생성===//
        private void InitSlots()
        {
            //===슬롯 프리팹 설정===//
            slotUiPrefab.TryGetComponent(out RectTransform slotRect);
            slotRect.sizeDelta = new Vector2(slotSize, slotSize);

            slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
            if (itemSlot == null)
                slotUiPrefab.AddComponent<ItemSlotUI>();

            slotUiPrefab.SetActive(false);

            //===초기 위치 설정===//
            Vector2 beginPos = new Vector2(contentAreaPadding, -contentAreaPadding);
            Vector2 curPos = beginPos;

            slotUIList = new List<ItemSlotUI>(verticalSlotCount * horizontalSlotCount);

            //===슬롯들 동적 생성===//
            for (int j = 0; j < verticalSlotCount; j++)
            {
                for (int i = 0; i < horizontalSlotCount; i++)
                {
                    int slotIndex = (horizontalSlotCount * j) + i;

                    var slotRT = CloneSlot();
                    slotRT.pivot = new Vector2(0f, 1f); // Left Top
                    slotRT.anchoredPosition = curPos;
                    slotRT.gameObject.SetActive(true);
                    slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                    var slotUI = slotRT.GetComponent<ItemSlotUI>();
                    slotUI.SetSlotIndex(slotIndex);
                    slotUIList.Add(slotUI);

                    //===다음 칸===//
                    curPos.x += (slotMargin + slotSize);
                }

                //===다음 줄===//
                curPos.x = beginPos.x;
                curPos.y -= (slotMargin + slotSize);
            }

            //===슬롯 프리팹이 아닌 경우 파괴===//
            if(slotUiPrefab.scene.rootCount != 0)
                Destroy(slotUiPrefab);

            RectTransform CloneSlot()
            {
                GameObject slotGo = Instantiate(slotUiPrefab);
                RectTransform rt = slotGo.GetComponent<RectTransform>();
                rt.SetParent(contentAreaRT);

                return rt;
            }
        }

        //===정렬 기능 버튼 이벤트 추가===//
        private void InitButtonEvents()
        {
            trimButton.onClick.AddListener(() => inventory.TrimAll());
            sortButton.onClick.AddListener(() => inventory.SortAll());
        }

        //===토글 버튼 이벤트 추가===//
        private void InitToggleEvents()
        {
            toggleFilterAll.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.All));
            toggleFilterArmors.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Armor));
            toggleFilterWeapons.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Weapon));
            toggleFilterIngredients.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Ingredients));

            void UpdateFilter(bool flag, FilterOption option)
            {
                if (flag)
                {
                    currentFilterOption = option;
                    UpdateAllSlotFilters();
                }
            }
        }

        private bool IsOverUI()
            => EventSystem.current.IsPointerOverGameObject();

        //===Raycast로 얻은 첫 번째 UI에서 컴포넌트 찾아 값 반환===//
        private T RaycastAndGetFirstComponent<T>() where T : Component
        {
            raycastList.Clear();

            gr.Raycast(ped, raycastList);
            
            if(raycastList.Count == 0)
                return null;

            return raycastList[0].gameObject.GetComponent<T>();
        }

        //===슬롯에 포인터가 올라가는/빠져나갈 때===//
        private void OnPointerEnterAndExit()
        {
            //===이전 프레임의 슬롯===//
            var prevSlot = pointerOverSlot;

            //===현재 프레임의 슬롯===//
            var curSlot = pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

            if (prevSlot == null)
            {
                if (curSlot != null)
                {
                    OnCurrentEnter();
                }
            }
            else
            {
                if (curSlot == null)
                {
                    OnPrevExit();
                }

                //===교환===//
                else if (prevSlot != curSlot)
                {
                    OnPrevExit();
                    OnCurrentEnter();
                }
            }

            void OnCurrentEnter()
            {
                if(showHighlight)
                    curSlot.Highlight(true);
            }
            void OnPrevExit()
            {
                prevSlot.Highlight(false);
            }
        }

        //===아이템 정보 툴팁 보여주거나 감추기===//
        private void ShowOrHideItemTooltip()
        {
            //===마우스가 유효한 아이템 위에 있다면 툴팁 출력===//
            bool isValid = pointerOverSlot != null && pointerOverSlot.HasItem && pointerOverSlot.IsAccessible && (pointerOverSlot != beginDragSlot); //===드래그(이동) 할 때는 제외===//

            if (isValid)
            {
                UpdateTooltipUI(pointerOverSlot);
                itemTooltip.Show();
            }
            else
                itemTooltip.Hide();
        }

        //===슬롯을 클릭하는 경우===//
        private void OnPointerDown()
        {
            if (Input.GetMouseButtonDown(leftClick))
            {
                beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

                //===아이템을 갖고 있는 슬롯인 경우===//
                if (beginDragSlot != null && beginDragSlot.HasItem && beginDragSlot.IsAccessible)
                {
                    //===위치 기억===//
                    beginDragIconTransform = beginDragSlot.IconRect.transform;
                    beginDragIconPoint = beginDragIconTransform.position;
                    beginDragCursorPoint = Input.mousePosition;

                    //===가장 위에 표시===//
                    beginDragSlotSiblingIndex = beginDragSlot.transform.GetSiblingIndex();
                    beginDragSlot.transform.SetAsLastSibling();

                    //===해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치===//
                    beginDragSlot.SetHighlightOnTop(false);
                }
                else
                {
                    beginDragSlot = null;
                }
            }
        }

        //===드래그하는 도중===//
        private void OnPointerDrag()
        {
            if(beginDragSlot == null) return;

            if (Input.GetMouseButton(leftClick))
            {
                //===위치 이동===//
                beginDragIconTransform.position =
                    beginDragIconPoint + (Input.mousePosition - beginDragCursorPoint);
            }
        }

        //===클릭을 뗄 경우===//
        private void OnPointerUp()
        {
            if (Input.GetMouseButtonUp(leftClick))
            {
                if (beginDragSlot != null)
                {
                    //===위치 복원===//
                    beginDragIconTransform.position = beginDragIconPoint;

                    //===UI 순서 복원===//
                    beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);

                    //===드래그 완료===//
                    EndDrag();

                    //===해당 슬롯의 하이라이트 이미지를 아이콘보다 잎에 위치===//
                    beginDragSlot.SetHighlightOnTop(true);

                    //===참조 제거===//
                    beginDragSlot = null;
                    beginDragIconTransform = null;
                }
            }
        }

        private void EndDrag()
        {
            ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

            //===아이템 교환 및이동===//
            if (endDragSlot != null && endDragSlot.IsAccessible)
            {
                bool isSeparation = false;

                if(!isSeparation)
                    TrySwapItems(beginDragSlot, endDragSlot);

                UpdateTooltipUI(endDragSlot);
                return;
            }

            //===버리는 기능===//
            if (!IsOverUI())
            {
                //===확인 팝업 출력===//
                int index = beginDragSlot.Index;
                string itemName = inventory.GetItemName(index);
                int amount = inventory.GetCurrentAmount(index);

                if (amount > 1)
                    itemName += $" x{amount}";

                if (showRemovingPopup)
                    popup.OpenConfirmationPopup(() => TryRemoveItem(index), itemName);
                else
                    TryRemoveItem(index);
            }
        }

        //===인벤토리에서 아이템 제거===//
        private void TryRemoveItem(int index)
        {
            inventory.Remove(index);
        }

        //===두 슬롯의 아이템 교환===//
        private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
        {
            if (from == to)
            {
                return;
            }

            from.SwapOrMoveIcon(to);
            inventory.Swap(from.Index, to.Index);
        }

        //===툴팁 UI 데이터 업데이트===//
        private void UpdateTooltipUI(ItemSlotUI slot)
        {
            if(!slot.IsAccessible || !slot.HasItem)
                return;

            //===툴팁 정보 업데이트===//
            itemTooltip.SetItemInfo(inventory.GetItemData(slot.Index));

            //===툴팁 위치 조정===//
            itemTooltip.SetRectPosition(slot.SlotRect);
        }

        //===인벤토리 참조 등록===//
        public void SetInventoryReference(Inventory _inventory)
        {
            //===인벤토리에서 직접 호출===//
            inventory = _inventory; 
        }

        //===마우스 클릭 좌우 반전===//
        public void InvertMouse(bool value)
        {
            leftClick = value ? 1 : 0;
            rightClick = value ? 0 : 1;

            mouseReversed = value;
        }

        //===슬롯에 아이템 아이콘 등록===//
        public void SetItemIcon(int index, Sprite icon)
        {
            slotUIList[index].SetItem(icon);
        }

        //===아이템 개수 텍스트 출력===//
        public void SetItemAmountText(int index, int amount)
        {
            //===amount가 1 이하일 경우 텍스트 미표시===//
            slotUIList[index].SetItemAmount(amount);
        }

        //===아이템 개수 텍스트 지정===//
        public void HideItemAmountText(int index)
        {
            slotUIList[index].SetItemAmount(1);
        }

        //===슬롯에서 아이템 제거===//
        public void RemoveItem(int index)
        {
            slotUIList[index].RemoveItem();
        }

        //===접근 가능한 슬롯 범위 설정===//
        public void SetAccessibleSlotRange(int accessibleSlotCount)
        {
            for (int i = 0; i < slotUIList.Count; i++)
            {
                slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
            }
        }

        //===카테고리 별 상태 업데이트===//
        public void UpdateSlotFilterState(int index, ItemData itemData)
        {
            bool isFiltered = true;

            //===null인 슬롯은 타입 검사 없이 활성화===//
            if(itemData != null)
                switch (currentFilterOption)
                {
                    case FilterOption.Armor:
                        isFiltered = (itemData is ArmorItemData);
                        break;
                    case FilterOption.Weapon:
                        isFiltered = (itemData is WeaponItemData);
                        break;
                    case FilterOption.Ingredients:
                        isFiltered = (itemData is IngredientsItemData);
                        break;
                }

            slotUIList[index].SetItemAccessibleState(isFiltered);
        }

        //===모든 슬롯 필터 상태 업데이트===//
        public void UpdateAllSlotFilters()
        {
            int capacity = inventory.Capacity;

            for (int i = 0; i < capacity; i++)
            {
                ItemData data = inventory.GetItemData(i);
                UpdateSlotFilterState(i, data);
            }
        }


        //===슬롯 생성 미리보기===//
        [SerializeField] private bool showPreview = false;

        [Range(0.01f, 1f)]
        [SerializeField] private float previewAlpha = 0.1f;

        private List<GameObject> previewSlotGoList = new List<GameObject>();
        private int prevSlotCountPerLine;
        private int prevSlotLineCount;
        private float prevSlotSize;
        private float prevSlotMargin;
        private float prevContentPadding;
        private float prevAlpha;
        private bool prevShow = false;
        private bool prevMouseReversed = false;

        private void OnValidate()
        {
            if (prevMouseReversed != mouseReversed)
            {
                prevMouseReversed = mouseReversed;
                InvertMouse(mouseReversed);   
            }

            if (Application.isPlaying) return;

            if (showPreview && !prevShow)
            {
                CreateSlots();
            }
            prevShow = showPreview;

            if (Unavailable())
            {
                ClearAll();
                return;
            }
            if (CountChanged())
            {
                ClearAll();
                CreateSlots();
                prevSlotCountPerLine = horizontalSlotCount;
                prevSlotLineCount = verticalSlotCount;
            }
            if (ValueChanged())
            {
                DrawGrid();
                prevSlotSize = slotSize;
                prevSlotMargin = slotMargin;
                prevContentPadding = contentAreaPadding;
            }
            if (AlphaChanged())
            {
                SetImageAlpha();
                prevAlpha = previewAlpha;
            }

            bool Unavailable()
            {
                return !showPreview ||
                        horizontalSlotCount < 1 ||
                        verticalSlotCount < 1 ||
                        slotSize <= 0f ||
                        contentAreaRT == null ||
                        slotUiPrefab == null;
            }
            bool CountChanged()
            {
                return horizontalSlotCount != prevSlotCountPerLine || 
                       verticalSlotCount != prevSlotLineCount;
            }
            bool ValueChanged()
            {
                return slotSize != prevSlotSize ||
                       slotMargin != prevSlotMargin ||
                       contentAreaPadding != prevContentPadding;
            }
            bool AlphaChanged()
            {
                return previewAlpha != prevAlpha;
            }
            void ClearAll()
            {
                foreach (var go in previewSlotGoList)
                {
                    Destroyer.Destroy(go);
                }
                previewSlotGoList.Clear();
            }
            void CreateSlots()
            {
                int count = horizontalSlotCount * verticalSlotCount;
                previewSlotGoList.Capacity = count;

                RectTransform slotPrefabRT = slotUiPrefab.GetComponent<RectTransform>();
                slotPrefabRT.pivot = new Vector2(0f, 1f);

                for (int i = 0; i < count; i++)
                {
                    GameObject slotGo = Instantiate(slotUiPrefab);
                    slotGo.transform.SetParent(contentAreaRT.transform);
                    slotGo.SetActive(true);
                    slotGo.AddComponent<PreviewItemSlot>();

                    slotGo.transform.localScale = Vector3.one;

                    HideGameObject(slotGo);

                    previewSlotGoList.Add(slotGo);
                }

                DrawGrid();
                SetImageAlpha();
            }
            void DrawGrid()
            {
                Vector2 beginPos = new Vector2(contentAreaPadding, -contentAreaPadding);
                Vector2 curPos = beginPos;

                int index = 0;
                for (int j = 0; j < verticalSlotCount; j++)
                {
                    for (int i = 0; i < horizontalSlotCount; i++)
                    {
                        GameObject slotGo = previewSlotGoList[index++];
                        RectTransform slotRT = slotGo.GetComponent<RectTransform>();

                        slotRT.anchoredPosition = curPos;
                        slotRT.sizeDelta = new Vector2(slotSize, slotSize);
                        previewSlotGoList.Add(slotGo);

                        curPos.x += (slotMargin + slotSize);
                    }

                    curPos.x = beginPos.x;
                    curPos.y -= (slotMargin + slotSize);
                }
            }
            void HideGameObject(GameObject go)
            {
                go.hideFlags = HideFlags.HideAndDontSave;

                Transform tr = go.transform;
                for (int i = 0; i < tr.childCount; i++)
                {
                    tr.GetChild(i).gameObject.hideFlags = HideFlags.HideAndDontSave;
                }
            }
            void SetImageAlpha()
            {
                foreach (var go in previewSlotGoList)
                {
                    var images = go.GetComponentsInChildren<Image>();
                    foreach (var img in images)
                    {
                        img.color = new Color(img.color.r, img.color.g, img.color.b, previewAlpha);
                        var outline = img.GetComponent<Outline>();
                        if (outline)
                            outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, previewAlpha);
                    }
                }
            }
        }

        //======================================================================================//

        private class PreviewItemSlot : MonoBehaviour { }

        private static class Destroyer
        {
            private static Queue<GameObject> targetQueue = new Queue<GameObject>();

            static Destroyer()
            { 
                {
                    for (int i = 0; targetQueue.Count > 0 && i < 100000; i++)
                    {
                        var next = targetQueue.Dequeue();
                        DestroyImmediate(next);
                    }
                };
            }
            public static void Destroy(GameObject go) => targetQueue.Enqueue(go);
        }
    }
}