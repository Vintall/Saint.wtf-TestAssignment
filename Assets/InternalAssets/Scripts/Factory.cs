using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class Factory : MonoBehaviour
{
    [SerializeField] List<Structures.ResourceWithQuantity> requirements;
    [SerializeField] List<Structures.ResourceWithQuantity> produce;

    [SerializeField] ItemPlate inputPlate;
    [SerializeField] ItemPlate outputPlate;

    [SerializeField] float produceTime;

    bool isProducing = false;

    bool IsHaveSpace
    {
        get
        {
            foreach (Structures.ResourceWithQuantity product in produce)
                if (outputPlate.SpaceRemained(product.Type) < product.Quantity)
                    return false;

            return true;
        }
    }
    bool IsHaveRequirements
    {
        get
        {
            foreach (Structures.ResourceWithQuantity product in requirements)
                if (inputPlate.ResourceSlots[product.Type].Count < product.Quantity)
                    return false;

            return true;
        }
    }

    private void Start()
    {
        if (inputPlate != null)
            inputPlate.InitPlate(this, Structures.ItemPlateType.Input, requirements);

        if (outputPlate != null)
            outputPlate.InitPlate(this, Structures.ItemPlateType.Output, requirements);


        StartCoroutine(RequirementsCheck());
    }
    IEnumerator RequirementsCheck()
    {
        yield return new WaitUntil(() => IsHaveRequirements);
        yield return new WaitUntil(() => IsHaveSpace);
        yield return new WaitUntil(() => isProducing);

        StartCoroutine(ProducingCycle());
    }
    IEnumerator ProducingCycle()
    {
        RemoveInput();

        yield return new WaitForSeconds(produceTime);

        AddOutput();


    }
    void RemoveInput()
    {
        if (requirements == null)
            return;
    }
    void AddOutput()
    {
        if (produce == null)
            return;

        foreach (Structures.ResourceWithQuantity product in produce)
        {

            ResourcesPool.PullResource(product.Type);
        }
    }
}
