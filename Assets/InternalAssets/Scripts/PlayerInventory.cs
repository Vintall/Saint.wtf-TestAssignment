using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Transform[] stackHolders;
    Dictionary<Structures.ResourceType, Transform> stackHoldersDictionary;

    public const int CapacityPerSlot = 20;
    Inventory inventory;

    public Inventory Inventory => inventory;
    //public Structures.ResourceType AllSlotTypes => inventory. 
    public InventorySlot GetResourceSlot(Structures.ResourceType type) => inventory[type];
    public Transform GetResourceTransform(Structures.ResourceType type) => stackHoldersDictionary[type];
    
    private void Awake()
    {
        stackHoldersDictionary = new Dictionary<Structures.ResourceType, Transform>();

        foreach (Structures.ResourceType resourceType in Structures.AllResourceTypes)
            stackHoldersDictionary.Add(resourceType, stackHolders[(int)resourceType]);
        
        inventory = new Inventory(CapacityPerSlot, Structures.AllResourceTypes);
    }

}
