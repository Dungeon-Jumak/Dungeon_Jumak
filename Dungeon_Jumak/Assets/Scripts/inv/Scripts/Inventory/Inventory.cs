using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public int Capacity { get; private set; }

        [Header("초기 슬롯 수용 제한")]
        [SerializeField, Range(8, 64)]
        private int _initalCapacity = 32;

        [Header("최대 수용 한도(아이템 배열 크기)")]
        [SerializeField, Range(8, 64)]
        private int _maxCapacity = 64;

        [Header("인벤토리 UI")]
        [SerializeField]
        private InventoryUI _inventoryUI; 

        //===아이템 목록===//
        [SerializeField]
        private Item[] _items;

        //===인덱스 목록 업데이트===//
        private readonly HashSet<int> _indexSetForUpdate = new HashSet<int>();

        //===아이템 데이터 타입별 정렬 가중치===//
        private readonly static Dictionary<Type, int> _sortWeightDict = new Dictionary<Type, int>
        {
            { typeof(WeaponItemData), 20000 },
            { typeof(ArmorItemData), 30000 },
        };

        private class ItemComparer : IComparer<Item>
        {
            public int Compare(Item a, Item b)
            {
                return (a.Data.ID + _sortWeightDict[a.Data.GetType()])
                     - (b.Data.ID + _sortWeightDict[b.Data.GetType()]);
            }
        }
        private static readonly ItemComparer _itemComparer = new ItemComparer();

        private void OnValidate()
        {
            if(_initalCapacity > _maxCapacity)
                _initalCapacity = _maxCapacity;
        }

        private void Awake()
        {
            _items = new Item[_maxCapacity];
            Capacity = _initalCapacity;
            _inventoryUI.SetInventoryReference(this);
        }

        private void Start()
        {
            UpdateAccessibleStatesAll();
        }

        //===인덱스가 수용 범위 내에 있는지 검사===//
        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < Capacity;
        }

        //===앞에서부터 빈 슬롯 인덱스 탐색===//
        private int FindEmptySlotIndex(int startIndex = 0)
        {
            for (int i = startIndex; i < Capacity; i++)
                if (_items[i] == null)
                    return i;
            return -1;
        }

        //===해당하는 인덱스의 슬롯 상태 및 UI 업데이트===//
        private void UpdateSlot(int index)
        {
            if (!IsValidIndex(index)) return;

            Item item = _items[index];

            //===아이템이 슬롯에 존재하는 경우===//
            if (item != null)
            {
                //===아이콘 등록===//
                _inventoryUI.SetItemIcon(index, item.Data.IconSprite);

                //===장비 아이템인 경우 수량 텍스트 제거===//
                if (item is EquipmentItem)
                {
                    _inventoryUI.HideItemAmountText(index);
                }

                //===슬롯 필터링 상태 업데이트===//
                _inventoryUI.UpdateSlotFilterState(index, item.Data);
            }
            //===빈 슬롯인 경우===//
            else
            {
                //===아이콘 제거===//
                RemoveIcon();
            }

            void RemoveIcon()
            {
                _inventoryUI.RemoveItem(index);
                _inventoryUI.HideItemAmountText(index);
            }
        }

        //===해당하는 인덱스의 슬롯들의 상태 및 UI 업데이트===//
        private void UpdateSlot(params int[] indices)
        {
            foreach (var i in indices)
            {
                UpdateSlot(i);
            }
        }

        //===모든 슬롯들의 상태를 UI에 업데이트===//
        private void UpdateAllSlot()
        {
            for (int i = 0; i < Capacity; i++)
            {
                UpdateSlot(i);
            }
        }

        //===해당 슬롯에 아이템이 있는지 여부===//
        public bool HasItem(int index)
        {
            return IsValidIndex(index) && _items[index] != null;
        }

        //===아이템 개수 반환===//
        public int GetCurrentAmount(int index)
        {
            if (!IsValidIndex(index)) return -1; //===잘못된 인덱스===//
            if (_items[index] == null) return 0; //===빈 슬롯이면===//

            return 1; //===슬롯에 아이템이 있으면===//
        }

        //===아이템 정보 반환===//
        public ItemData GetItemData(int index)
        {
            if (!IsValidIndex(index)) return null;
            if (_items[index] == null) return null;

            return _items[index].Data;
        }

        //===아이템 이름 값 반환===//
        public string GetItemName(int index)
        {
            if (!IsValidIndex(index)) return "";
            if (_items[index] == null) return "";

            return _items[index].Data.Name;
        }

        //===인벤토리 UI 연결===//
        public void ConnectUI(InventoryUI inventoryUI)
        {
            _inventoryUI = inventoryUI;
            _inventoryUI.SetInventoryReference(this);
        }

        //===!!!!!!!아이템 추가!!!!!!!===//
        public int Add(ItemData itemData, int amount = 1)
        {
            int index;

            //===장비 아이템===//
            if (itemData is EquipmentItemData)
            {
                //===1개만 넣는 경우===//
                if (amount == 1)
                {
                    index = FindEmptySlotIndex();
                    if (index != -1)
                    {
                        //===아이템을 생성하여 슬롯에 추가===//
                        _items[index] = itemData.CreateItem();
                        amount = 0;

                        UpdateSlot(index);
                    }
                }

                //===2개 이상의 장비 아이템을 동시에 추가하는 경우===//
                index = -1;
                for (; amount > 0; amount--)
                {
                    //===아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색===//
                    index = FindEmptySlotIndex(index + 1);

                    //===가득차 있을 경우 루프 종료===//
                    if (index == -1)
                    {
                        break;
                    }

                    //===아이템을 생성하여 슬롯에 추가===//
                    _items[index] = itemData.CreateItem();

                    UpdateSlot(index);
                }
            }
            //===반환 값이 0이면 정상===//
            return amount; 
        }

        //===아이템 제거===//
        public void Remove(int index)
        {
            if (!IsValidIndex(index)) return;

            _items[index] = null;
            _inventoryUI.RemoveItem(index);
        }

        //=== 두 인덱스의 아이템 위치를 교체===//
        public void Swap(int indexA, int indexB)
        {
            if (!IsValidIndex(indexA)) return;
            if (!IsValidIndex(indexB)) return;

            Item itemA = _items[indexA];
            Item itemB = _items[indexB];

            if (itemA != null && itemB != null)
            {
                _items[indexA] = itemB;
                _items[indexB] = itemA;
            }
            else
            {
                _items[indexA] = itemB;
                _items[indexB] = itemA;
            }

            UpdateSlot(indexA, indexB);
        }


        //===슬롯 UI에 접근 가능 여부 업데이트===//
        public void UpdateAccessibleStatesAll()
        {
            _inventoryUI.SetAccessibleSlotRange(Capacity);
        }

        //===빈 슬롯 없이 앞에서부터 채우기===//
        public void TrimAll()
        {
            _indexSetForUpdate.Clear();

            int i = -1;
            while (_items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < Capacity && _items[j] == null);

                if (j == Capacity)
                    break;

                _indexSetForUpdate.Add(i);
                _indexSetForUpdate.Add(j);

                _items[i] = _items[j];
                _items[j] = null;
                i++;
            }

            foreach (var index in _indexSetForUpdate)
            {
                UpdateSlot(index);
            }
            _inventoryUI.UpdateAllSlotFilters();
        }

        //===빈 슬롯 없이 채우면서 아이템 종류별로 정렬하기===//
        public void SortAll()
        {
            //===Trim===//
            int i = -1;
            while (_items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < Capacity && _items[j] == null) ;

                if (j == Capacity)
                    break;

                _items[i] = _items[j];
                _items[j] = null;
                i++;
            }

            //===Sort===//
            Array.Sort(_items, 0, i, _itemComparer);

            UpdateAllSlot();
            _inventoryUI.UpdateAllSlotFilters();
        }
    }
}