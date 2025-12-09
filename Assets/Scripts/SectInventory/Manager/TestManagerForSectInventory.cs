using System;
using System.Collections.Generic;
using SectInventory.Enum;
using SectInventory.Struct;
using UnityEngine;

public class TestManagerForSectInventory : MonoBehaviour {
    public List<ResourceWrapper> inventoryWrappers;

    private SectInventory.SectInventory _sectInventory;
    
    public SectInventory.SectInventory Inventory => _sectInventory;
    
    private void Awake() {
        _sectInventory = new SectInventory.SectInventory(inventoryWrappers);
    }

    private void Update() {if (Input.GetKeyDown(KeyCode.W)) {
            Debug.Log(Inventory.TryAddingToInventory(EResourceType.Mouse, 4));
        } else if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log(Inventory.TryAddingToInventory(EResourceType.Wood, 230));
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log(Inventory.TryGettingFromInventory(EResourceType.Mouse, 5));
        } else if (Input.GetKeyDown(KeyCode.F)) {
            Debug.Log(Inventory.TryGettingFromInventory(EResourceType.Wood, 3));
        }
    }
}
