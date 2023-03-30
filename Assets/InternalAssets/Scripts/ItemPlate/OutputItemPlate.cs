using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputItemPlate : ItemPlate
{
    protected override void OnPlayerCollisionEnter(Collider collider)
    {
        isPlayerOnPlate = true;
        playerCollider = collider;

        isTransferCycleActive = true;
        StartCoroutine(TransferCycle(collider));
    }
    protected override void OnPlayerCollisionExit(Collider collider)
    {
        StopCoroutine(TransferCycle(collider));
        playerCollider = null;
        isPlayerOnPlate = false;
    }
    Collider playerCollider;
    private void Update()
    {
        if (!isPlayerOnPlate)
            return;

        if (isTransferCycleActive)
            return;

        StartCoroutine(TransferCycle(playerCollider));
    }

    bool isTransferCycleActive = false;
    IEnumerator TransferCycle(Collider collider)
    {
        PlayerInventory inventory = collider.gameObject.GetComponent<PlayerInventory>();

        foreach (KeyValuePair<Structures.ResourceType, Stack<ResourceItem>> plateResourceSlot in ResourceSlots)
        {
            (Stack<ResourceItem>, Transform) playerResourceSlot = inventory.GetResourceInventory(plateResourceSlot.Key);

            if (playerResourceSlot.Item1.Count == PlayerInventory.CapacityPerStack)
                continue;

            if (ResourceSlots[plateResourceSlot.Key].Count == 0)
                continue;
            
            yield return StartCoroutine(StartTransfering());
                
            IEnumerator StartTransfering()
            {
                isTransferCycleActive = true;
                ResourceItem item = plateResourceSlot.Value.Pop();

                yield return StartCoroutine(Factory.LerpResource(item, item.transform.position, playerResourceSlot.Item2));

                item.gameObject.transform.position = playerResourceSlot.Item2.position + Vector3.up * 0.2f * playerResourceSlot.Item1.Count;
                item.gameObject.transform.parent = playerResourceSlot.Item2;
                playerResourceSlot.Item1.Push(item);
            }
        }
        isTransferCycleActive = false;
    }
    
}
