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

    //bool isProducing = false;

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
            outputPlate.InitPlate(this, Structures.ItemPlateType.Output, produce);


        StartCoroutine(RequirementsCheck());
    }
    IEnumerator RequirementsCheck()
    {
        yield return new WaitUntil(() => IsHaveRequirements);
        yield return new WaitUntil(() => IsHaveSpace);
        //yield return new WaitUntil(() => !isProducing);

        StartCoroutine(ProducingCycle());
    }
    IEnumerator ProducingCycle()
    {
        yield return StartCoroutine(RemoveInput());

        yield return new WaitForSeconds(produceTime);

        yield return StartCoroutine(AddOutput());
    }
    IEnumerator RemoveInput()
    {
        if (requirements == null)
            yield return null;

        foreach (Structures.ResourceWithQuantity product in requirements)
            for (int i = 0; i < product.Quantity; ++i)
            {
                ResourceItem item = inputPlate.ResourceSlots[product.Type].Pop();

                StartCoroutine(LerpResource(item, item.gameObject.transform.position, transform.position));

                yield return new WaitForSeconds(1);

                ResourcesPool.PlaceResource(item);
            }
    }
    IEnumerator AddOutput()
    {
        if (produce == null)
            yield return null;

        foreach (Structures.ResourceWithQuantity product in produce)
            for (int i = 0; i < product.Quantity; ++i)
            {
                ResourceItem item = ResourcesPool.PullResource(product.Type);

                item.gameObject.transform.position = transform.position;
                item.gameObject.transform.parent = outputPlate.transform;
                item.gameObject.SetActive(true);

                StartCoroutine(LerpResource(item, transform.position, outputPlate.transform.position
                     + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f))));

                yield return new WaitForSeconds(1);
                outputPlate.ResourceSlots[product.Type].Push(item);
            }

        StartCoroutine(RequirementsCheck());
        yield return null;
    }
    public static IEnumerator LerpResource(ResourceItem item, Vector3 a, Vector3 b)
    {
        for (int i = 0; i < 60; i++)
        {
            item.transform.position = Vector3.Lerp(a, b, i / 60f);
            yield return new WaitForSecondsRealtime(1 / 60f);
        }
    }
    public static IEnumerator LerpResource(ResourceItem item, Vector3 a, Transform b)
    {
        for (int i = 0; i < 60; i++)
        {
            item.transform.position = Vector3.Lerp(a, b.position, i / 60f);
            yield return new WaitForSecondsRealtime(1 / 60f);
        }
    }
    public static IEnumerator LerpResource(ResourceItem item, Transform a, Vector3 b)
    {
        for (int i = 0; i < 60; i++)
        {
            item.transform.position = Vector3.Lerp(a.position, b, i / 60f);
            yield return new WaitForSecondsRealtime(1 / 60f);
        }
    }
}
