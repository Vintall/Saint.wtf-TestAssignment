using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourcesPool : MonoBehaviour
{
    Dictionary<Structures.ResourceType, GameObject> assetHolderDictionary = null;
    Dictionary<Structures.ResourceType, Queue<ResourceItem>> pool;
    Dictionary<Structures.ResourceType, Transform> objectHolders;

    static ResourcesPool instance;
    public static ResourcesPool Instance => instance;


    int resourcesInitComplete = 0;
    public bool IsInitDone => resourcesInitComplete == System.Enum.GetValues(typeof(Structures.ResourceType)).Length;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        assetHolderDictionary = new Dictionary<Structures.ResourceType, GameObject>();
        pool = new Dictionary<Structures.ResourceType, Queue<ResourceItem>>();
        objectHolders = new Dictionary<Structures.ResourceType, Transform>();

        foreach (Structures.ResourceType type in System.Enum.GetValues(typeof(Structures.ResourceType)))
        {
            Addressables.LoadAssetAsync<GameObject>($"{type}Item").Completed +=
                handle =>
                {
                    assetHolderDictionary.Add(type, handle.Result);
                    ++resourcesInitComplete;
                };

            pool.Add(type, new Queue<ResourceItem>());
            objectHolders.Add(type, new GameObject($"{type}Pool").transform);
            objectHolders[type].parent = transform;
        }
    }
    public static void PlaceResource(ResourceItem item) => instance._PlaceResource(item);
    public void _PlaceResource(ResourceItem item)
    {
        if (pool == null)
            Debug.LogError($"Overall pool is not defined");

        if (pool[item.Type] == null)
            Debug.LogError($"{item.Type} pool is not defined");

        item.gameObject.SetActive(false);

        item.transform.parent = objectHolders[item.Type];
        pool[item.Type].Enqueue(item);
    }

    /// <summary>
    /// Notice, that returned object is gonna be deactivated.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ResourceItem PullResource(Structures.ResourceType type) => instance._PullResource(type);
    public ResourceItem _PullResource(Structures.ResourceType type)
    {
        if (!IsInitDone)
            return null;


        if (pool == null)
            Debug.LogError($"Overall pool is not defined");

        if (pool[type] == null)
            Debug.LogError($"{type} pool is not defined");

        if (pool[type].Count == 0)
            CreateNewResource(type);

        return pool[type].Dequeue();
    }
    void CreateNewResource(Structures.ResourceType type)
    {
        GameObject newObject = Instantiate(assetHolderDictionary[type]);
        newObject.SetActive(false);

        _PlaceResource(newObject.GetComponent<ResourceItem>());
    }
}
