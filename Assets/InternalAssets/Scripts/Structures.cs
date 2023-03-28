using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures
{
    public enum ResourceType
    {
        Apple,
        Watermelon,
        Peach
    }
    [System.Serializable]
    public struct ResourceWithQuantity
    {
        [SerializeField] ResourceType type;
        [SerializeField] int quantity;

        public ResourceType Type => type;
        public int Quantity => quantity;
        public ResourceWithQuantity(ResourceType type, int quantity)
        {
            this.type = type;
            this.quantity = quantity;
        }
    }
    public enum ItemPlateType
    {
        Input,
        Output
    }
    [System.Serializable]
    public struct ResourceTypeWithAsset
    {
        [SerializeField] ResourceType type;
        [SerializeField] GameObject gameObject;

        public ResourceType Type => type;
        public GameObject Object => gameObject;
    }
}
