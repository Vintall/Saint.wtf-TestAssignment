using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputItemPlate : ItemPlate
{
    protected override void OnPlayerCollisionEnter(Collider collider)
    {
        isPlayerOnPlate = true;
        playerCollider = collider;

        if (isTransferCycleActive)
            return;

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
        PlayerInventory playerInventory = collider.gameObject.GetComponent<PlayerInventory>();

        foreach (KeyValuePair<Structures.ResourceType, InventorySlot> plateResourceSlot in Inventory.InventoryStorage)// inventory.Inventory.InventoryStorage)
        {
            Transform playerResourceTransform = playerInventory.GetResourceTransform(plateResourceSlot.Key);
            InventorySlot playerResourceSlot = playerInventory.GetResourceSlot(plateResourceSlot.Key);

            if (playerResourceSlot.ItemsCount == PlayerInventory.CapacityPerSlot)
                continue;

            if (plateResourceSlot.Value.ItemsCount == 0)
                continue;
            
            yield return StartCoroutine(StartTransfering());
                
            IEnumerator StartTransfering()
            {
                isTransferCycleActive = true;
                ResourceItem item = plateResourceSlot.Value.PopItem();

                yield return StartCoroutine(Factory.LerpResource(item, item.transform.position, playerResourceTransform));

                item.gameObject.transform.position = playerResourceTransform.position + Vector3.up * 0.2f * playerResourceSlot.ItemsCount;
                item.gameObject.transform.parent = playerResourceTransform;
                playerResourceSlot.PushItem(item);
            }
        }
        isTransferCycleActive = false;
    }
    
}
