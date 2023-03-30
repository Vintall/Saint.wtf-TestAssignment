using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputItemPlate : ItemPlate
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

            if (plateResourceSlot.Value.Count == CapacityPerItem)
                continue;

            if (playerResourceSlot.Item1.Count == 0)
                continue;

            yield return StartCoroutine(StartTransfering());

            IEnumerator StartTransfering()
            {
                isTransferCycleActive = true;
                ResourceItem item = playerResourceSlot.Item1.Pop();

                yield return StartCoroutine(Factory.LerpResource(item, item.transform.position, transform.position
                    + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f))));

                item.gameObject.transform.parent = transform;
                plateResourceSlot.Value.Push(item);

                yield return null;
            }
        }
        isTransferCycleActive = false;
    }
}
