using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Dictionary<Structures.ResourceType, InventorySlot> inventoryStorage;

    public Dictionary<Structures.ResourceType, InventorySlot> InventoryStorage => inventoryStorage;
    public bool ContainsKey(Structures.ResourceType resourceType) => inventoryStorage.ContainsKey(resourceType);
    public InventorySlot this[Structures.ResourceType resourceType]
    {
        get
        {
            if (inventoryStorage == null)
                return null;

            if (!inventoryStorage.ContainsKey(resourceType))
                return null;

            return inventoryStorage[resourceType];
        }
    }
    public Inventory(int capacityPerSlot, List<Structures.ResourceType> slotTypes)
    {
        inventoryStorage = new Dictionary<Structures.ResourceType, InventorySlot>();

        foreach (Structures.ResourceType resourceType in slotTypes)
            inventoryStorage.Add(resourceType, new InventorySlot(capacityPerSlot, resourceType));
    }
    public ResourceItem PopItem(Structures.ResourceType resourceType)
    {
        if (inventoryStorage[resourceType].ItemsCount == 0)
            return null;

        return inventoryStorage[resourceType].PopItem();
    }
    public void PushItem(ResourceItem item)
    {
        if (!inventoryStorage.ContainsKey(item.Type))
            return;

        if (inventoryStorage[item.Type].IsFull)
            return;

        inventoryStorage[item.Type].PushItem(item);
    }
}
