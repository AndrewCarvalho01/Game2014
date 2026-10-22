using System.Collections.Generic;
using UnityEngine;

// A simple object pool. Put one on an empty GameObject per kind of object
// (one for enemies, one for bullets) and drag the prefab into the slot.
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    // Hidden objects waiting to be used.
    private readonly Queue<GameObject> available = new Queue<GameObject>();

    void Awake()
    {
        // Create all the objects once, at the start, and hide them.
        for (int i = 0; i < initialSize; i++)
        {
            available.Enqueue(CreateNew());
        }
    }

    private GameObject CreateNew()
    {
        // Created as a child of the pool to keep the Hierarchy tidy.
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);

        // Every pooled object remembers which pool it came from,
        // so it knows where to go back to.
        PooledObject pooled = obj.GetComponent<PooledObject>();
        if (pooled == null)
        {
            pooled = obj.AddComponent<PooledObject>();
        }
        pooled.pool = this;

        return obj;
    }

    // Take an object out of the pool and place it at a position.
    public GameObject Get(Vector3 position)
    {
        // If every object is already in use, make one more.
        GameObject obj = available.Count > 0 ? available.Dequeue() : CreateNew();

        obj.transform.position = position;
        obj.SetActive(true); // triggers OnEnable on the object's scripts
        return obj;
    }

    // Hide an object and put it back in the pool for reuse.
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        available.Enqueue(obj);
    }
}