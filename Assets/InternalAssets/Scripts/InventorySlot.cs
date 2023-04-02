using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    Structures.ResourceType resourceType;
    Stack<ResourceItem> stack;
    int capacity;

    public bool IsFull => ItemsCount == SlotCapacity;
    public int SpaceRemained => SlotCapacity - stack.Count;
    public int SlotCapacity => capacity;
    public int ItemsCount => stack.Count;
    public InventorySlot(int capacity, Structures.ResourceType resourceType)
    {
        this.resourceType = resourceType;
        stack = new Stack<ResourceItem>(capacity);
        this.capacity = capacity;
    }
    public ResourceItem PopItem()
    {
        if (ItemsCount == 0)
            return null;

        return stack.Pop();
    }
    public void PushItem(ResourceItem item)
    {
        stack.Push(item);
    }
}
