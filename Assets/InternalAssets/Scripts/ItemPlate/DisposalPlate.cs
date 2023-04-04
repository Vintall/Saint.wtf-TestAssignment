using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposalPlate : ItemPlate
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
        PlayerInventory playerInventory = collider.gameObject.GetComponent<PlayerInventory>();

        foreach (Structures.ResourceType resourceType in Structures.AllResourceTypes)
        {
            InventorySlot playerResourceSlot = playerInventory.GetResourceSlot(resourceType);
            Transform playerResourceTransform = playerInventory.GetResourceTransform(resourceType);

            if (playerResourceSlot.ItemsCount == 0)
                continue;

            yield return StartCoroutine(StartTransfering());

            IEnumerator StartTransfering()
            {
                isTransferCycleActive = true;
                ResourceItem item = playerResourceSlot.PopItem();

                yield return StartCoroutine(Factory.LerpResource(item, item.transform.position, transform.position
                    + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f))));

                item.gameObject.transform.parent = transform;

                ResourcesPool.PlaceResource(item);

                yield return null;
            }
            playerInventory.GetResourceSlot(resourceType); // Dumb UI fix...
        }
        isTransferCycleActive = false;
    }
}
