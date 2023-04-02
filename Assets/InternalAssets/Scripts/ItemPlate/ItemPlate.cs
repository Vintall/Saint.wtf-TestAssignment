using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlate : MonoBehaviour
{
    [SerializeField] int capacityPerSlot;
    public int CapacityPerItem => capacityPerSlot;
    protected bool isPlayerOnPlate;

    //Dictionary<Structures.ResourceType, Stack<ResourceItem>> resourceSlots;

    Inventory inventory;
    public Inventory Inventory => inventory;
    //public Dictionary<Structures.ResourceType, Stack<ResourceItem>> ResourceSlots => resourceSlots;

    public void InitPlate(List<Structures.ResourceType> resourceTypes) => inventory = new Inventory(capacityPerSlot, resourceTypes);
    public void InitPlate(List<Structures.ResourceWithQuantity> resourceTypes)
    {
        List<Structures.ResourceType> bufferList = new List<Structures.ResourceType>();

        foreach (Structures.ResourceWithQuantity item in resourceTypes)
            bufferList.Add(item.Type);

        InitPlate(bufferList);
    }

    public int? SpaceRemained(Structures.ResourceType type) => inventory[type]?.SpaceRemained;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
            OnPlayerCollisionEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            OnPlayerCollisionExit(other);
    }
    protected virtual void OnPlayerCollisionEnter(Collider collider) => Debug.Log($"Virtual Metod Called");

    protected virtual void OnPlayerCollisionExit(Collider collider) => Debug.Log($"Virtual Metod Called");
}
