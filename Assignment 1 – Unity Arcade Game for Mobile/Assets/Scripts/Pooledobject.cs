using UnityEngine;

// Added automatically by ObjectPool. Lets an object send itself back to its pool.
public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPool pool;

    public void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            // Not from a pool (for example, placed in the scene by hand).
            Destroy(gameObject);
        }
    }
}