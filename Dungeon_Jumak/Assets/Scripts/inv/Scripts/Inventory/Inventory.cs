using System;
using System.Collections.Generic;
using UnityEngine;

namespace DJ.InventorySystem
{   
    public class Inventory : MonoBehaviour
    {
        public int Capacity { get; private set; }

        [Header("초기 슬롯 수용 제한")]
        [SerializeField, Range(8, 64)]
        private int initalCapacity = 32;

        [Header("최대 수용 한도(아이템 배열 크기)")]
        [SerializeField, Range(8, 64)]
        private int maxCapacity = 64;

        [Header("인벤토리 UI")]
        [SerializeField]
        private InventoryUI inventoryUI; 

        //===아이템 목록===//
        [SerializeField]
        private Item[] items;

        //===인덱스 목록 업데이트===//
        private readonly HashSet<int> indexSetForUpdate = new HashSet<int>();

        //===아이템 데이터 타입별 정렬 가중치===//
        private readonly static Dictionary<Type, int> sortWeightDict = new Dictionary<Type, int>
        {
            { typeof(IngredientsItemData), 10000 },
            { typeof(WeaponItemData), 20000 },
            { typeof(ArmorItemData), 30000 },
        };

        private class ItemCompare : IComparer<Item>
        {
            public int Compare(Item a, Item b)
            {
                return (a.Data.ID + sortWeightDict[a.Data.GetType()])
                     - (b.Data.ID + sortWeightDict[b.Data.GetType()]);
            }
        }
        private static readonly ItemCompare itemCompare = new ItemCompare();

        private void OnValidate()
        {
            if(initalCapacity > maxCapacity)
                initalCapacity = maxCapacity;
        }

        private void Awake()
        {
            items = new Item[maxCapacity];
            Capacity = initalCapacity;
            inventoryUI.SetInventoryReference(this);
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
                if (items[i] == null)
                    return i;
            return -1;
        }

        private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
        {
            for (int i = startIndex; i < Capacity; i++)
            {
                var current = items[i];
                if (current == null)
                    continue;

                if (current.Data == target && current is CountableItem ci)
                {
                    if (!ci.IsMax)
                        return i;
                }
            }

            return -1;
        }

