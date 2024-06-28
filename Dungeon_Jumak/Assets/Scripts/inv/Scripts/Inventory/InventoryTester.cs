using UnityEngine;
using UnityEngine.UI;
using DJ.InventorySystem;


public class InventoryTester : MonoBehaviour
{
    public Inventory inventory;

    public ItemData[] itemDataArray;
    public Button removeAllButton;
    public Button AddArmorA;
    public Button AddArmorB;
    public Button AddSwordA;
    public Button AddSwordB;
    public Button AddIngredientsA;

    private void Start()
    {
        removeAllButton.onClick.AddListener(() =>
        {
            int capacity = inventory.Capacity;
            for(int i = 0; i < capacity; i++)
                inventory.Remove(i);
        });

        AddArmorA.onClick.AddListener(() => inventory.Add(itemDataArray[0]));
        AddArmorB.onClick.AddListener(() => inventory.Add(itemDataArray[1]));

        AddSwordA.onClick.AddListener(() => inventory.Add(itemDataArray[2]));
        AddSwordB.onClick.AddListener(() => inventory.Add(itemDataArray[3]));

        AddIngredientsA.onClick.AddListener(() => inventory.Add(itemDataArray[4], 10));
    }

}