using System.Collections.Generic;
using UnityEngine;

// A simple object pool. Put one on an empty GameObject per kind of object.
// Bullet pools get their bullets from the BulletFactory; other pools (like enemies)
// just copy their prefab.
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;          // used when there's no factory (e.g. enemies)
    [SerializeField] private int initialSize = 10;

    [Header("Bullets (optional)")]
    [SerializeField] private BulletFactory factory;      // set this on bullet pools
    [SerializeField] private BulletType bulletType;      // which bullet the factory should build

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
        // Bullets come from the factory; everything else is a copy of the prefab.
        GameObject obj = (factory != null)
            ? factory.CreateBullet(bulletType, transform)
            : Instantiate(prefab, transform);

        obj.SetActive(false);

        // Every pooled object remembers which pool it came from, so it knows where to go back to.
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
        obj.SetActive(true);
        return obj;
    }

    // Hide an object and put it back in the pool for reuse.
    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        available.Enqueue(obj);
    }
}