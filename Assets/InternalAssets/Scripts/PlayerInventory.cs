using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Transform[] stackHolders;
    [SerializeField]
    List<TextMeshProUGUI> uiLogger;


    Dictionary<Structures.ResourceType, Transform> stackHoldersDictionary;
    public const int CapacityPerSlot = 20;

    Inventory inventory;
    

    public Inventory Inventory => inventory;

    public InventorySlot GetResourceSlot(Structures.ResourceType type)
    {
        uiLogger[(int)type].text = $"{inventory[type].ItemsCount} / {CapacityPerSlot}";

        return inventory[type];
    }

    public Transform GetResourceTransform(Structures.ResourceType type) => stackHoldersDictionary[type];
    
    private void Awake()
    {
        stackHoldersDictionary = new Dictionary<Structures.ResourceType, Transform>();

        foreach (Structures.ResourceType resourceType in Structures.AllResourceTypes)
            stackHoldersDictionary.Add(resourceType, stackHolders[(int)resourceType]);
        
        inventory = new Inventory(CapacityPerSlot, Structures.AllResourceTypes);
    }

}
