using UnityEngine;
using UnityEngine.UI;
using DJ.InventorySystem;


public class InventoryTester : MonoBehaviour
{
    public Inventory _inventory;

    public ItemData[] _itemDataArray;
    public Button _removeAllButton;
    public Button _AddArmorA1;
    public Button _AddArmorB1;
    public Button _AddSwordA1;
    public Button _AddSwordB1;

    private void Start()
    {
        _removeAllButton.onClick.AddListener(() =>
        {
            int capacity = _inventory.Capacity;
            for(int i = 0; i < capacity; i++)
                _inventory.Remove(i);
        });

        _AddArmorA1.onClick.AddListener(() => _inventory.Add(_itemDataArray[0]));
        _AddArmorB1.onClick.AddListener(() => _inventory.Add(_itemDataArray[1]));

        _AddSwordA1.onClick.AddListener(() => _inventory.Add(_itemDataArray[2]));
        _AddSwordB1.onClick.AddListener(() => _inventory.Add(_itemDataArray[3]));
    }

}