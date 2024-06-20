using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DJ
{
    public class InventroyOnOff : MonoBehaviour
    {
        public GameObject Inventory;

        public void InventoryOn()
        {
            Inventory.gameObject.SetActive(true);
        }

        public void InventoryOff()
        {
            Inventory.gameObject.SetActive(false);
        }


    }
}

