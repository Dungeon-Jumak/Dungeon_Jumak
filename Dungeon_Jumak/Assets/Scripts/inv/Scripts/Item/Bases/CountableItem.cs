using UnityEngine;

namespace DJ.InventorySystem
{
    public abstract class CountableItem : Item
    {
        public CountableItemData CountableData { get; private set; }

        //===현재 아이템 개수===//
        public int Amount { get; protected set; }

        //===슬롯이 가질 수 있는 아이템 Max 값 99===//
        public int MaxAmount => CountableData.MaxAmount;

        //===Max 값을 넘었는가?===//
        public bool IsMax => Amount >= CountableData.MaxAmount;

        //===개수가 없는가?===//
        public bool IsEmpty => Amount <= 0;


        public CountableItem(CountableItemData data, int amount = 1) : base(data)
        {
            CountableData = data;
            SetAmount(amount);
        }

        //==범위 제한===//
        public void SetAmount(int amount)
        {
            Amount = Mathf.Clamp(amount, 0, MaxAmount);
        }

        //===개수 추가 및 최대치 초과량 반환(초과량 없을 경우 0)===//
        public int AddAmountAndGetExcess(int amount)
        {
            int nextAmount = Amount + amount;
            SetAmount(nextAmount);

            return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
        }

        public CountableItem SeperateAndClone(int amount)
        {
            //===수량이 한개 이하일 경우, 나누기 불가===//
            if(Amount <= 1) return null;

            if(amount > Amount - 1)
                amount = Amount - 1;

            Amount -= amount;
            return Clone(amount);
        }

        protected abstract CountableItem Clone(int amount);
    }
}