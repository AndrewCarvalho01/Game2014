using UnityEngine;
using UnityEngine.SceneManagement;
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private string gameOverScene = "End";
    private Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        float bottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
        if (transform.position.y < bottom - 1f)
        {
            GetComponent<PooledObject>().ReturnToPool();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerBehavior>() != null)
        {
            GetComponent<PooledObject>().ReturnToPool();
            SceneManager.LoadScene(gameOverScene);
        }
    }
}