using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputItemPlate : ItemPlate
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
        
        foreach (KeyValuePair<Structures.ResourceType, InventorySlot> plateResourceSlot in Inventory.InventoryStorage)
        {
            InventorySlot playerResourceSlot = playerInventory.GetResourceSlot(plateResourceSlot.Key);
            Transform playerResourceTransform = playerInventory.GetResourceTransform(plateResourceSlot.Key);

            if (plateResourceSlot.Value.ItemsCount == CapacityPerItem)
                continue;

            if (playerResourceSlot.ItemsCount == 0)
                continue;

            yield return StartCoroutine(StartTransfering());

            IEnumerator StartTransfering()
            {
                isTransferCycleActive = true;
                ResourceItem item = playerResourceSlot.PopItem();

                yield return StartCoroutine(Factory.LerpResource(item, item.transform.position, transform.position
                    + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f))));
                
                item.gameObject.transform.parent = transform; // Bring it up to 'before the lerp'
                plateResourceSlot.Value.PushItem(item);

                yield return null;
            }
            playerInventory.GetResourceSlot(plateResourceSlot.Key); // Dumb UI fix...
        }
        isTransferCycleActive = false;
    }
}
