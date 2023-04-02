using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactoryPreset", menuName = "ScriptableObjects/FactoryPreset")]
public class FactoryPresetScriptableObject : ScriptableObject
{
    [SerializeField] string factoryName;
    [SerializeField] float produceTime;
    [SerializeField] List<Structures.ResourceWithQuantity> requirements;
    [SerializeField] List<Structures.ResourceWithQuantity> produce;

    public float ProduceTime => produceTime;
    public string FactoryName => factoryName;
    public List<Structures.ResourceWithQuantity> Requirements => requirements;
    public List<Structures.ResourceWithQuantity> Produce => produce;
}
