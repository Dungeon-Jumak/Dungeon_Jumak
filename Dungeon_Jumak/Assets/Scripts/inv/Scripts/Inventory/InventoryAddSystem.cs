using DJ.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAddSystem : MonoBehaviour
{
    private Data data;

    public Inventory inventory;

    public ItemData[] itemDataArray;



    private void Awake()
    {
        data = DataManager.Instance.data;
    }

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < data.ingredient.Length; i++)
        {
            for (int j = 0; j < data.ingredient[i]; j++)
            {
                inventory.Add(itemDataArray[i], 1);
            }
        }
    }
}
