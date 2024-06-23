using System;
using System.Collections;
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
        [SerializeField] private int _horizontalSlotCount = 8;

        [Header("슬롯 세로 개수")]
        [Range(0, 10)]
        [SerializeField] private int _verticalSlotCount = 8;

        [Header("한 슬롯의 상하좌우 여백")]
        [SerializeField] private float _slotMargin = 8f;

        [Header("인벤토리 영역의 내부 여백")]
        [SerializeField] private float _contentAreaPadding = 20f;

        [Header("각 슬롯의 크기")]
        [Range(32, 64)]
        [SerializeField] private float _slotSize = 64f;

        [Space]
        [SerializeField] private bool _showTooltip = true;
        [SerializeField] private bool _showHighlight = true;
        [SerializeField] private bool _showRemovingPopup = true;

        [Header("슬롯들이 위치할 영역")]
        [SerializeField] private RectTransform _contentAreaRT;

        [Header("슬롯 프리팹")]
        [SerializeField] private GameObject _slotUiPrefab;

        [Header("아이템 정보 툴팁 UI")]
        [SerializeField] private ItemTooltipUI _itemTooltip;

        [Header("팝업 UI 오브젝트")]
        [SerializeField] private InventoryPopupUI _popup;

        [Header("버튼")]
        [SerializeField] private Button _trimButton;
        [SerializeField] private Button _sortButton;

        [Header("카테고리")]
        [SerializeField] private Toggle _toggleFilterAll;
        [SerializeField] private Toggle _toggleFilterEquipments;
        [SerializeField] private Toggle _toggleFilterPortions;

        [Space]
        [SerializeField] private bool _mouseReversed = false;

        //===연결된 인벤토리===//
        private Inventory _inventory;

        private List<ItemSlotUI> _slotUIList = new List<ItemSlotUI>();
        private GraphicRaycaster _gr;
        private PointerEventData _ped;
        private List<RaycastResult> _rrList;

        //===현재 포인터가 위치한 곳의 슬롯===//
        private ItemSlotUI _pointerOverSlot;

        //===현재 드래그를 시작한 슬롯===//
        private ItemSlotUI _beginDragSlot;

        //===해당 슬롯의 아이콘 Transform===//
        private Transform _beginDragIconTransform; 

        private int _leftClick = 0;
        private int _rightClick = 1;

        //===드래그 시작 시 슬롯의 위치===//
        private Vector3 _beginDragIconPoint;

        //===드래그 시작 시 커서의 위치===//
        private Vector3 _beginDragCursorPoint; 

        private int _beginDragSlotSiblingIndex;
        
        //===인벤토리 UI 내 아이템 카테고리 옵션===//
        private enum FilterOption
        {
            All, Armor, Weapon
        }

        private FilterOption _currentFilterOption = FilterOption.All;

        private void Awake()
        {
            Init();
            InitSlots();
            InitButtonEvents();
            InitToggleEvents();
        }

        private void Update()
        {
            _ped.position = Input.mousePosition;

            OnPointerEnterAndExit();
            if(_showTooltip) ShowOrHideItemTooltip();
            OnPointerDown();
            OnPointerDrag();
            OnPointerUp();
        }

        //===초기 설정===//
        private void Init()
        {
            TryGetComponent(out _gr);
            if (_gr == null)
                _gr = gameObject.AddComponent<GraphicRaycaster>();

            _ped = new PointerEventData(EventSystem.current);
            _rrList = new List<RaycastResult>(10);

            if (_itemTooltip == null)
            {
                _itemTooltip = GetComponentInChildren<ItemTooltipUI>();
            }
        }

        //===슬롯 영역 내에 슬롯들 동적 생성===//
        private void InitSlots()
        {
            //===슬롯 프리팹 설정===//
            _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
            slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);

            _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
            if (itemSlot == null)
                _slotUiPrefab.AddComponent<ItemSlotUI>();

            _slotUiPrefab.SetActive(false);

            //===초기 위치 설정===//
            Vector2 beginPos = new Vector2(_contentAreaPadding, -_contentAreaPadding);
            Vector2 curPos = beginPos;

            _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);

            //===슬롯들 동적 생성===//
            for (int j = 0; j < _verticalSlotCount; j++)
            {
                for (int i = 0; i < _horizontalSlotCount; i++)
                {
                    int slotIndex = (_horizontalSlotCount * j) + i;

                    var slotRT = CloneSlot();
                    slotRT.pivot = new Vector2(0f, 1f); // Left Top
                    slotRT.anchoredPosition = curPos;
                    slotRT.gameObject.SetActive(true);
                    slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                    var slotUI = slotRT.GetComponent<ItemSlotUI>();
                    slotUI.SetSlotIndex(slotIndex);
                    _slotUIList.Add(slotUI);

                    //===다음 칸===//
                    curPos.x += (_slotMargin + _slotSize);
                }

                //===다음 줄===//
                curPos.x = beginPos.x;
                curPos.y -= (_slotMargin + _slotSize);
            }

            //===슬롯 프리팹이 아닌 경우 파괴===//
            if(_slotUiPrefab.scene.rootCount != 0)
                Destroy(_slotUiPrefab);

            RectTransform CloneSlot()
            {
                GameObject slotGo = Instantiate(_slotUiPrefab);
                RectTransform rt = slotGo.GetComponent<RectTransform>();
                rt.SetParent(_contentAreaRT);

                return rt;
            }
        }

        //===정렬 기능 버튼 이벤트 추가===//
        private void InitButtonEvents()
        {
            _trimButton.onClick.AddListener(() => _inventory.TrimAll());
            _sortButton.onClick.AddListener(() => _inventory.SortAll());
        }

        //===토글 버튼 이벤트 추가===//
        private void InitToggleEvents()
        {
            _toggleFilterAll.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.All));
            _toggleFilterEquipments.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Armor));
            _toggleFilterPortions.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Weapon));

            void UpdateFilter(bool flag, FilterOption option)
            {
                if (flag)
                {
                    _currentFilterOption = option;
                    UpdateAllSlotFilters();
                }
            }
        }

        private bool IsOverUI()
            => EventSystem.current.IsPointerOverGameObject();

        //===Raycast로 얻은 첫 번째 UI에서 컴포넌트 찾아 값 반환===//
        private T RaycastAndGetFirstComponent<T>() where T : Component
        {
            _rrList.Clear();

            _gr.Raycast(_ped, _rrList);
            
            if(_rrList.Count == 0)
                return null;

            return _rrList[0].gameObject.GetComponent<T>();
        }

        //===슬롯에 포인터가 올라가는/빠져나갈 때===//
        private void OnPointerEnterAndExit()
        {
            //===이전 프레임의 슬롯===//
            var prevSlot = _pointerOverSlot;

            //===현재 프레임의 슬롯===//
            var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

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
                if(_showHighlight)
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
            bool isValid =
                _pointerOverSlot != null && _pointerOverSlot.HasItem && _pointerOverSlot.IsAccessible
                && (_pointerOverSlot != _beginDragSlot); //===드래그(이동) 할 때는 제외===//

            if (isValid)
            {
                UpdateTooltipUI(_pointerOverSlot);
                _itemTooltip.Show();
            }
            else
                _itemTooltip.Hide();
        }

        //===슬롯을 클릭하는 경우===//
        private void OnPointerDown()
        {
            if (Input.GetMouseButtonDown(_leftClick))
            {
                _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

                //===아이템을 갖고 있는 슬롯인 경우===//
                if (_beginDragSlot != null && _beginDragSlot.HasItem && _beginDragSlot.IsAccessible)
                {
                    //===위치 기억===//
                    _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                    _beginDragIconPoint = _beginDragIconTransform.position;
                    _beginDragCursorPoint = Input.mousePosition;

                    //===가장 위에 표시===//
                    _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                    _beginDragSlot.transform.SetAsLastSibling();

                    //===해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치===//
                    _beginDragSlot.SetHighlightOnTop(false);
                }
                else
                {
                    _beginDragSlot = null;
                }
            }
        }

        //===드래그하는 도중===//
        private void OnPointerDrag()
        {
            if(_beginDragSlot == null) return;

            if (Input.GetMouseButton(_leftClick))
            {
                //===위치 이동===//
                _beginDragIconTransform.position =
                    _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
            }
        }

        //===클릭을 뗄 경우===//
        private void OnPointerUp()
        {
            if (Input.GetMouseButtonUp(_leftClick))
            {
                if (_beginDragSlot != null)
                {
                    //===위치 복원===//
                    _beginDragIconTransform.position = _beginDragIconPoint;

                    //===UI 순서 복원===//
                    _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                    //===드래그 완료===//
                    EndDrag();

                    //===해당 슬롯의 하이라이트 이미지를 아이콘보다 잎에 위치===//
                    _beginDragSlot.SetHighlightOnTop(true);

                    //===참조 제거===//
                    _beginDragSlot = null;
                    _beginDragIconTransform = null;
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
                    TrySwapItems(_beginDragSlot, endDragSlot);

                UpdateTooltipUI(endDragSlot);
                return;
            }

            //===버리는 기능===//
            if (!IsOverUI())
            {
                //===확인 팝업 출력===//
                int index = _beginDragSlot.Index;
                string itemName = _inventory.GetItemName(index);
                int amount = _inventory.GetCurrentAmount(index);

                if(_showRemovingPopup)
                    _popup.OpenConfirmationPopup(() => TryRemoveItem(index), itemName);
                else
                    TryRemoveItem(index);
            }
        }

        //===인벤토리에서 아이템 제거===//
        private void TryRemoveItem(int index)
        {
            _inventory.Remove(index);
        }

        //===두 슬롯의 아이템 교환===//
        private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
        {
            if (from == to)
            {
                return;
            }

            from.SwapOrMoveIcon(to);
            _inventory.Swap(from.Index, to.Index);
        }

        //===툴팁 UI 데이터 업데이트===//
        private void UpdateTooltipUI(ItemSlotUI slot)
        {
            if(!slot.IsAccessible || !slot.HasItem)
                return;

            //===툴팁 정보 업데이트===//
            _itemTooltip.SetItemInfo(_inventory.GetItemData(slot.Index));

            //===툴팁 위치 조정===//
            _itemTooltip.SetRectPosition(slot.SlotRect);
        }


        //===인벤토리 참조 등록===//
        public void SetInventoryReference(Inventory inventory)
        {
            //===인벤토리에서 직접 호출===//
            _inventory = inventory; 
        }

        //===마우스 클릭 좌우 반전===//
        public void InvertMouse(bool value)
        {
            _leftClick = value ? 1 : 0;
            _rightClick = value ? 0 : 1;

            _mouseReversed = value;
        }

        //===슬롯에 아이템 아이콘 등록===//
        public void SetItemIcon(int index, Sprite icon)
        {
            _slotUIList[index].SetItem(icon);
        }

        //===아이템 개수 텍스트 출력===//
        public void SetItemAmountText(int index, int amount)
        {
            //===amount가 1 이하일 경우 텍스트 미표시===//
            _slotUIList[index].SetItemAmount(amount);
        }

        //===아이템 개수 텍스트 지정===//
        public void HideItemAmountText(int index)
        {
            _slotUIList[index].SetItemAmount(1);
        }

        //===슬롯에서 아이템 제거===//
        public void RemoveItem(int index)
        {
            _slotUIList[index].RemoveItem();
        }

        //===접근 가능한 슬롯 범위 설정===//
        public void SetAccessibleSlotRange(int accessibleSlotCount)
        {
            for (int i = 0; i < _slotUIList.Count; i++)
            {
                _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
            }
        }

        //===카테고리 별 상태 업데이트===//
        public void UpdateSlotFilterState(int index, ItemData itemData)
        {
            bool isFiltered = true;

            //===null인 슬롯은 타입 검사 없이 활성화===//
            if(itemData != null)
                switch (_currentFilterOption)
                {
                    case FilterOption.Armor:
                        isFiltered = (itemData is ArmorItemData);
                        break;
                    case FilterOption.Weapon:
                        isFiltered = (itemData is WeaponItemData);
                        break;
                }

            _slotUIList[index].SetItemAccessibleState(isFiltered);
        }

        //===모든 슬롯 필터 상태 업데이트===//
        public void UpdateAllSlotFilters()
        {
            int capacity = _inventory.Capacity;

            for (int i = 0; i < capacity; i++)
            {
                ItemData data = _inventory.GetItemData(i);
                UpdateSlotFilterState(i, data);
            }
        }

        //===슬롯 생성 미리보기===//
        [SerializeField] private bool __showPreview = false;

        [Range(0.01f, 1f)]
        [SerializeField] private float __previewAlpha = 0.1f;

        private List<GameObject> __previewSlotGoList = new List<GameObject>();
        private int __prevSlotCountPerLine;
        private int __prevSlotLineCount;
        private float __prevSlotSize;
        private float __prevSlotMargin;
        private float __prevContentPadding;
        private float __prevAlpha;
        private bool __prevShow = false;
        private bool __prevMouseReversed = false;

        private void OnValidate()
        {
            if (__prevMouseReversed != _mouseReversed)
            {
                __prevMouseReversed = _mouseReversed;
                InvertMouse(_mouseReversed);   
            }

            if (Application.isPlaying) return;

            if (__showPreview && !__prevShow)
            {
                CreateSlots();
            }
            __prevShow = __showPreview;

            if (Unavailable())
            {
                ClearAll();
                return;
            }
            if (CountChanged())
            {
                ClearAll();
                CreateSlots();
                __prevSlotCountPerLine = _horizontalSlotCount;
                __prevSlotLineCount = _verticalSlotCount;
            }
            if (ValueChanged())
            {
                DrawGrid();
                __prevSlotSize = _slotSize;
                __prevSlotMargin = _slotMargin;
                __prevContentPadding = _contentAreaPadding;
            }
            if (AlphaChanged())
            {
                SetImageAlpha();
                __prevAlpha = __previewAlpha;
            }

            bool Unavailable()
            {
                return !__showPreview ||
                        _horizontalSlotCount < 1 ||
                        _verticalSlotCount < 1 ||
                        _slotSize <= 0f ||
                        _contentAreaRT == null ||
                        _slotUiPrefab == null;
            }
            bool CountChanged()
            {
                return _horizontalSlotCount != __prevSlotCountPerLine ||
                       _verticalSlotCount != __prevSlotLineCount;
            }
            bool ValueChanged()
            {
                return _slotSize != __prevSlotSize ||
                       _slotMargin != __prevSlotMargin ||
                       _contentAreaPadding != __prevContentPadding;
            }
            bool AlphaChanged()
            {
                return __previewAlpha != __prevAlpha;
            }
            void ClearAll()
            {
                foreach (var go in __previewSlotGoList)
                {
                    Destroyer.Destroy(go);
                }
                __previewSlotGoList.Clear();
            }
            void CreateSlots()
            {
                int count = _horizontalSlotCount * _verticalSlotCount;
                __previewSlotGoList.Capacity = count;

                RectTransform slotPrefabRT = _slotUiPrefab.GetComponent<RectTransform>();
                slotPrefabRT.pivot = new Vector2(0f, 1f);

                for (int i = 0; i < count; i++)
                {
                    GameObject slotGo = Instantiate(_slotUiPrefab);
                    slotGo.transform.SetParent(_contentAreaRT.transform);
                    slotGo.SetActive(true);
                    slotGo.AddComponent<PreviewItemSlot>();

                    slotGo.transform.localScale = Vector3.one;

                    HideGameObject(slotGo);

                    __previewSlotGoList.Add(slotGo);
                }

                DrawGrid();
                SetImageAlpha();
            }
            void DrawGrid()
            {
                Vector2 beginPos = new Vector2(_contentAreaPadding, -_contentAreaPadding);
                Vector2 curPos = beginPos;

                int index = 0;
                for (int j = 0; j < _verticalSlotCount; j++)
                {
                    for (int i = 0; i < _horizontalSlotCount; i++)
                    {
                        GameObject slotGo = __previewSlotGoList[index++];
                        RectTransform slotRT = slotGo.GetComponent<RectTransform>();

                        slotRT.anchoredPosition = curPos;
                        slotRT.sizeDelta = new Vector2(_slotSize, _slotSize);
                        __previewSlotGoList.Add(slotGo);

                        curPos.x += (_slotMargin + _slotSize);
                    }

                    curPos.x = beginPos.x;
                    curPos.y -= (_slotMargin + _slotSize);
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
                foreach (var go in __previewSlotGoList)
                {
                    var images = go.GetComponentsInChildren<Image>();
                    foreach (var img in images)
                    {
                        img.color = new Color(img.color.r, img.color.g, img.color.b, __previewAlpha);
                        var outline = img.GetComponent<Outline>();
                        if (outline)
                            outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, __previewAlpha);
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
                //UnityEditor.EditorApplication.update += () =>
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