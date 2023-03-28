using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] Structures.ResourceType type;
    public Structures.ResourceType Type => type;

}
