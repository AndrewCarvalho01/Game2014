using System.Collections.Generic;
using UnityEngine;
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;
    // The objects that are currently available for use.
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
    // Return an object to the pool.
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        available.Enqueue(obj);
    }
}