        //===해당하는 인덱스의 슬롯 상태 및 UI 업데이트===//
        private void UpdateSlot(int index)
        {
            if (!IsValidIndex(index)) return;

            Item item = items[index];

            //===아이템이 슬롯에 존재하는 경우===//
            if (item != null)
            {
                //===아이콘 등록===//
                inventoryUI.SetItemIcon(index, item.Data.IconSprite);

                if (item is CountableItem ci)
                {
                    if (ci.IsEmpty)
                    {
                        items[index] = null;
                        RemoveIcon();
                        return;
                    }
                    else
                    {
                        inventoryUI.SetItemAmountText(index, ci.Amount);
                    }
                }
            }
            //===빈 슬롯인 경우===//
            else
            {
                //===아이콘 제거===//
                RemoveIcon();
            }

            void RemoveIcon()
            {
                inventoryUI.RemoveItem(index);
                inventoryUI.HideItemAmountText(index);
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
            return IsValidIndex(index) && items[index] != null;
        }
        public bool IsCountableItem(int index)
        {
            return HasItem(index) && items[index] is CountableItem;
        }

        //===아이템 개수 반환===//
        public int GetCurrentAmount(int index)
        {
            if (!IsValidIndex(index)) return -1; //===잘못된 인덱스===//
            if (items[index] == null) return 0; //===빈 슬롯이면===//

            CountableItem ci = items[index] as CountableItem;
            if (ci == null)
                return 1;

            return ci.Amount; //===슬롯에 아이템이 있으면===//
        }

        //===아이템 정보 반환===//
        public ItemData GetItemData(int index)
        {
            if (!IsValidIndex(index)) return null;
            if (items[index] == null) return null;

            return items[index].Data;
        }

        //===아이템 이름 값 반환===//
        public string GetItemName(int index)
        {
            if (!IsValidIndex(index)) return "";
            if (items[index] == null) return "";

            return items[index].Data.Name;
        }

        //===인벤토리 UI 연결===//
        public void ConnectUI(InventoryUI _inventoryUI)
        {
            inventoryUI = _inventoryUI;
            inventoryUI.SetInventoryReference(this);
        }

        //===!!!!!!!아이템 추가!!!!!!!===//
        public int Add(ItemData itemData, int amount = 1)
        {
            int index;
            
            //===재료 아이템===//
            if (itemData is CountableItemData ciData)
            {
                bool findNextCountable = true;
                index = -1;

                while (amount > 0)
                {
                    //===이미 해당 아이템이 인벤토리 내에 존재하는지, Max 값을 초과하지 않는지 검사===//
                    if (findNextCountable)
                    {
                        index = FindCountableItemSlotIndex(ciData, index + 1);

                        //===아이템을 담을 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작===//
                        if (index == -1)
                        {
                            findNextCountable = false;
                        }
                        //===기존에 아이템이 담긴 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화===//
                        else
                        {
                            CountableItem ci = items[index] as CountableItem;
                            amount = ci.AddAmountAndGetExcess(amount);

                            UpdateSlot(index);
                        }
                    }
                    //===빈 슬롯 탐색===//
                    else
                    {
                        index = FindEmptySlotIndex(index + 1);

                        //===빈 슬롯조차 없는 경우 종료===//
                        if (index == -1)
                        {
                            break;
                        }
                        //===빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잔여 계산===//
                        else
                        {
                            //===새로운 아이템 생성===//
                            CountableItem ci = ciData.CreateItem() as CountableItem;
                            ci.SetAmount(amount);

                            //===슬롯에 추가===//
                            items[index] = ci;

                            //===남은 개수 계산===//
                            amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                            UpdateSlot(index);
                        }
                    }
                }
            }
            //===장비 아이템===//
            else
            {
                //===1개만 넣는 경우===//
                if (amount == 1)
                {
                    index = FindEmptySlotIndex();
                    if (index != -1)
                    {
                        //===아이템 생성 => 슬롯 추가===//
                        items[index] = itemData.CreateItem();
                        amount = 0;

                        UpdateSlot(index);
                    }
                }

                //===2개 이상 넣는 경우===//
                index = -1;
                for (; amount > 0; amount--)
                {
                    //===아이템 넣은 인덱스의 다음 인덱스부터 탐색===//
                    index = FindEmptySlotIndex(index + 1);

                    //===인덱스 공간이 없을 경우===//
                    if (index == -1)
                    {
                        break;
                    }

                    //===아이템을 생성하여 슬롯에 추가===//
                    items[index] = itemData.CreateItem();

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

            items[index] = null;
            inventoryUI.RemoveItem(index);
        }

        //=== 두 인덱스의 아이템 위치를 교체===//
        public void Swap(int indexA, int indexB)
        {
            if (!IsValidIndex(indexA)) return;
            if (!IsValidIndex(indexB)) return;

            Item itemA = items[indexA];
            Item itemB = items[indexB];

            if (itemA != null && itemB != null && itemA.Data == itemB.Data && itemA is CountableItem ciA && itemB is CountableItem ciB)
            {
                int maxAmount = ciB.MaxAmount;
                int sum = ciA.Amount + ciB.Amount;

                if (sum <= maxAmount)
                {
                    ciA.SetAmount(0);
                    ciB.SetAmount(sum);
                }
                else
                {
                    ciA.SetAmount(sum - maxAmount);
                    ciB.SetAmount(maxAmount);
                }
            }
            else
            {
                items[indexA] = itemB;
                items[indexB] = itemA;
            }

            UpdateSlot(indexA, indexB);
        }

        public void SeparateAmount(int indexA, int indexB, int amount)
        {
            if (!IsValidIndex(indexA)) return;
            if (!IsValidIndex(indexB)) return;

            Item _itemA = items[indexA];
            Item _itemB = items[indexB];

            CountableItem _ciA = _itemA as CountableItem;

            if (_ciA != null && _itemB == null)
            {
                items[indexB] = _ciA.SeperateAndClone(amount);

                UpdateSlot(indexA, indexB);
            }
        }

        //===슬롯 UI에 접근 가능 여부 업데이트===//
        public void UpdateAccessibleStatesAll()
        {
            inventoryUI.SetAccessibleSlotRange(Capacity);
        }

        //===빈 슬롯 없이 앞에서부터 채우기===//
        public void TrimAll()
        {
            indexSetForUpdate.Clear();

            int i = -1;
            while (items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < Capacity && items[j] == null);

                if (j == Capacity)
                    break;

                indexSetForUpdate.Add(i);
                indexSetForUpdate.Add(j);

                items[i] = items[j];
                items[j] = null;
                i++;
            }

            foreach (var index in indexSetForUpdate)
            {
                UpdateSlot(index);
            }
            inventoryUI.UpdateAllSlotFilters();
        }

        //===빈 슬롯 없이 채우면서 아이템 종류별로 정렬하기===//
        public void SortAll()
        {
            //===Trim===//
            int i = -1;
            while (items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < Capacity && items[j] == null) ;

                if (j == Capacity)
                    break;

                items[i] = items[j];
                items[j] = null;
                i++;
            }

            //===Sort===//
            Array.Sort(items, 0, i, itemCompare);

            UpdateAllSlot();
            inventoryUI.UpdateAllSlotFilters();
        }
    }
}