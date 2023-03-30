using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Transform[] stackHolders;

    Dictionary<Structures.ResourceType, (Stack<ResourceItem>, Transform)> stackInventory;
    public (Stack<ResourceItem>, Transform) GetResourceInventory(Structures.ResourceType type) => stackInventory[type];
    public const int CapacityPerStack = 20;

    private void Awake()
    {
        stackInventory = new Dictionary<Structures.ResourceType, (Stack<ResourceItem>, Transform)>();

        foreach (Structures.ResourceType type in System.Enum.GetValues(typeof(Structures.ResourceType)))
            stackInventory.Add(type, (new Stack<ResourceItem>(), stackHolders[(int)type]));
    }

}
