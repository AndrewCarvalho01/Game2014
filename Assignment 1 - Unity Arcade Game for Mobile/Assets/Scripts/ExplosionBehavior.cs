using UnityEngine;

// Plays the bubble-burst animation once, then removes itself.
public class ExplosionBehavior : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}