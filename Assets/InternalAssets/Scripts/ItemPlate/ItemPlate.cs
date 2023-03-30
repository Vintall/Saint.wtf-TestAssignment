using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlate : MonoBehaviour
{
    [SerializeField] int capacityPerItem;
    public int CapacityPerItem => capacityPerItem;
    protected bool isPlayerOnPlate;

    Structures.ItemPlateType plateType;
    Dictionary<Structures.ResourceType, Stack<ResourceItem>> resourceSlots;
    public Dictionary<Structures.ResourceType, Stack<ResourceItem>> ResourceSlots => resourceSlots;

    Factory parentFactory;

    public void InitPlate(in Factory parentFactory, Structures.ItemPlateType plateType, List<Structures.ResourceWithQuantity> resourceTypes)
    {
        this.parentFactory = parentFactory;
        this.plateType = plateType;
        this.resourceSlots = new Dictionary<Structures.ResourceType, Stack<ResourceItem>>();

        foreach (Structures.ResourceWithQuantity resourceType in resourceTypes)
            resourceSlots.Add(resourceType.Type, new Stack<ResourceItem>());
    }
    public int SpaceRemained(Structures.ResourceType type)
    {
        if (!resourceSlots.ContainsKey(type))
            return -1;

        return CapacityPerItem - resourceSlots[type].Count;
    }
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
