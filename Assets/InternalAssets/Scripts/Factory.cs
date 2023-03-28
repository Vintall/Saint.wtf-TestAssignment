using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class Factory : MonoBehaviour
{
    //Type - quantity
    [SerializeField] List<Structures.ResourceWithQuantity> requirements;
    [SerializeField] List<Structures.ResourceWithQuantity> produce;

    [SerializeField] ItemPlate inputPlate;
    [SerializeField] ItemPlate outputPlate;

    [SerializeField] float produceTime;

    void StartProducing()
    {

    }


}
