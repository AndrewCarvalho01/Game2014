using UnityEngine;
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
            Destroy(gameObject);
        }
    }
}