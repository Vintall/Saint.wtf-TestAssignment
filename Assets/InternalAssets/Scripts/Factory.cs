using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;


public class Factory : MonoBehaviour
{
    [SerializeField] FactoryPresetScriptableObject factoryPreset;

    [SerializeField] ItemPlate inputPlate;
    [SerializeField] ItemPlate outputPlate;

    [SerializeField] TextMeshProUGUI uiLogger;

    bool IsHaveSpace
    {
        get
        {
            if (outputPlate == null)
                return true;

            foreach (Structures.ResourceWithQuantity product in factoryPreset.Produce)
                if (outputPlate.SpaceRemained(product.Type) < product.Quantity)
                {
                    if (uiLogger != null)
                        uiLogger.text = "Require More Space";

                    return false;
                }

            return true;
        }
    }
    bool IsHaveRequirements
    {
        get
        {
            if (inputPlate == null)
                return true;

            foreach (Structures.ResourceWithQuantity requirement in factoryPreset.Requirements)
                if (inputPlate.Inventory[requirement.Type].ItemsCount < requirement.Quantity)
                {
                    if (uiLogger != null)
                        uiLogger.text = $"Require More {requirement.Type.ToString()}s";

                    return false;
                }

            return true;
        }
    }
    public void InitFactory()
    {
        if (inputPlate != null)
            inputPlate.InitPlate(factoryPreset.Requirements);

        if (outputPlate != null)
            outputPlate.InitPlate(factoryPreset.Produce);

        StartCoroutine(RequirementsCheck());
    }
    private void Start() => InitFactory();
    IEnumerator RequirementsCheck()
    {
        yield return new WaitUntil(() => IsHaveRequirements);
        yield return new WaitUntil(() => IsHaveSpace);

        if (uiLogger != null)
            uiLogger.text = "Up and Running";

        StartCoroutine(ProducingCycle());
    }
    IEnumerator ProducingCycle()
    {
        yield return StartCoroutine(RemoveInput());

        yield return new WaitForSeconds(factoryPreset.ProduceTime);

        yield return StartCoroutine(AddOutput());
    }
    IEnumerator RemoveInput()
    {
        if (factoryPreset.Requirements == null)
            yield return null;

        foreach (Structures.ResourceWithQuantity product in factoryPreset.Requirements)
            for (int i = 0; i < product.Quantity; ++i)
            {
                ResourceItem item = inputPlate.Inventory[product.Type].PopItem();

                yield return StartCoroutine(LerpResource(item, item.gameObject.transform.position, transform.position));

                ResourcesPool.PlaceResource(item);
            }
    }
    IEnumerator AddOutput()
    {
        if (factoryPreset.Produce == null)
            yield return null;

        foreach (Structures.ResourceWithQuantity product in factoryPreset.Produce)
            for (int i = 0; i < product.Quantity; ++i)
            {
                ResourceItem item = ResourcesPool.PullResource(product.Type);
                //Debug.Log(item);

                item.gameObject.transform.position = transform.position;
                item.gameObject.transform.parent = outputPlate.transform;
                item.gameObject.SetActive(true);

                yield return StartCoroutine(LerpResource(item, transform.position, outputPlate.transform.position
                     + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f))));

                outputPlate.Inventory[product.Type].PushItem(item);
            }

        StartCoroutine(RequirementsCheck());
        yield return null;
    }

    const float lerpTransferSpeed = 600;
    const int lerpSteps = 30;
    const float lerpStepAwait = 1f / lerpSteps / lerpTransferSpeed;
    public static IEnumerator LerpResource(ResourceItem item, Vector3 a, Vector3 b)
    {
        for (int i = 0; i < lerpSteps; ++i)
        {
            item.transform.position = Vector3.Lerp(a, b, (float)i / lerpSteps);
            yield return new WaitForSecondsRealtime(lerpStepAwait);
        }
    }
    public static IEnumerator LerpResource(ResourceItem item, Vector3 a, Transform b)
    {
        for (int i = 0; i < lerpSteps; ++i)
        {
            item.transform.position = Vector3.Lerp(a, b.position, (float)i / lerpSteps);
            yield return new WaitForSecondsRealtime(lerpStepAwait);
        }
    }
    public static IEnumerator LerpResource(ResourceItem item, Transform a, Vector3 b)
    {
        for (int i = 0; i < lerpSteps; ++i)
        {
            item.transform.position = Vector3.Lerp(a.position, b, (float)i / lerpSteps);
            yield return new WaitForSecondsRealtime(lerpStepAwait);
        }
    }
}
